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
            }
        }

        private void Faction_KickForInvalidTrigger(Client player)
        {
            player.kick(Messages.player_kicked_inconsistency);
        }

        //Commands
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
    }
}
