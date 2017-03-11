using GTANetworkServer;
using GTANetworkShared;
using ProjetoRP.Business.Player;
using ProjetoRP.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;


namespace ProjetoRP.Modules.Faction
{
    public class Faction : Script
    {
        private Business.FactionBLL FacBLL = new Business.FactionBLL();

        public Faction()
        {
            API.onResourceStart += OnResourceStart;
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        public void OnResourceStart()
        {
            API.consoleOutput(Messages.console_startup);
            FacBLL.LoadFactions();
        }

        public void OnClientEventTrigger(Client player, string eventName, object[] args)
        {
            switch (eventName)
            {
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
            }
        }

        private void Faction_KickForInvalidTrigger(Client player)
        {
            player.kick(Messages.player_kicked_inconsistency);
        }

        //General Faction Commands
        [Command("editarrank", GreedyArg = true)]
        public void EditRankCommand(Client sender)
        {
            var ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            Entities.Character c = ac.Character;

            if(c.Faction == null || !FacBLL.Faction_IsLeader(c, c.Faction))
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

        [Command("f", GreedyArg = true)]
        public void FactionChatCommand(Client sender, string msg)
        {
            Entities.Character c = Business.Player.ActivePlayer.GetSpawned(sender).Character;

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
            Entities.Character c = Business.Player.ActivePlayer.GetSpawned(sender).Character;

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
                    Business.Player.ActivePlayer ac = Business.Player.ActivePlayer.Get(player);

                    Entities.Character c2 = ac.Character;

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

        //Police Faction Commands
        [Command("m", GreedyArg = true)]
        public void MegaphoneCommand(Client sender, string msg)
        {
            Entities.Character c = Business.Player.ActivePlayer.GetSpawned(sender).Character;

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
    }
}