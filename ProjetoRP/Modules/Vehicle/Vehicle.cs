using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ProjetoRP.Modules.Vehicle
{
    public class Vehicle : Script
    {
        public List<GrandTheftMultiplayer.Server.Elements.Vehicle> ServerVehicles = new List<GrandTheftMultiplayer.Server.Elements.Vehicle>();

        public Vehicle()
        {
            API.onResourceStart += OnResourceStart;            
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        public void OnResourceStart()
        {
            API.consoleOutput(Messages.console_startup);
        }        

        public void OnClientEventTrigger(Client player, string eventName, object[] args)
        {
            switch (eventName)
            {
                case "CS_VEHICLE_SPAWN":
                    var data = API.fromJson((string)args[0]);

                    Entities.Vehicle veh = new Entities.Vehicle();
                    veh.Id = (int)data.Id;                    
                    veh.Character = player.getData("CHARACTER_DATA");
                    veh.Name = (string)data.Name;
                    
                    if (Vehicle_IsOwner(veh.Character, veh))
                    {
                        if (!Vehicle_IsSpawned(veh))
                        {
                            veh = SQL_FetchVehicleData(veh.Id);
                            Vehicle_Spawn(veh);
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

        private bool Vehicle_IsSpawned(Entities.Vehicle veh)
        {
            List<NetHandle> vehicles = API.getAllVehicles();
            foreach (NetHandle loopveh in vehicles)
            {
                Entities.Vehicle toCheck = API.getEntityData(loopveh, "VEHICLE_DATA");
                if (toCheck.Id == veh.Id && toCheck.Spawned)
                {
                    return true;
                }
            }
            return false;
        }

        private void Vehicle_Spawn(Entities.Vehicle veh)
        {
            Vector3 pos = new Vector3(veh.X, veh.Y, veh.Z);
            Vector3 rot = new Vector3(veh.rX, veh.rY, veh.rZ);

            GrandTheftMultiplayer.Server.Elements.Vehicle serverVeh = API.createVehicle(API.vehicleNameToModel(veh.Name), pos, rot, veh.Color1, veh.Color2, veh.Dimension);
            API.setVehicleEngineStatus(serverVeh, veh.Engine);
            API.setVehicleLocked(serverVeh, veh.Locked);
            API.setVehicleHealth(serverVeh, veh.Health);

            veh.Spawned = true;

            serverVeh.setData("VEHICLE_DATA", veh);

            ServerVehicles.Add(serverVeh);
        }

        private bool Vehicle_IsOwner(Entities.Character character, Entities.Vehicle veh)
        {
            return veh.Character == character;
        }

        private GrandTheftMultiplayer.Server.Elements.Vehicle Vehicle_GetNearestInRange(Client player, double range)
        {
            Vector3 playerPos = API.getEntityPosition(player);

            GrandTheftMultiplayer.Server.Elements.Vehicle nearestVeh = null;

            double nearestDistance = range;

            foreach (GrandTheftMultiplayer.Server.Elements.Vehicle veh in ServerVehicles)
            {
                Vector3 vehPos = API.getEntityPosition(veh);
                float distance = playerPos.DistanceTo(vehPos);

                if (distance <= range && distance <= nearestDistance)
                {
                    nearestDistance = distance;
                    nearestVeh = veh;
                }
            }
            return nearestVeh;
        }      

        private bool Vehicle_IsLocked(Entities.Vehicle veh)
        {
            return veh.Locked;
        }


        // SQL Functions
        private Entities.Vehicle SQL_FetchVehicleData(int vehicle_id)
        {
            Entities.Vehicle veh = null;

            using (var context = new DatabaseContext())
            {
                veh = (from v in context.Vehicles where v.Id == vehicle_id select v).AsNoTracking().Single();
                // AsNoTracking "detaches" the entity from the Context, allowing it to be kept in memory and used as please up until reattached again @Player_Save                                
            }

            return veh;
        }

        private List<Entities.Vehicle> SQL_FetchVehiclesFromCharacter(Entities.Character character)
        {
            List<Entities.Vehicle> vehs = new List<Entities.Vehicle>();

            using (var context = new DatabaseContext())
            {
                vehs = (from v in context.Vehicles where v.Character.Id == character.Id select v).Include(v => v.Character).AsNoTracking().ToList();
                // AsNoTracking "detaches" the entity from the Context, allowing it to be kept in memory and used as please up until reattached again @Player_Save                                
            }            
            return vehs;
        }

        public void Vehicle_Save(Entities.Vehicle veh)
        {
            using (var context = new DatabaseContext())
            {
                context.Database.Log = s => API.shared.consoleOutput(s);

                context.Vehicles.Attach(veh);
                context.Entry(veh).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        private void Vehicle_KickForInvalidTrigger(Client player)
        {
            player.kick(Player.Messages.player_kicked_inconsistency);
        }


        //Commands
        [Command("v")]
        public void VCommand(Client player)
        {            
            List<Entities.Vehicle> vehs = SQL_FetchVehiclesFromCharacter(player.getData("CHARACTER_DATA"));            
            API.triggerClientEvent(player, "SC_SHOW_VEHICLEMENU", API.toJson(vehs));
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
            Entities.Vehicle veh = API.getEntityData(serverVeh, "VEHICLE_DATA");

            if (!Vehicle_IsOwner(player.getData("CHARACTER_DATA"), veh))
            {
                API.sendChatMessageToPlayer(player, Messages.vehicle_no_keys);
                return;
            }

            if (veh.Engine == true)
            {
                API.setVehicleEngineStatus(serverVeh, false);
                veh.Engine = false;
            }
            else
            {
                API.setVehicleEngineStatus(serverVeh, true);
                veh.Engine = true;
            }
        }

        //[Command("trancar")]
        //public void LockCommand(Client player)
        //{
        //    GrandTheftMultiplayer.Server.Elements.Vehicle serverVeh = Vehicle_GetNearestInRange(player, 4.0);

        //    if (serverVeh == null)
        //    {
        //        API.sendChatMessageToPlayer(player, Messages.vehicle_not_near_any);
        //        return;
        //    }

        //    Entities.Vehicle veh = API.getEntityData(serverVeh, "VEHICLE_DATA");

        //    if (!Vehicle_IsOwner(player.getData("CHARACTER_DATA"), veh)) //GETCHARID
        //    {
        //        API.sendChatMessageToPlayer(player, Messages.vehicle_no_keys);
        //        return;
        //    }

        //    if (veh.Locked == true)
        //    {
        //        API.setVehicleLocked(serverVeh, false);
        //        veh.Locked = false;
        //    }
        //    else
        //    {
        //        API.setVehicleLocked(serverVeh, true);
        //        veh.Locked = true;
        //    }
        //}

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
            GrandTheftMultiplayer.Server.Elements.Vehicle serverVeh = Vehicle_GetNearestInRange(player, 2.0);

            if (serverVeh == null)
            {
                API.sendChatMessageToPlayer(player, Messages.vehicle_not_near_any);
                return;
            }

            Entities.Vehicle veh = API.getEntityData(serverVeh, "VEHICLE_DATA");

            switch (action)
            {
                case "abrir":
                    if (Vehicle_IsLocked(veh))
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
