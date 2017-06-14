using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using System.Data.Entity;
using ProjetoRP.Business.Player;

namespace ProjetoRP.Business.Vehicle
{
    public class VehicleBLL
    {
        Business.Item.ItemService iService = new Business.Item.ItemService(new DatabaseContext());

        public void LoadVehicles()
        {
            List<Entities.Vehicle.Vehicle> vehicles = new List<Entities.Vehicle.Vehicle>();

            using (var context = new DatabaseContext())
            {
                vehicles = (from v in context.Vehicles select v).AsNoTracking().ToList();
            }            

            foreach(var v in vehicles)
            {
                var av = new ActiveVehicle(v);
                av.Status = Types.VehicleStatus.NotSpawned;
                if (Vehicle_IsFactionOwned(v) || Vehicle_IsCareerOwned(v))
                {                    
                    Vehicle_Spawn(v);
                }
            }
        }

        public bool Vehicle_IsFactionOwned(Entities.Vehicle.Vehicle veh)
        {
            return veh.Owner_Type == Entities.Vehicle.OwnerType.OWNER_TYPE_FACTION;
        }

        public bool Vehicle_IsCharacterOwned(Entities.Vehicle.Vehicle veh)
        {
            return veh.Owner_Type == Entities.Vehicle.OwnerType.OWNER_TYPE_CHARACTER;
        }

        public bool Vehicle_IsCareerOwned(Entities.Vehicle.Vehicle veh)
        {
            return veh.Owner_Type == Entities.Vehicle.OwnerType.OWNER_TYPE_CAREER;
        }

        public bool Vehicle_IsSpawned(Entities.Vehicle.Vehicle veh)
        {
            return ActiveVehicle.GetBySQLID(veh.Id).Status == Types.VehicleStatus.Spawned;            
        }

        public void Vehicle_Spawn(Entities.Vehicle.Vehicle veh)
        {
            Vector3 pos = new Vector3(veh.X, veh.Y, veh.Z);
            Vector3 rot = new Vector3(veh.rX, veh.rY, veh.rZ);

            GrandTheftMultiplayer.Server.Elements.Vehicle serverVeh = API.shared.createVehicle(API.shared.vehicleNameToModel(veh.Name), pos, rot, veh.Color1, veh.Color2, veh.Dimension);
            API.shared.setVehicleEngineStatus(serverVeh, veh.Engine);
            API.shared.setVehicleLocked(serverVeh, veh.Locked);
            API.shared.setVehicleHealth(serverVeh, veh.Health);
            API.shared.setVehicleNumberPlate(serverVeh, veh.LicensePlate);            

            ActiveVehicle av = Business.Vehicle.ActiveVehicle.GetBySQLID(veh.Id);

            if(veh.Name == "Sheriff")
            {
                API.shared.setVehicleExtra(serverVeh, 1, true); //Lightbar
                API.shared.setVehicleExtra(serverVeh, 2, false); //Rotating Lightbar
            }
            
            av.VehicleHandle = serverVeh;
            av.Status = Types.VehicleStatus.Spawned;            
        }

        public bool Vehicle_IsOwner(Entities.Character character, Entities.Vehicle.Vehicle veh)
        {            
            return (Vehicle_IsCharacterOwned(veh) && veh.Owner_Id == character.Id);
        }

        public GrandTheftMultiplayer.Server.Elements.Vehicle Vehicle_GetNearestInRange(Client player, double range)
        {
            Vector3 playerPos = API.shared.getEntityPosition(player);

            GrandTheftMultiplayer.Server.Elements.Vehicle nearestVeh = null;

            double nearestDistance = range;

            foreach (Business.Vehicle.ActiveVehicle av in Business.Vehicle.ActiveVehicle.GetAllSpawned())
            {
                GrandTheftMultiplayer.Server.Elements.Vehicle veh = av.VehicleHandle;                

                Vector3 vehPos = API.shared.getEntityPosition(veh);
                float distance = playerPos.DistanceTo(vehPos);

                if (distance <= range && distance <= nearestDistance)
                {
                    nearestDistance = distance;
                    nearestVeh = veh;
                }
            }
            return nearestVeh;
        }

        public bool Vehicle_IsLocked(Entities.Vehicle.Vehicle veh)
        {
            return veh.Locked;
        }

        public void Vehicle_LockCommand(Client player, Entities.Vehicle.Vehicle veh)
        {
            var ac = ActivePlayer.GetSpawned(player);
            if (ac == null) return;

            var av = ActiveVehicle.GetSpawned(veh);
            if (av == null) return;

            Entities.Character c = ac.Character;

            if (Vehicle_HasKey(veh, c) || (veh.Owner_Type == Entities.Vehicle.OwnerType.OWNER_TYPE_FACTION && (veh.Owner_Id == c.Faction_Id)))
            {            
                if (Vehicle_IsLocked(veh))
                {
                    API.shared.setVehicleLocked(av.VehicleHandle, false);
                    veh.Locked = false;
                    API.shared.sendNotificationToPlayer(player, "Veículo destrancado");
                }
                else
                {
                    API.shared.setVehicleLocked(av.VehicleHandle, true);
                    veh.Locked = true;
                    API.shared.sendNotificationToPlayer(player, "Veículo trancado");
                }
            }
            else
            {
                API.shared.sendChatMessageToPlayer(player, "Você não possui as chaves deste veículo!");
            }
        }

        public bool Vehicle_HasKey(Entities.Vehicle.Vehicle veh, Entities.Character c)
        {
            var data = iService.GetCascadingItemsFromPlayer(c);
            foreach (var item in data)
            {
                if (item is Entities.ItemModel.CarKey)
                {
                    Entities.ItemModel.CarKey key = (Entities.ItemModel.CarKey)item;
                    if (key.Vehicle_Id == veh.Id)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public string Vehicle_GeneratePlate()
        {
            char[] charactersAvailable = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

            string plate = "";

            return plate;
        }

        public bool Vehicle_IsNearPlayer(Entities.Vehicle.Vehicle veh, Client player, double range = 5.0)
        {
            GrandTheftMultiplayer.Server.Elements.Vehicle serverVeh = ActiveVehicle.GetSpawned(veh).VehicleHandle;

            return API.shared.getEntityPosition(serverVeh).DistanceTo(API.shared.getEntityPosition(player)) <= range;
        }

        // SQL Functions
        public Entities.Vehicle.Vehicle SQL_FetchVehicleData(int vehicle_id)
        {
            Entities.Vehicle.Vehicle veh = null;

            using (var context = new DatabaseContext())
            {
                veh = (from v in context.Vehicles where v.Id == vehicle_id select v).AsNoTracking().Single();                                            
            }

            return veh;
        }

        public List<Entities.Vehicle.Vehicle> SQL_FetchVehiclesFromCharacter(Entities.Character character)
        {
            List<Entities.Vehicle.Vehicle> vehs;
            using (var context = new DatabaseContext())
            {
                vehs = (from v in context.Vehicles
                        where v.Owner_Id == character.Id && (Entities.Vehicle.OwnerType)v.Owner_Type == Entities.Vehicle.OwnerType.OWNER_TYPE_CHARACTER
                        select v).AsNoTracking().ToList();                
            }
            return vehs;
        }

        public void Vehicle_Save(Entities.Vehicle.Vehicle veh)
        {
            using (var context = new DatabaseContext())
            {
                context.Database.Log = s => API.shared.consoleOutput(s);

                context.Vehicles.Attach(veh);
                context.Entry(veh).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
