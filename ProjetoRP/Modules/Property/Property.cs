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
        private Business.PropertyBLL PropBLL = new Business.PropertyBLL();
        private Business.DoorBLL DoorBLL = new Business.DoorBLL();
        public Property()
        {
            API.onResourceStart += OnResourceStart;
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        public void OnResourceStart()
        {
            API.consoleOutput(Messages.console_startup);
            PropBLL.LoadProperties();
            DoorBLL.LoadDoors();
            PropBLL.DrawPropertiesPickups();
        }

        public void OnClientEventTrigger(Client player, string eventName, object[] args)
        {
            
        }

        //Commands

        [Command("entrar")]
        public void EnterCommand(Client player)
        {
            Entities.Property.Door door = DoorBLL.Door_GetNearestInRange(player, 4.0, true);

            if(door == null)
            {
                API.sendChatMessageToPlayer(player, "Você não está próximo a nenhuma porta!");
            }
            else
            {
                if (DoorBLL.Door_IsLocked(door))
                {
                    API.sendChatMessageToPlayer(player, "Esta porta está trancada!");
                }
                else
                {
                    API.setEntityPosition(player.handle, new Vector3(door.InteriorX, door.InteriorY, door.InteriorZ));
                    API.setEntityDimension(player.handle, door.InteriorDimension);
                    Character c = player.getData("CHARACTER_DATA");
                    //c.InsideHouse = door.Property; 
                }
            }
        }

        [Command("sair")]
        public void ExitCommand(Client player)
        {
            Entities.Property.Door door = DoorBLL.Door_GetNearestInRange(player, 4.0, false);

            if (door == null)
            {
                API.sendChatMessageToPlayer(player, "Você não está próximo a nenhuma porta!");                                
            }
            else
            {
                if (DoorBLL.Door_IsLocked(door))
                {
                    API.sendChatMessageToPlayer(player, "Esta porta está trancada!");
                }
                else
                {
                    API.setEntityPosition(player.handle, new Vector3(door.ExteriorX, door.ExteriorY, door.ExteriorZ));
                    API.setEntityDimension(player.handle, door.ExteriorDimension);
                    Character c = player.getData("CHARACTER_DATA");
                    //c.InsideHouse = null;
                }
            }
        }

        [Command("position")]
        public void getPosition(Client player)
        {
            API.consoleOutput("{0},{1},{2}", player.position.X, player.position.Y, player.position.Z);            
        }
    }
}
