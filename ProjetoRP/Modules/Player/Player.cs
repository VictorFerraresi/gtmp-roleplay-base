using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared.Math;
using ProjetoRP.Business.Player;
using ProjetoRP.Entities;
using ProjetoRP.Types;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetoRP.Business.Property;
using ProjetoRP.Business.Vehicle;
using ProjetoRP.Business.Faction;
using ProjetoRP.Business;
using System.Data.Entity.Validation;
using System.Diagnostics;


namespace ProjetoRP.Modules.Player
{
    public class Player : Script
    {
        private PropertyBLL PropBLL = new PropertyBLL();
        private DoorBLL DoorBLL = new DoorBLL();
        private PlayerBLL PlayerBLL = new PlayerBLL();
        private VehicleBLL VehBLL = new VehicleBLL();
        private FactionBLL FacBLL = new FactionBLL();

        const int NULL_DIMENSION = int.MaxValue;
        const int MAX_LOGIN_TRIES = 3;
        const int MAX_CHARACTERS_PER_PLAYER = 3;

        public Player()
        {
            API.onResourceStart += OnResourceStart;
            API.onPlayerConnected += OnPlayerConnected;
            API.onPlayerDisconnected += OnPlayerDisconnected;
            API.onClientEventTrigger += OnClientEventTrigger;
            API.onChatMessage += OnChatMessage;
        }

        private void OnPlayerDisconnected(Client player, string reason)
        {
            var ac = ActivePlayer.GetSpawned(player);
            if (null != ac) // Means that the player was spawned (has character instantiated)
            {

                try
                {
                    Player_Save(player);
                }
                finally 
                {
                    ac.Dispose(); // Removing from AC pool
                }
                
            }
        }

        public void OnResourceStart()
        {
            API.consoleOutput(Messages.console_startup);
        }

        private void OnPlayerConnected(Client player)
        {
            var ac = new ActivePlayer(player);
            ac.Status = PlayerStatus.PreLoad; // Redundant but healthy

            API.setEntityDimension(player.handle, NULL_DIMENSION);
            API.consoleOutput("Player (" + ac.Id + ") " + player.socialClubName + " connected.");

            if (player.isCEFenabled)
            {
                player.setData("PLAYER_IS_CEF_ENABLED", true);
            }
            else
            {
                API.sendChatMessageToPlayer(player, Messages.player_cef_is_disabled);
                player.setData("PLAYER_IS_CEF_ENABLED", false);
            }
        }

        public void OnClientEventTrigger(Client player, string eventName, object[] args)
        {
            var ac = ActivePlayer.Get(player);
            switch (eventName)
            {
                case "CS_PLAYER_PRELOAD_READY":
                    if (ac.Status == PlayerStatus.PreLoad)
                    {
                        bool isCefEnabled = player.getData("PLAYER_IS_CEF_ENABLED");

                        API.sendChatMessageToPlayer(player, Messages.player_welcome_message);
                        if (isCefEnabled)
                        {
                            API.sendChatMessageToPlayer(player, Messages.player_please_login);
                        }
                        else
                        {
                            API.sendChatMessageToPlayer(player, Messages.player_please_login_nocef);
                        }

                        ac.Status = PlayerStatus.Login;
                        player.setData("PLAYER_LOGIN_TRIES_COUNT", 0);
                    }
                    else
                    {
                        Player_KickForInvalidTrigger(player);
                    }
                    break;
                case "CS_UI_PRELOAD_READY":
                    if (player.getData("PLAYER_IS_CEF_ENABLED"))
                    {
                        API.call("Ui", "fixCursor", player, true);
                        API.call("Ui", "evalUi", player, "login_app.display=true;");
                    }
                    break;
                case "CS_LOGIN_SUBMIT":
                    var data = API.fromJson((string)args[0]);

                    Player_Login(player, (string)data.user, (string)data.pass);
                    break;
                case "CS_CHARSEL_SWITCH":
                    if (ac.Status != PlayerStatus.CharacterSelection)
                    {
                        Player_KickForInvalidTrigger(player);
                        return;
                    }

                    var switch_data = API.fromJson((string)args[0]);
                    List<Character> lcd = player.getData("PLAYER_CHARSEL_DATA");

                    Character selected = lcd.Single(x => x.Id == (int)switch_data.character_id);

                    PedHash pedHash;
                    Enum.TryParse(selected.Skin, out pedHash);

                    player.setSkin(pedHash);
                    break;
                case "CS_CHARSEL_SUBMIT":
                    if (ac.Status != PlayerStatus.CharacterSelection)
                    {
                        Player_KickForInvalidTrigger(player);
                        return;
                    }

                    var submit_data = API.fromJson((string)args[0]);
                    int cid = (int)submit_data.character_id;

                    if (cid == 0)
                    {
                        // suspicious
                        API.consoleOutput("Player " + player.socialClubName + " tried to spawn cid 0");
                        return;
                    }
                    API.call("Ui", "evalUi", player, "charsel_app.display=false;");

                    Player_Spawn(player, cid);
                    break;

                case "CS_RETURN_LOGOUT_AREA":                    
                    var street = args[0];
                    var area = args[1];

                    string areaFormat = String.Format("{0}, {1}", street, area);
                    ac.Character.LogoutArea = areaFormat;                    
                    break;
            }
        }

