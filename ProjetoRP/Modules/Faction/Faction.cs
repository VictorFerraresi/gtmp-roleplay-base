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
                        
        }

        private void Faction_KickForInvalidTrigger(Client player)
        {
            player.kick(Messages.player_kicked_inconsistency);
        }

        //Commands        
        [Command("teste")]
        public void CreateFactionCommand(Client sender, int id)
        {            
            if (Business.GlobalVariables.Instance.ServerFactions.Find(x => x.Id == id) == null)
            {
                API.sendChatMessageToPlayer(sender, "Fac nao existe");
            }           
            else
            {
                Entities.Faction.Faction fac = Business.GlobalVariables.Instance.ServerFactions.Find(x => x.Id == id);
                foreach(var a in fac.Ranks)
                {
                    API.sendChatMessageToPlayer(sender, a.Name);
                }
            }            
        }

    }
}
