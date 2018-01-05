using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using ProjetoRP.Business.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ProjetoRP.Business.Faction;


namespace ProjetoRP.Modules.Faction
{
    public class Faction : Script
    {
        private FactionBLL FacBLL = new FactionBLL();

        public Faction()
        {
            API.onResourceStart += OnResourceStart;
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        public void OnResourceStart()
        {
            API.consoleOutput(Messages.console_startup);
            FacBLL.LoadFactions();
            FacBLL.DrawLockersPickups();
        }

        public void OnClientEventTrigger(Client player, string eventName, object[] args)
        {
            switch (eventName)
            {
                case "CS_SIREN_TOGGLE":
                    NetHandle vehHandle = API.getPlayerVehicle(player);                                                           
                    if (!API.isPlayerInAnyVehicle(player) || API.getPlayerVehicleSeat(player) != -1 || !API.getVehicleSirenState(vehHandle))
                    {
                        return;
                    }

                    bool siren = API.fetchNativeFromPlayer<bool>(player, Hash._IS_VEHICLE_SIREN_SOUND_ON, vehHandle);

                    GrandTheftMultiplayer.Server.Elements.Vehicle v = Business.Vehicle.ActiveVehicle.GetSpawned(vehHandle).VehicleHandle;

                    v.setSyncedData("SIREN_SOUND_STATUS", !siren);

                    API.sendNativeToAllPlayers(Hash.DISABLE_VEHICLE_IMPACT_EXPLOSION_ACTIVATION, vehHandle, siren);                    

                    break;
                case "CS_EDIT_RANKS_SUBMIT":
                    List<Entities.Faction.Rank> ranks = JArray.Parse((string)args[0]).ToObject<List<Entities.Faction.Rank>>();
                                        
                    var ac = ActivePlayer.GetSpawned(player);
                    if (ac == null) return;

                    Entities.Character c = ac.Character;

                    foreach(var rank in ranks)
                    {
                        Entities.Faction.Rank oldRank = c.Faction.Ranks.FirstOrDefault(r => r.Id == rank.Id);

                        if (oldRank == null)
                        {
                            rank.Faction = c.Faction;
                            rank.Faction_Id = (int)c.Faction_Id;
                            c.Faction.Ranks.Add(rank);
                            FacBLL.Rank_Create(rank);
                        }
                        else
                        {
                            oldRank.Level = rank.Level;
                            oldRank.Name = rank.Name;
                            FacBLL.Rank_Save(oldRank);
                        }
                    }
                                        
                    foreach (var scriptRank in c.Faction.Ranks.Reverse())
                    {
                        Entities.Faction.Rank vueRank = ranks.FirstOrDefault(r => r.Id == scriptRank.Id);

                        if (vueRank == null)
                        {
                            c.Faction.Ranks.Remove(scriptRank);
                            FacBLL.Rank_Delete(scriptRank);
                            //Need to deal with players that had a deleted rank
                        }
                    }

                    API.call("Ui", "evalUi", player, "rankedit_app.display=false;rankedit_app.blocked=false");
                    API.call("Ui", "fixCursor", player, false);
                    API.sendChatMessageToPlayer(player, "Você editou os ranks com sucesso!");
                    break;

                case "CS_EDIT_RANKS_CANCEL":
                    API.call("Ui", "evalUi", player, "rankedit_app.display=false;rankedit_app.blocked=false");
                    API.call("Ui", "fixCursor", player, false);
                    API.sendChatMessageToPlayer(player, "Você cancelou a edição dos ranks e nenhuma alteração foi salva!");
                    break;

                case "CS_SHOW_FACTION_MEMBERS_CLOSE":
                    API.call("Ui", "evalUi", player, "factionmembers_app.display=false;factionmembers_app.blocked=false");
                    API.call("Ui", "fixCursor", player, false);
                    break;

                case "CS_SHOW_FACTIONS_CLOSE":
                    API.call("Ui", "evalUi", player, "showfactions_app.display=false;showfactions_app.blocked=false");
                    API.call("Ui", "fixCursor", player, false);
                    break;

                case "CS_SET_LOCKER_SKIN":
                    API.triggerClientEvent(player, "SC_CLOSE_LOCKER_UNIFORM_MENU");

                    PedHash pedHash;
                    Enum.TryParse(args[0].ToString(), out pedHash);                
                    API.setPlayerSkin(player, pedHash);
                    break;

                case "CS_SET_LOCKER_ARMOR":
                    API.setPlayerArmor(player, 100);
                    break;

                case "CS_SET_LOCKER_EQUIPMENT":
                    WeaponHash weaponHash;
                    Enum.TryParse(args[0].ToString(), out weaponHash);

                    API.givePlayerWeapon(player, weaponHash, (int)args[1], false, true);
                    break;
            }
        }

        private void Faction_KickForInvalidTrigger(Client player)
        {
            player.kick(Messages.player_kicked_inconsistency);
        }

        //Faction Leader Commands
        [Command("editarrank", GreedyArg = true)]
        public void EditRankCommand(Client sender)
        {
            var ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            Entities.Character c = ac.Character;

            if (c.Faction == null || !FacBLL.Faction_IsLeader(c, c.Faction))
            {
                API.sendChatMessageToPlayer(sender, "Você não tem permissão para utilizar este comando!");
            }
            else
            {
                dynamic ranks = new List<System.Dynamic.ExpandoObject>();                

                List<Entities.Faction.Rank> orderedByLevelDesc = c.Faction.Ranks.OrderByDescending(r => r.Level).ToList();

                foreach (Entities.Faction.Rank r in orderedByLevelDesc)
                {
                    dynamic dyn = new System.Dynamic.ExpandoObject();

                    dyn.id = r.Id;
                    dyn.name = r.Name;
                    dyn.level = r.Level;
                    dyn.leader = r.Leader;

                    ranks.Add(dyn);
                }

                string _in = API.toJson(ranks);
                API.call("Ui", "fixCursor", sender, true);
                API.call("Ui", "evalUi", sender, "rankedit_app.ranks = " + _in + ";rankedit_app.display=true;");
            }
        }

        [Command("demitir")]
        public void FireCommand(Client sender, int targetid)
        {
            var ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            Entities.Character c = ac.Character;

            if (c.Faction == null || !FacBLL.Faction_IsLeader(c, c.Faction))
            {
                API.sendChatMessageToPlayer(sender, "Você não tem permissão para utilizar este comando!");
            }
            else
            {
                var targetAc = ActivePlayer.GetSpawned(targetid);

                if (null == targetAc)
                {
                    API.sendChatMessageToPlayer(sender, "Escolha um playerid válido!");
                }
                else
                {
                    if(targetAc.Character.Faction_Id != c.Faction_Id)
                    {
                        API.sendChatMessageToPlayer(sender, "Este jogador não é da sua facção!");
                    }
                    else
                    {
                        if(targetAc.Character.Rank.Level >= c.Rank.Level)
                        {
                            API.sendChatMessageToPlayer(sender, "Este jogador possui um rank maior que o seu!");
                        }
                        else
                        {
                            Client targetC = targetAc.Client;

                            targetAc.Character.Rank = null;
                            targetAc.Character.Rank_Id = null;
                            targetAc.Character.Faction = null;
                            targetAc.Character.Faction_Id = null;
                            API.sendChatMessageToPlayer(sender, "Você demitiu o jogador " + targetAc.Character.Name + " da sua facção!");
                            API.sendChatMessageToPlayer(targetC, "Você foi demitido da facção pelo líder " + c.Name);
                        }
                    }
                }
            }
        }

        [Command("rank")]
        public void SetRankCommand(Client sender, int targetid, int rankid)
        {
            var ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            Entities.Character c = ac.Character;

            if (c.Faction == null || !FacBLL.Faction_IsLeader(c, c.Faction))
            {
                API.sendChatMessageToPlayer(sender, "Você não tem permissão para utilizar este comando!");
            }
            else
            {
                var targetAc = ActivePlayer.GetSpawned(targetid);

                if (null == targetAc)
                {
                    API.sendChatMessageToPlayer(sender, "Escolha um playerid válido!");
                }
                else
                {
                    if (targetAc.Character.Faction_Id != c.Faction_Id)
                    {
                        API.sendChatMessageToPlayer(sender, "Este jogador não é da sua facção!");
                    }
                    else
                    {
                        if (targetAc.Character.Rank.Level >= c.Rank.Level)
                        {
                            API.sendChatMessageToPlayer(sender, "Este jogador possui um rank maior que o seu!");
                        }
                        else
                        {
                            Entities.Faction.Rank rank = FacBLL.Faction_GetRankByLevel(c.Faction, rankid);

                            if(null == rank)
                            {
                                API.sendChatMessageToPlayer(sender, "Este rank não existe!");
                            }
                            else if(rank.Level > c.Rank.Level)
                            {
                                API.sendChatMessageToPlayer(sender, "Você não pode promover a um rank maior do que o seu!");
                            }
                            else
                            {
                                Client targetC = targetAc.Client;

                                targetAc.Character.Rank = rank;
                                targetAc.Character.Rank_Id = rank.Id;
                                                                
                                API.sendChatMessageToPlayer(sender, "Você setou o rank do jogador " + targetAc.Character.Name + " para " + rank.Name);
                                API.sendChatMessageToPlayer(targetC, "O líder " + c.Name + " setou o seu rank para " + rank.Name);
                            }                            
                        }
                    }
                }
            }
        }

        [Command("convidar")]
        public void InviteCommand(Client sender, int targetid)
        {
            var ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            Entities.Character c = ac.Character;

            if (c.Faction == null || !FacBLL.Faction_IsLeader(c, c.Faction))
            {
                API.sendChatMessageToPlayer(sender, "Você não tem permissão para utilizar este comando!");
            }
            else
            {
                var targetAc = ActivePlayer.GetSpawned(targetid);

                if (null == targetAc)
                {
                    API.sendChatMessageToPlayer(sender, "Escolha um playerid válido!");
                }
                else
                {
                    if (targetAc.Character.Faction_Id != null)
                    {
                        API.sendChatMessageToPlayer(sender, "Este jogador já está em uma facção!");
                    }
                    else
                    {
                        Entities.Faction.Rank rank = FacBLL.Faction_GetRankByLevel(c.Faction, 1);

                        if (null == rank)
                        {
                            API.sendChatMessageToPlayer(sender, "Configure um rank para a facção antes de convidar membros!");
                        }
                        else
                        {
                            Client targetC = targetAc.Client;

                            targetC.setData("FACTION_INVITE", c.Faction);

                            API.sendChatMessageToPlayer(sender, "Você convidou o jogador " + targetAc.Character.Name + " para a facção!");
                            API.sendChatMessageToPlayer(targetC, "O líder " + c.Name + " te convidou para a facção " + c.Faction.Name);
                            API.sendChatMessageToPlayer(targetC, "Digite \"/aceitar faccao\" para aceitar o convite");
                        }
                    }
                }
            }
        }

        //General Faction Commands
        [Command("f", GreedyArg = true)]
        public void FactionChatCommand(Client sender, string msg)
        {
            var ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;            

            Entities.Character c = ac.Character;

            if (c.Faction == null)
            {
                API.sendChatMessageToPlayer(sender, "Você não tem permissão para utilizar este comando!");
            }
            else
            {
                FacBLL.Faction_SendChatMessage(c, msg);
            }
        }

        [Command("membros")]
        public void SeeMembersCommand(Client sender)
        {
            var ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            Entities.Character c = ac.Character;

            if (c.Faction == null)
            {
                API.sendChatMessageToPlayer(sender, "Você não tem permissão para utilizar este comando!");
            }
            else
            {
                dynamic data = new System.Dynamic.ExpandoObject();

                dynamic characters = new List<System.Dynamic.ExpandoObject>();

                List<Entities.Character> orderedByRankDesc = new List<Entities.Character>();

                foreach (var player in API.shared.getAllPlayers())
                {
                    Business.Player.ActivePlayer ac2 = Business.Player.ActivePlayer.Get(player);

                    Entities.Character c2 = ac2.Character;

                    if (c2.Faction_Id == c.Faction.Id)
                    {
                        orderedByRankDesc.Add(c2);
                    }
                }

                orderedByRankDesc.Sort((x, y) => y.Rank.Level.CompareTo(x.Rank.Level));

                foreach (Entities.Character c2 in orderedByRankDesc)
                {
                    dynamic dyn = new System.Dynamic.ExpandoObject();

                    dyn.id = c2.Id;
                    dyn.activeId = Business.Player.ActivePlayer.Get(c2).Id;
                    dyn.name = c2.Name;
                    dyn.rank = c2.Rank.Name;

                    characters.Add(dyn);
                }

                data.characters = characters;
                data.faction = c.Faction.Name;

                string _in = API.shared.toJson(data);
                API.call("Ui", "fixCursor", sender, true);
                API.call("Ui", "evalUi", sender, "factionmembers_app.in = " + _in + ";factionmembers_app.display=true;");
            }
        }

        [Command("faccoes")]
        public void SeeFactionsCommand(Client sender)
        {
            if (ActivePlayer.GetSpawned(sender) == null) return;

            dynamic data = new System.Dynamic.ExpandoObject();

            dynamic factions = new List<System.Dynamic.ExpandoObject>();            

            foreach (Entities.Faction.Faction fac in Business.GlobalVariables.Instance.ServerFactions)
            {
                dynamic dyn = new System.Dynamic.ExpandoObject();
                                
                dyn.name = fac.Name;
                dyn.onlineMembers = FacBLL.Faction_GetOnlineMemberCount(fac);
                dyn.totalMembers = FacBLL.Faction_GetMemberCount(fac);

                factions.Add(dyn);
            }

            data.factions = factions;            

            string _in = API.shared.toJson(data);
            API.call("Ui", "fixCursor", sender, true);
            API.call("Ui", "evalUi", sender, "showfactions_app.in = " + _in + ";showfactions_app.display=true;");
        }

        [Command("armario")]
        public void LockerCommand(Client sender)
        {
            var ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            Entities.Character c = ac.Character;

            Entities.Faction.Locker locker = FacBLL.Faction_GetNearestLockerInRange(sender, 5.0);

            if (locker == null)
            {
                API.sendChatMessageToPlayer(sender, "Você não está próximo a nenhum armário!");
            }            
            else if(locker.Faction_Id != c.Faction_Id)
            {
                API.sendChatMessageToPlayer(sender, "Este armário não pertence à sua facção!");
            }
            else
            {
                if(c.Faction.Type == Entities.Faction.FactionType.FACTION_TYPE_POLICE)
                {
                    API.shared.triggerClientEvent(sender, "SC_SHOW_LOCKER_MENU_POLICE", locker);
                }
                else if(c.Faction.Type == Entities.Faction.FactionType.FACTION_TYPE_EMS)
                {
                    API.shared.triggerClientEvent(sender, "SC_SHOW_LOCKER_MENU_EMS", locker);
                }
            }
        }

        //Police Faction Commands
        [Command("m", GreedyArg = true)]
        public void MegaphoneCommand(Client sender, string msg)
        {
            var ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            Entities.Character c = ac.Character;

            if (c.Faction == null || (c.Faction.Type != Entities.Faction.FactionType.FACTION_TYPE_POLICE && c.Faction.Type != Entities.Faction.FactionType.FACTION_TYPE_EMS))
            {
                API.sendChatMessageToPlayer(sender, "Você não tem permissão para utilizar este comando!");
            }
            else
            {
                string finalmsg = String.Format("[{0} {1}:o< {2}]", c.Rank.Name, c.Name, msg);

                Business.Utils.ProxDetector(30.0f, sender, finalmsg, "~#FFFF00~", "~#FFFF00~", "~#FFFF00~", "~#FFFF00~", "~#FFFF00~");
            }
        }

        [Command("dep", GreedyArg = true)]
        public void DepartmentRadioCommand(Client sender, string msg)
        {
            var ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            Entities.Character c = ac.Character;

            if (c.Faction == null || (c.Faction.Type != Entities.Faction.FactionType.FACTION_TYPE_POLICE && c.Faction.Type != Entities.Faction.FactionType.FACTION_TYPE_EMS))
            {
                API.sendChatMessageToPlayer(sender, "Você não tem permissão para utilizar este comando!");
            }
            else
            {
                string finalmsg = String.Format("*[{0}] {1} {2}: {3}", c.Faction.Acro, c.Rank.Name, c.Name, msg);

                FacBLL.Faction_SendDepartmentMessage(finalmsg);

                finalmsg = String.Format("(Rádio) {0} diz: {1}", c.Name, msg);
                Business.Utils.ExclusiveProxDetector(30.0f, sender, finalmsg, "~#FFFFFF~", "~#C8C8C8~", "~#AAAAAA~", "~#8C8C8C~", "~#6E6E6E~");
            }
        }

        [Command("duty")]
        public void DutyCommand(Client sender)
        {
            var ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            Entities.Character c = ac.Character;

            if (c.Faction == null || (c.Faction.Type != Entities.Faction.FactionType.FACTION_TYPE_POLICE && c.Faction.Type != Entities.Faction.FactionType.FACTION_TYPE_EMS))
            {
                API.sendChatMessageToPlayer(sender, "Você não tem permissão para utilizar este comando!");
            }
            else
            {
                if (sender.hasData("PLAYER_DUTY"))
                {
                    sender.resetData("PLAYER_DUTY");
                    string dutyMsg = String.Format("{0} {1} saiu do trabalho.", c.Rank.Name, c.Name);
                    FacBLL.Faction_SendMessage(c.Faction, "~#FF5050~", dutyMsg);
                    sender.removeAllWeapons();
                    sender.resetNametagColor();                    
                }
                else
                {
                    string dutyMsg = String.Format("{0} {1} entrou em trabalho.", c.Rank.Name, c.Name);
                    FacBLL.Faction_SendMessage(c.Faction, "~#FF5050~", dutyMsg);
                    sender.setData("PLAYER_DUTY", true);
                    sender.nametagColor = new Color(0, 0, 255);
                }
            }
        }
    }
}