        private void Player_Login(Client sender, string user, string password)
        {
            var ac = ActivePlayer.Get(sender);
            if (ac.Status == PlayerStatus.Login)
            {
                int player_id;
                string hashed;
                bool isCefEnabled = sender.getData("PLAYER_IS_CEF_ENABLED");                

                using (var context = new DatabaseContext())
                {
                    var player = (from p in context.Players where p.Name == user select p).SingleOrDefault();
                    if (player != null)
                    {
                        player_id = player.Id;
                        hashed = player.Password;

                        bool test = BCrypt.Net.BCrypt.Verify(password, hashed);

                        if (test)
                        {
                            if (isCefEnabled)
                            {
                                API.call("Ui", "evalUi", sender, "login_app.display=false;");
                            }
                            sender.setData("PLAYER_LOGIN_TRIES_COUNT", 0);
                            sender.setData("id", player_id);
                            sender.setData("name", user);

                            try
                            {
                                var last_successful_session = (from ls in context.Sessions where ls.Player.Id == player.Id && ls.Failed == false select ls).OrderByDescending(x => x.LoginAt).First();
                                var sessions = (from ss in context.Sessions where ss.Player.Id == player.Id && ss.LoginAt > last_successful_session.LoginAt select ss);

                                var text = String.Format(Messages.player_last_logins, last_successful_session.LoginAt, last_successful_session.Ip, sessions.Count());
                                sender.sendChatMessage(text);
                                sender.sendChatMessage(Messages.player_login_success);
                            }
                            catch (System.InvalidOperationException) { }
                            catch (System.ArgumentNullException) { }

                            Entities.Session s = new Entities.Session
                            {
                                Player = player,
                                Failed = false,
                                Ip = sender.address,
                                LoginAt = DateTime.UtcNow,
                                Rgsc = sender.socialClubName
                            };

                            context.Sessions.Add(s);
                            context.SaveChanges();

                            sender.setData("PLAYER_SESSION", s.Id);

                            Player_LoadData(sender);
                            Player_SetCharacterSelection(sender);
                            return;
                        }
                        else
                        {
                            Entities.Session s = new Entities.Session
                            {
                                Player = player,
                                Failed = true,
                                Ip = sender.address,
                                LoginAt = DateTime.UtcNow,
                                Rgsc = sender.socialClubName
                            };

                            context.Sessions.Add(s);
                        }
                    }
                    int tries = sender.getData("PLAYER_LOGIN_TRIES_COUNT") + 1;

                    string msg = string.Format(Messages.player_wrong_password, tries, MAX_LOGIN_TRIES);
                    if (isCefEnabled)
                    {
                        API.call("Ui", "evalUi", sender, "login_app.blocked=false;login_app.error='" + msg + "';");
                    }
                    else
                    {
                        sender.sendChatMessage(msg);
                    }

                    if (tries >= MAX_LOGIN_TRIES)
                    {
                        sender.kick("Exceeded amount of allowed login tries.");
                    }
                    else
                    {
                        sender.setData("PLAYER_LOGIN_TRIES_COUNT", tries);
                    }

                    context.SaveChanges();
                }
            }
        }

