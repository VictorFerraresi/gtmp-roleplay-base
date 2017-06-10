using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared.Math;
using ProjetoRP.Business.Player;
using ProjetoRP.Entities;
using ProjetoRP.Business.Property;

namespace ProjetoRP.Modules.Property
{
    public class Property : Script
    {
        private PropertyBLL PropBLL = new PropertyBLL();
        private DoorBLL DoorBLL = new DoorBLL();

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
            switch (eventName)
            {
                case "CS_BUY_PROP_CONFIRMATION":
                    if (ActivePlayer.GetSpawned(player) == null) return;
                    int resp = (int)args[0];
                    int propid = (int)args[1];

                    Entities.Property.Property prop = PropBLL.FindPropertyById(propid);                    

                    if (prop == null)
                    {
                        Property_KickForInvalidTrigger(player);
                    }
                    else
                    {                        
                        switch (resp)
                        {
                            case 0: //Yes
                                PropBLL.Property_BuyCommand(player, prop, true);
                                break;

                            case 1: //No
                                API.sendChatMessageToPlayer(player, "Você desistiu da compra da propriedade!");
                                break;

                            default:
                                Property_KickForInvalidTrigger(player);
                                break;
                        }
                    }                    

                    API.triggerClientEvent(player, "SC_CLOSE_BUY_PROP_CONFIRM_MENU");
                    break;
            }
        }

        private void Property_KickForInvalidTrigger(Client player)
        {
            player.kick("Você foi kickado por inconsistencia de informações transmitidas.");
        }

        //Commands

        [Command("entrar")]
        public void EnterCommand(Client player)
        {
            var ac = ActivePlayer.GetSpawned(player);
            if (ac == null) return;

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
                    Character c = ac.Character;
                    //c.InsideHouse = door.Property; 
                }
            }
        }

        [Command("sair")]
        public void ExitCommand(Client player)
        {
            var ac = ActivePlayer.GetSpawned(player);
            if (ac == null) return;

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
                    Character c = ac.Character;
                    //c.InsideHouse = null;
                }
            }
        }

        [Command("position")]
        public void getPosition(Client player)
        {
            API.consoleOutput("{0},{1},{2}", player.position.X, player.position.Y, player.position.Z);           
        }

        [Command("carpos")]
        public void getCarPos(Client player)
        {            
            Vector3 vehPos = API.shared.getEntityPosition(player.vehicle);
            API.consoleOutput("{0},{1},{2}", vehPos.X, vehPos.Y, vehPos.Z);
            Vector3 vehRot = API.shared.getEntityRotation(player.vehicle);
            API.consoleOutput("{0},{1},{2}", vehRot.X, vehRot.Y, vehRot.Z);
            API.consoleOutput("----------------------------", vehRot.X, vehRot.Y, vehRot.Z);
        }
    }
}
