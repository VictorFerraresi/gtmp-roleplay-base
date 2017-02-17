﻿using GTANetworkServer;
using ProjetoRP.Entities;
using ProjetoRP.Modules.Player.Types;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProjetoRP.Modules.Player
{
    public class Player : Script
    {
        const int NULL_DIMENSION = int.MaxValue;
        const int MAX_LOGIN_TRIES = 3;
        const int MAX_CHARACTERS_PER_PLAYER = 3;

        public Player()
        {
            API.onResourceStart += OnResourceStart;
            API.onPlayerConnected += OnPlayerConnected;
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        public void OnResourceStart()
        {
            API.consoleOutput(Messages.console_startup);
        }

        private void OnPlayerConnected(Client player)
        {
            player.setData("PLAYER_STATUS", PlayerStatus.PreLoad);
            API.setEntityDimension(player.handle, NULL_DIMENSION);

            API.consoleOutput("Player " + player.socialClubName + " connected.");

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
            switch (eventName)
            {
                case "CS_PLAYER_PRELOAD_READY":
                    if (player.getData("PLAYER_STATUS") == PlayerStatus.PreLoad)
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

                        player.setData("PLAYER_STATUS", PlayerStatus.Login);
                        player.setData("PLAYER_LOGIN_TRIES_COUNT", 0);
                    }
                    else
                    {
                        Player_KickForInvalidTrigger(player);
                    }
                    break;
            }
        }

        private void Player_Login(Client sender, string user, string password)
        {
            if (sender.getData("PLAYER_STATUS") == PlayerStatus.Login)
            {
                int player_id;
                string hashed;
                bool isCefEnabled = sender.getData("PLAYER_IS_CEF_ENABLED");

                using (var context = new DatabaseContext())
                {
                    try
                    {
                        var player = (from p in context.Players where p.Name == user select p).Single();
                        player_id = player.Id;
                        hashed = player.Password;

                        bool test = BCrypt.Net.BCrypt.Verify(password, hashed);

                        if (test)
                        {
                            if (isCefEnabled)
                            {
                                API.exported.ui.evalUi(sender, "login_app.display=false;");
                            }
                            sender.setData("PLAYER_LOGIN_TRIES_COUNT", 0);
                            sender.setData("id", player_id);
                            sender.setData("name", user);

                            var last_successful_session = (from ls in context.Sessions where ls.Player == player && ls.Failed == false select ls).OrderByDescending(x => x.LoginAt).First();
                            var sessions = (from ss in context.Sessions where ss.Player == player && ss.LoginAt > last_successful_session.LoginAt select ss);

                            var text = String.Format(Messages.player_last_logins, last_successful_session.LoginAt, last_successful_session.Ip, sessions.Count());
                            sender.sendChatMessage(text);
                            sender.sendChatMessage(Messages.player_login_success);

                            Entities.Session s = new Entities.Session
                            {
                                Player = player,
                                Failed = false,
                                Ip = sender.address,
                                LoginAt = DateTime.UtcNow,
                                Rgsc = sender.socialClubName
                            };

                            context.Sessions.Add(s);
                            sender.setData("PLAYER_SESSION", s);

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
                    catch (ArgumentNullException)
                    {
                        int tries = sender.getData("PLAYER_LOGIN_TRIES_COUNT") + 1;

                        string msg = string.Format(Messages.player_wrong_password, tries, MAX_LOGIN_TRIES);
                        if (isCefEnabled)
                        {
                            API.exported.ui.evalUi(sender, "login_app.blocked=false;login_app.error='" + msg + "';");
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
                    }
                    context.SaveChanges();
                }
            }
        }

        private void Player_SetCharacterSelection(Client player)
        {
            bool isCefEnabled = player.getData("PLAYER_IS_CEF_ENABLED");

            player.setData("PLAYER_STATUS", PlayerStatus.CharacterSelection);
            API.freezePlayer(player, true);

            var id = player.getData("id");
            Entities.Player player_data = player.getData("PlayerData");
            var char_data = player_data.Characters;

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
                    dyn.location = "San Andreas";
                    dyn.health = "100%";
                    dyn.armour = "100%";
                    dyn.cash = c.Cash;
                    dyn.bank = c.Bank;

                    data.characters.Add(dyn);
                }

                string _in = API.toJson(data);
                API.exported.ui.evalUi(player, "charsel_app.in = " + _in + ";charsel_app.display=true;");
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

        private void Player_LoadData(Client player)
        {
            int id = player.getData("id");

            using (var context = new DatabaseContext())
            {
                var player_data = (from p in context.Players where p.Id == id select p).AsNoTracking().First(); 
                // AsNoTracking "detaches" the entity from the Context, allowing it to be kept in memory and used as please up until reattached again @Player_Save
                player.setData("PlayerData", player_data);
                // context.Entry(player_data).State = EntityState.Detached;
            }
        }

        public void Player_Save(Client player)
        {
            if (player.getData("PLAYER_STATUS") == PlayerStatus.AccountOptions || 
                player.getData("PLAYER_STATUS") == PlayerStatus.CharacterSelection ||
                player.getData("PLAYER_STATUS") == PlayerStatus.Spawned) 
            {
                Entities.Player p = player.getData("PlayerData");

                using (var context = new DatabaseContext())
                {
                    context.Players.Attach(p);
                    context.Entry(p).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
                
        }

        [Command(nameof(Messages.command_login))]
        public void Player_LoginCommand(Client player, string user, string password)
        {
            Player_Login(player, user, password);
        }

        private void Player_KickForInvalidTrigger(Client player)
        {
            player.kick(Messages.player_kicked_inconsistency);
        }

        private int GetXpNeededToLevelUp(int level)
        {
            return 8 + (4 * level);
        }
    }
}