        private void Player_SetCharacterSelection(Client player)
        {
            bool isCefEnabled = player.getData("PLAYER_IS_CEF_ENABLED");

            var ac = ActivePlayer.Get(player);
            ac.Status = PlayerStatus.CharacterSelection;

            API.freezePlayer(player, true);

            using (var context = new DatabaseContext())
            {
                var id = player.getData("id");
                Entities.Player player_data = ac.Player;

                var char_data = (from c in context.Characters where c.Player.Id == player_data.Id select c).AsNoTracking().ToList();

                // Pls delete after spawn
                player.setData("PLAYER_CHARSEL_DATA", char_data);

                if (isCefEnabled)
                {
                    dynamic data = new System.Dynamic.ExpandoObject();
                    data.can_create = false;
                    data.characters = new List<System.Dynamic.ExpandoObject>();

                    foreach (Character c in char_data)
                    {
                        dynamic dyn = new System.Dynamic.ExpandoObject();

                        dyn.id = c.Id;
                        dyn.name = c.Name;
                        dyn.xp = c.Xp;
                        dyn.maxxp = GetXpNeededToLevelUp(c.Level);
                        dyn.level = c.Level;
                        dyn.location = c.LogoutArea;
                        dyn.health = "100%";
                        dyn.armour = "100%";
                        dyn.cash = c.Cash;
                        dyn.bank = c.Bank;

                        data.characters.Add(dyn);
                    }

                    string _in = API.toJson(data);
                    API.call("Ui", "evalUi", player, "charsel_app.in = " + _in + ";charsel_app.display=true;");
                }
                else
                {
                    player.sendChatMessage(Messages.player_your_characters);

                    var cids = new Dictionary<int, int>();

                    foreach (var item in char_data.Select((value, i) => new { i, value }))
                    {
                        var c = item.value;

                        var text = String.Format(Messages.player_character_n, item.i, c.Name, c.Level, c.Xp, GetXpNeededToLevelUp(c.Level), c.Cash, c.Bank);
                        player.sendChatMessage(text);

                        cids.Add(item.i, c.Id);
                    }

                    player.setData("PLAYER_CHARSEL_CIDS", cids);
                }

                API.triggerClientEvent(player, "SC_DO_CHARSEL");

                if (char_data.Count() > 0)
                {
                    PedHash pedHash;
                    Enum.TryParse(char_data.First().Skin, out pedHash);

                    player.setSkin(pedHash);
                }
            }
        }

        private void Player_LoadData(Client player)
        {
            int id = player.getData("id");

            using (var context = new DatabaseContext())
            {
                var player_data = (from p in context.Players where p.Id == id select p).AsNoTracking().Single();
                // AsNoTracking "detaches" the entity from the Context, allowing it to be kept in memory and used as please up until reattached again @Player_Save
                var ac = ActivePlayer.Get(player);
                ac.Player = player_data;
                // context.Entry(player_data).State = EntityState.Detached;
            }
        }

        private void Player_Spawn(Client player, int character_id)
        {
            player.resetData("PLAYER_CHARSEL_DATA");
            player.resetData("PLAYER_CHARSEL_CIDS");
            int id = player.getData("id");

            using (var context = new DatabaseContext())
            {
                Character cd = (from c in context.Characters where c.Id == character_id && c.Player.Id == id select c).Include(c => c.Faction).Include(c => c.Faction.Ranks).Include(c => c.Rank).Include(c => c.Career).AsNoTracking().Single();

                var ac = ActivePlayer.Get(player);
                ac.Character = cd;
                
                // player.setData("CHARACTER_DATA", cd);
                // player.setData("CHARACTER_ID", cd.Id);                                

                PedHash pedHash;
                Enum.TryParse(cd.Skin, out pedHash);
                player.setSkin(pedHash);

                API.consoleOutput(cd.Rank.ToString());

                player.position = new Vector3(cd.X, cd.Y, cd.Z);
                player.dimension = cd.Dimension;
                player.freeze(false);
                API.call("Ui", "freeCursor", player);

                player.sendChatMessage(String.Format(Messages.player_your_id_is, ac.Id));

                ac.Status = PlayerStatus.Spawned;
                API.triggerClientEvent(player, "SC_DO_SPAWN");
            }
        }

