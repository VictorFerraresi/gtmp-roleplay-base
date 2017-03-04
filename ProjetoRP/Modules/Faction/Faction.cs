using GTANetworkServer;
using GTANetworkShared;
using ProjetoRP.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
                    API.sendChatMessageToPlayer(player, (string)args[0]);
                    var dataer = API.fromJson((string)args[0]);

                    foreach(var rank in dataer.ranks)
                    {
                        string name = rank.name;
                        API.sendChatMessageToPlayer(player, name);
                    }                                        

                    break;
            }
        }

        private void Faction_KickForInvalidTrigger(Client player)
        {
            player.kick(Messages.player_kicked_inconsistency);
        }

        //Commands
        [Command("editarrank", GreedyArg = true)]
        public void CreateFactionCommand(Client sender)
        {
            Entities.Character c = sender.getData("CHARACTER_DATA");

            if(c.Faction == null || !FacBLL.Faction_IsLeader(c, c.Faction))
            {
                API.sendChatMessageToPlayer(sender, "Você não tem permissão para utilizar este comando!");
            }
            else
            {              
                dynamic ranks = new List<System.Dynamic.ExpandoObject>();

                foreach(Entities.Faction.Rank r in c.Faction.Ranks)
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
    }
}
