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
using ProjetoRP.Business.Vehicle;

namespace ProjetoRP.Modules.Vehicle
{
    public class Vehicle : Script
    {
        private VehicleBLL VehBLL = new VehicleBLL();

        public Vehicle()
        {
            API.onResourceStart += OnResourceStart;            
            API.onClientEventTrigger += OnClientEventTrigger;
            API.onPlayerEnterVehicle += OnPlayerEnterVehicle;
        }

        public void OnResourceStart()
        {
            API.consoleOutput(Messages.console_startup);
            VehBLL.LoadVehicles();
        }        

        public void OnClientEventTrigger(Client player, string eventName, object[] args)
        {
            switch (eventName)
            {
                case "CS_VEHICLE_SPAWN":
                    var data = API.fromJson((string)args[0]);

                    Character c = Business.Player.ActivePlayer.Get(player).Character;

                    Entities.Vehicle.Vehicle veh = Business.Vehicle.ActiveVehicle.GetBySQLID((int)data.Id).Vehicle;                                       
                    
                    if (VehBLL.Vehicle_IsOwner(c, veh))
                    {
                        if (!VehBLL.Vehicle_IsSpawned(veh))
                        {
                            veh = VehBLL.SQL_FetchVehicleData(veh.Id);
                            VehBLL.Vehicle_Spawn(veh);
                            API.triggerClientEvent(player, "SC_CLOSE_VEHICLEMENU");
                        }
                        else
                        {
                            API.sendChatMessageToPlayer(player, Messages.vehicle_already_spawned);
                        }
                    }
                    else
                    {
                        Vehicle_KickForInvalidTrigger(player);
                    }
                    break;
            }
        }

        private void OnPlayerEnterVehicle(Client player, NetHandle vehicle)
        {            
            Entities.Vehicle.Vehicle veh = ActiveVehicle.GetSpawned(vehicle).Vehicle;
            Entities.Character c = Business.Player.ActivePlayer.Get(player).Character;            
                        
            if (veh.Owner_Type == Entities.Vehicle.OwnerType.OWNER_TYPE_FACTION) //Entered a faction vehicle
            {                
                if (API.getPlayerVehicleSeat(player) == -1) //Driver
                {                    
                    if (c.Faction_Id != veh.Owner_Id)
                    {                        
                        API.sendNotificationToPlayer(player, "Este veículo é restrito à uma facção!");
                        API.warpPlayerOutOfVehicle(player, vehicle);
                    }
                }                               
            }
        }

        private void Vehicle_KickForInvalidTrigger(Client player)
        {
            player.kick(Player.Messages.player_kicked_inconsistency);
        }


        //Commands
        [Command("v")]
        public void VCommand(Client player, string option)
        {
            switch (option)
            {
                case "lista":
                    Character c = Business.Player.ActivePlayer.Get(player).Character;
                    List<Entities.Vehicle.Vehicle> vehs = VehBLL.SQL_FetchVehiclesFromCharacter(c);
                    if (!vehs.Any())
                    {
                        API.sendChatMessageToPlayer(player, "Você não possui nenhum veículo!");
                    }
                    else
                    {
                        API.triggerClientEvent(player, "SC_SHOW_VEHICLEMENU", API.toJson(vehs));
                    }                    
                    break;

                case "estacionar":
                    break;
            }            
        }

        [Command("motor")]
        public void EngineCommand(Client player)
        {
            if (!API.isPlayerInAnyVehicle(player) || API.getPlayerVehicleSeat(player) != -1) //-1 is Driver Seat
            {
                API.sendChatMessageToPlayer(player, Messages.vehicle_not_driving);
                return;
            }
            NetHandle serverVeh = API.getPlayerVehicle(player);            
            Entities.Vehicle.Vehicle veh = ActiveVehicle.GetSpawned(serverVeh).Vehicle;
            Character c = Business.Player.ActivePlayer.Get(player).Character;

            if (VehBLL.Vehicle_HasKey(veh, c) || (veh.Owner_Type == Entities.Vehicle.OwnerType.OWNER_TYPE_FACTION && (veh.Owner_Id == c.Faction_Id)))
            {
                if (veh.Engine == true)
                {
                    API.setVehicleEngineStatus(serverVeh, false);
                    veh.Engine = false;
                    API.sendNotificationToPlayer(player, "Motor desligado");
                }
                else
                {
                    API.setVehicleEngineStatus(serverVeh, true);
                    veh.Engine = true;
                    API.sendNotificationToPlayer(player, "Motor ligado");
                }                
            }
            else
            {
                API.sendChatMessageToPlayer(player, Messages.vehicle_no_keys);
            }        
        }

        [Command("veh")]
        public void VehCommand(Client player, string name, int color1, int color2)
        {
            VehicleHash vhash = API.vehicleNameToModel(name);

            API.createVehicle(vhash, player.position, new Vector3(0, 0, 0), color1, color2, 0);
        }

        [Command("skin")]
        public void SkinCommand(Client player, long hash)
        {
            API.setPlayerSkin(player, (PedHash)hash);
        }    

        [Command("portamalas")]
        public void TrunkCommand(Client player, string action)
        {
            GTANetworkServer.Vehicle serverVeh = VehBLL.Vehicle_GetNearestInRange(player, 2.0);

            if (serverVeh == null)
            {
                API.sendChatMessageToPlayer(player, Messages.vehicle_not_near_any);
                return;
            }

            Entities.Vehicle.Vehicle veh = ActiveVehicle.GetSpawned(serverVeh).Vehicle;

            switch (action)
            {
                case "abrir":
                    if (VehBLL.Vehicle_IsLocked(veh))
                    {
                        API.sendChatMessageToPlayer(player, Messages.vehicle_locked);
                        return;
                    }
                    if (serverVeh.isDoorOpen(5))
                    {
                        API.sendChatMessageToPlayer(player, Messages.vehicle_trunk_already_opened);
                        return;
                    }
                    else
                    {
                        serverVeh.openDoor(5);
                        API.sendNotificationToPlayer(player, "Portamalas aberto");
                    }
                    break;
                case "fechar":
                    if (!serverVeh.isDoorOpen(5))
                    {
                        API.sendChatMessageToPlayer(player, Messages.vehicle_trunk_already_closed);
                        return;
                    }
                    else
                    {
                        serverVeh.closeDoor(5);
                        API.sendNotificationToPlayer(player, "Portamalas fechado");
                    }
                    break;
                case "ver":
                    //TO DO WITH THE ITEM.SYS
                    break;
                default:
                    API.sendChatMessageToPlayer(player, Messages.vehicle_invalid_action);
                    API.sendChatMessageToPlayer(player, "~y~[AÇÕES] ~w~abrir, fechar, ver.");
                    break;
            }
        }
    }
}