        public void Player_Save(Client player)
        {
            var ac = ActivePlayer.Get(player);

            if (ac.Status == PlayerStatus.AccountOptions ||
                ac.Status == PlayerStatus.CharacterSelection ||
                ac.Status == PlayerStatus.Spawned ||
                ac.Status == PlayerStatus.AdminDuty)
            {
                Entities.Player p = ac.Player;

                using (var context = new DatabaseContext())
                {
                    context.Players.Attach(p);
                    context.Entry(p).State = EntityState.Modified;
                    context.SaveChanges();

                    //context.Entry(p).State = EntityState.Detached;

                    if (ac.Status == PlayerStatus.Spawned)
                    {
                        Entities.Character c = ac.Character;

                        Vector3 pos = player.position;
                        int dimension = player.dimension;

                        c.X = pos.X;
                        c.Y = pos.Y;
                        c.Z = pos.Z;
                        c.Dimension = dimension;                        

                        c.Rank = c.Faction.Ranks.FirstOrDefault(r => r.Id == c.Rank_Id);

                        API.triggerClientEvent(player, "SC_GET_LOGOUT_AREA");                        

                        context.Characters.Attach(c);                        
                        context.Entry(c).State = EntityState.Modified;
                        try
                        {
                            context.SaveChanges();
                        }
                        catch (DbEntityValidationException dbEx)
                        {
                            foreach (var validationErrors in dbEx.EntityValidationErrors)
                            {
                                foreach (var validationError in validationErrors.ValidationErrors)
                                {
                                    API.consoleOutput("Property: {0} Error: {1}",
                                                            validationError.PropertyName,
                                                            validationError.ErrorMessage);
                                }
                            }
                        }                        

                        context.Entry(c).State = EntityState.Detached;
                    }
                }
            }
        }

        public void OnChatMessage(Client player, string message, CancelEventArgs e)
        {
            Character c = ActivePlayer.Get(player).Character;            
            Utils.ProxDetector(30.0f, player, c.Name + " diz: " + message, "~#FFFFFF~", "~#C8C8C8~", "~#AAAAAA~", "~#8C8C8C~", "~#6E6E6E~");
            e.Cancel = true;
            return;
        }

        [Command("login", GreedyArg = true)]
        public void Player_LoginCommand(Client player, string user, string password)
        {
            Player_Login(player, user, password);
        }

        [Command("spawn")]
        public void Player_SpawnCommand(Client player, int character_index)
        {
            var ac = ActivePlayer.Get(player);
            if (ac.Status != PlayerStatus.CharacterSelection || player.getData("PLAYER_IS_CEF_ENABLED") == true)
            {
                return;
            }

            Dictionary<int, int> cids = player.getData("PLAYER_CHARSEL_CIDS");

            // API.consoleOutput(API.toJson(cids));

            int character_id;
            if (cids.TryGetValue(character_index, out character_id))
            {
                Player_Spawn(player, character_id);
            }
            else
            {
                player.sendChatMessage(Messages.player_character_idx_not_exists);
            }
        }     

        private void Player_KickForInvalidTrigger(Client player)
        {
            player.kick(Messages.player_kicked_inconsistency);
        }

        private int GetXpNeededToLevelUp(int level)
        {
            return 8 + (4 * level);
        }


        //Commands

        [Command("comprar")]
        public void BuyCommand(Client player)
        {
            if (ActivePlayer.GetSpawned(player) == null) return;
            Entities.Property.Property prop = PropBLL.Property_GetNearestInRange(player, 4.0);

            if (prop != null)
            {
                PropBLL.Property_BuyCommand(player, prop, false);
            }
            //else if otherbuycases
            else
            {
                API.sendChatMessageToPlayer(player, "Você não está próximo a nada que possa comprar!");
            }
        }

