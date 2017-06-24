using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using ProjetoRP.Entities;
using System.Collections.Generic;
using System.Linq;
using ProjetoRP.Business.Vehicle;
using ProjetoRP.Business.Player;
using ProjetoRP.Business.Career;
using ProjetoRP.Types;

namespace ProjetoRP.Modules.Vehicle
{
    public class Vehicle : Script
    {
        private VehicleBLL VehBLL = new VehicleBLL();
        private TruckerCareerBLL TruckerBLL = new TruckerCareerBLL();
        private TaxiCareerBLL TaxiBLL = new TaxiCareerBLL();

        public Vehicle()
        {
            API.onResourceStart += OnResourceStart;
            API.onClientEventTrigger += OnClientEventTrigger;
            API.onPlayerEnterVehicle += OnPlayerEnterVehicle;
            API.onPlayerExitVehicle += OnPlayerExitVehicle;
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

                case "CS_VEHICLE_TURN_ENGINE":
                    EngineCommand(player);
                    break;
            }
        }

        private void OnPlayerEnterVehicle(Client player, NetHandle vehicle)
        {
            Entities.Vehicle.Vehicle veh = ActiveVehicle.GetSpawned(vehicle).Vehicle;
            Entities.Character c = ActivePlayer.Get(player).Character;

            if (player.hasData("CRATE_HOLDING"))
            {
                API.warpPlayerOutOfVehicle(player);
                API.sendChatMessageToPlayer(player, "Você não pode entrar em um veículo carregando uma carga. Utilize /guardarcarga ou /destruircarga.");
            }

            else if (veh.Owner_Type == Entities.Vehicle.OwnerType.OWNER_TYPE_FACTION) //Entered a faction vehicle
            {
                if (API.getPlayerVehicleSeat(player) == -1) //Driver
                {
                    if (c.Faction_Id != veh.Owner_Id)
                    {
                        API.sendNotificationToPlayer(player, "Este veículo é restrito à uma facção!");
                        API.warpPlayerOutOfVehicle(player);
                    }
                }
            }
            else if (veh.Owner_Type == Entities.Vehicle.OwnerType.OWNER_TYPE_CAREER) //Entered a career vehicle
            {                
                if (API.getPlayerVehicleSeat(player) == -1) //Driver
                {
                    if (c.Career_Id != veh.Owner_Id)
                    {
                        string career_name = Business.GlobalVariables.Instance.ServerCareers.FirstOrDefault(x => x.Id == veh.Owner_Id).Name;

                        API.sendNotificationToPlayer(player, "Este veículo é restrito a um emprego! (" + career_name + ")");
                        API.warpPlayerOutOfVehicle(player);
                    }
                    else
                    {
                        Entities.Career.CareerType type = Business.GlobalVariables.Instance.ServerCareers.FirstOrDefault(x => x.Id == veh.Owner_Id).Type;
                        switch (type)
                        {
                            case Entities.Career.CareerType.Trucker:
                                if (TruckerBLL.CanDriveTruck(c, veh))
                                {
                                    API.sendNotificationToPlayer(player, "Para começar a trabalhar, digite ~b~/caminhoneiro");
                                }
                                else
                                {
                                    TruckerRank rank;
                                    string needed_rank;
                                    TruckRestrictionsDictionary.TruckMinRank.TryGetValue(veh.Name, out rank);

                                    TruckerRankDictionary.TruckerRankNames.TryGetValue(rank, out needed_rank);

                                    API.sendNotificationToPlayer(player, "Este veículo é restrito a um cargo! (" + needed_rank + ")");
                                    API.warpPlayerOutOfVehicle(player);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                else //Entered a career vehicle as passenger
                {
                    Entities.Career.CareerType type = Business.GlobalVariables.Instance.ServerCareers.FirstOrDefault(x => x.Id == veh.Owner_Id).Type;
                    switch (type)
                    {
                        case Entities.Career.CareerType.Taxi:
                            if (player.hasData("TAXI_DRIVER"))
                            {
                                GrandTheftMultiplayer.Server.Elements.Vehicle elementVeh = ActiveVehicle.GetSpawned(vehicle).VehicleHandle;

                                Character taxiDriver = player.getData("TAXI_DRIVER");                                
                                Client vehicleDriver = VehBLL.Vehicle_GetDriver(elementVeh);

                                Character driverC = ActivePlayer.GetSpawned(vehicleDriver).Character;

                                if(driverC == taxiDriver)//The vehicle's driver is really the taxi that accepted the player's call
                                {
                                    TaxiBLL.StartFare(taxiDriver, c);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void OnPlayerExitVehicle(Client player, NetHandle vehicle)
        {
            Entities.Vehicle.Vehicle veh = ActiveVehicle.GetSpawned(vehicle).Vehicle;
            Entities.Character c = ActivePlayer.Get(player).Character;

            if (veh.Owner_Type == Entities.Vehicle.OwnerType.OWNER_TYPE_CAREER) //Entered a career vehicle
            {                
                Entities.Career.CareerType type = Business.GlobalVariables.Instance.ServerCareers.FirstOrDefault(x => x.Id == veh.Owner_Id).Type;
                switch (type)
                {
                    case Entities.Career.CareerType.Taxi: //Player left a taxi
                        if(player.hasData("TAXI_TIMER")) //Client left taxi
                        {
                            Character driver = player.getData("TAXI_DRIVER");
                            TaxiBLL.FinishFare(driver, c);
                        }
                        else if (player.hasData("TAXI_POSITION")) //Driver left taxi
                        {
                            Character customer = player.getData("TAXI_CUSTOMER");
                            TaxiBLL.FinishFare(c, customer);
                        }
                        break;
                    default:
                        break;
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

            if (VehBLL.Vehicle_HasKey(veh, c) || (veh.Owner_Type == Entities.Vehicle.OwnerType.OWNER_TYPE_FACTION && (veh.Owner_Id == c.Faction_Id)) ||
                (veh.Owner_Type == Entities.Vehicle.OwnerType.OWNER_TYPE_CAREER && (veh.Owner_Id == c.Career_Id)))
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
            GrandTheftMultiplayer.Server.Elements.Vehicle serverVeh = VehBLL.Vehicle_GetNearestInRange(player, 2.0);

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