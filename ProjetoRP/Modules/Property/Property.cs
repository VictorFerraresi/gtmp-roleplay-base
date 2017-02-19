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


namespace ProjetoRP.Modules.Property
{
    public class Property : Script
    {
        public Business.PropertyBLL PropBll = new Business.PropertyBLL();

        public Property()
        {
            API.onResourceStart += OnResourceStart;
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        public void OnResourceStart()
        {
            API.consoleOutput(Messages.console_startup);            
            PropBll.DrawPropertiesPickups();
        }

        public void OnClientEventTrigger(Client player, string eventName, object[] args)
        {
            
        }        

        //Commands

        [Command("position")]
        public void getPosition(Client player)
        {
            API.consoleOutput("{0},{1},{2}", player.position.X, player.position.Y, player.position.Z);            
        }
    }
}