        [Command("trancar")]
        public void LockCommand(Client player)
        {
            if (ActivePlayer.GetSpawned(player) == null) return;

            Entities.Property.Door door = DoorBLL.Door_GetNearestInRangeBothSides(player, 4.0);
            GrandTheftMultiplayer.Server.Elements.Vehicle serverVeh = VehBLL.Vehicle_GetNearestInRange(player, 4.0);            

            if (door != null)
            {
                DoorBLL.Door_LockCommand(player, door);
            }
            else if(serverVeh != null)
            {
                Entities.Vehicle.Vehicle veh = ActiveVehicle.GetSpawned(serverVeh).Vehicle;
                VehBLL.Vehicle_LockCommand(player, veh);
            }
            //else if otherlockcases
            else
            {
                API.sendChatMessageToPlayer(player, "Você não está próximo a nada que possa trancar!");
            }
        }

        [Command("players")]
        public void PlayersCommand(Client player)
        {
            if (ActivePlayer.GetSpawned(player) == null) return;

            foreach (var p in API.getAllPlayers())
            {
                ActivePlayer ac = ActivePlayer.Get(p);
                if(null != ac)
                {
                    API.sendChatMessageToPlayer(player, "(" + ac.Character.Id + ") " + ac.Character.Name);
                }
                else
                {
                    API.sendChatMessageToPlayer(player, "(NOT_LOGGED_IN) " + p.socialClubName);
                }
            }
        }

        //Chat Commands

        [Command("me", GreedyArg = true)]
        public void MeCommand(Client sender, string text)
        {
            ActivePlayer ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            Character c = ac.Character;
            Utils.ProxDetector(30.0f, sender, "* " + c.Name + " " + text, "~#C2A2DA~", "~#C2A2DA~", "~#C2A2DA~", "~#C2A2DA~", "~#C2A2DA~");
        }

        [Command("ame", GreedyArg = true)]
        public void AmeCommand(Client sender, string text)
        {
            ActivePlayer ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            Character c = ac.Character;

            string action;

            action = String.Format("> {0} {1}", c.Name, text);
            API.sendChatMessageToPlayer(sender, "~#C2A2DA~", action);
            action = String.Format("* {0} {1}", c.Name, text);

            if(sender.getData("AME_LABEL") == null)
            {
                TextLabel label = API.createTextLabel(action, new Vector3(), 20.0f, 0.5f, false);
                API.attachEntityToEntity(label, sender, "31086", new Vector3(0.0, 0.0, 1.0), new Vector3());
                API.setTextLabelColor(label, 194, 162, 218, 255);                

                sender.setData("AME_LABEL", label);                
            }
            else
            {
                TextLabel previousLabel = sender.getData("AME_LABEL");
                API.setTextLabelText(previousLabel, action);                                
            }          
        }        

        [Command("do", GreedyArg = true)]
        public void DoCommand(Client sender, string text)
        {
            ActivePlayer ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            Character c = ac.Character;
            Utils.ProxDetector(30.0f, sender, "* " + text + " (( " + c.Name + " ))", "~#C2A2DA~", "~#C2A2DA~", "~#C2A2DA~", "~#C2A2DA~", "~#C2A2DA~");
        }

        [Command("g", Alias = "gritar", GreedyArg = true)]
        public void ScreamCommand(Client sender, string text)
        {
            ActivePlayer ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            Character c = ac.Character;
            Utils.ProxDetector(60.0f, sender, "~h~" + c.Name + " grita: " + text, "~#FFFFFF~", "~#C8C8C8~", "~#AAAAAA~", "~#8C8C8C~", "~#6E6E6E~");
        }

        [Command("baixo", GreedyArg = true)]
        public void LowSayCommand(Client sender, string text)
        {
            ActivePlayer ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            Character c = ac.Character;
            Utils.ProxDetector(10.0f, sender, c.Name + " diz baixo: " + text, "~#FFFFFF~", "~#C8C8C8~", "~#AAAAAA~", "~#8C8C8C~", "~#6E6E6E~");
        }

        [Command("b", GreedyArg = true)]
        public void OocSayCommand(Client sender, string text)
        {
            ActivePlayer ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            Character c = ac.Character;
            Utils.ProxDetector(30.0f, sender, "(( " + c.Name + ": " + text + " ))", "~#939393~", "~#939393~", "~#939393~", "~#939393~", "~#939393~");
        }

        [Command("s", Alias = "sussurrar", GreedyArg = true)]
        public void WhisperCommand(Client sender, int targetid, string text)
        {
            var ac = ActivePlayer.GetSpawned(sender);
            if (null == ac) return;

            var targetAc = ActivePlayer.GetSpawned(targetid);

            if (null == targetAc)
            {
                API.sendChatMessageToPlayer(sender, "Escolha um playerid válido!");
            }
            else
            {
                if(!PlayerBLL.Player_IsInRangeOfPlayer(sender, targetAc.Client)){
                    API.sendChatMessageToPlayer(sender, "Você não está próximo a este jogador!");
                }
                else
                {
                    string msg;

                    msg = String.Format("Sussurro para {0}: {1}", targetAc.Character.Name, text);
                    API.sendChatMessageToPlayer(sender, "~#FFDC18~", msg);
                    msg = String.Format("Sussurro de {0}: {1}", ac.Character.Name, text);
                    API.sendChatMessageToPlayer(targetAc.Client, "~#D8B713~", msg);
                }                
            }
        }

        [Command("pm", GreedyArg = true)]
        public void PmCommand(Client sender, int targetid, string text)
        {
            var ac = ActivePlayer.GetSpawned(sender);
            if (null == ac) return;

            var targetAc = ActivePlayer.GetSpawned(targetid);

            if(null == targetAc)
            {
                API.sendChatMessageToPlayer(sender, "Escolha um playerid válido!");
            }
            else if(targetAc.Id == ac.Id)
            {
                API.sendChatMessageToPlayer(sender, "Você não pode enviar PMs para si mesmo!");
            }
            else
            {
                string msg;

                msg = String.Format("(( PM para {0}({1}): {2} ))", targetAc.Character.Name, targetAc.Id, text);                     
                API.sendChatMessageToPlayer(sender, "~#FFDC18~", msg);
                msg = String.Format("(( PM de {0}({1}): {2} ))", ac.Character.Name, ac.Id, text);
                API.sendChatMessageToPlayer(targetAc.Client, "~#D8B713~", msg);
            }
        }

        [Command("aceitar", GreedyArg = true)]
        public void AcceptCommand(Client sender, string option)
        {
            var ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            Entities.Character c = ac.Character;

            switch (option)
            {
                case "faccao":
                    if (!sender.hasData("FACTION_INVITE")){
                        API.sendChatMessageToPlayer(sender, "Você não possui nenhum convite de facção ativo!");
                    }
                    else
                    {
                        Entities.Faction.Faction fac = sender.getData("FACTION_INVITE");
                        Entities.Faction.Rank rank = fac.Ranks.FirstOrDefault(r => r.Level == 1);

                        sender.resetData("FACTION_INVITE");
                        c.Faction = fac;
                        c.Faction_Id = fac.Id;
                        c.Rank = rank;
                        c.Rank_Id = rank.Id;

                        API.sendChatMessageToPlayer(sender, "Você aceitou o convite para entrar na facção " + fac.Name);
                        FacBLL.Faction_SendMessage(c.Faction, "~w~", "O jogador " + c.Name + " entrou na facção!");
                    }
                    break;
                default:
                    API.sendChatMessageToPlayer(sender, "Escolha uma ação válida!");
                    API.sendChatMessageToPlayer(sender, "~y~[OPÇÕES] ~w~faccao.");
                    break;
            }
        }

        [Command("anim", GreedyArg = true)]
        public void AnimCommand(Client sender, string a1, string a2)
        {
            API.playPlayerAnimation(sender, (int)(AnimationFlags.Loop | AnimationFlags.AllowPlayerControl), a1, a2);
            //API.playPlayerAnimation(player, (int)(AnimationFlags.Loop | AnimationFlags.OnlyAnimateUpperBody | AnimationFlags.AllowPlayerControl), "mp_arresting", "idle");
            //mp_arresting arrested_spin_l_0
            //mp_am_hold_up	handsup_base
            //get_up@cuffed	back_to_default
            API.sendChatMessageToPlayer(sender, "Reproduzindo " + a1 + " " + a2);
        }
    }
}