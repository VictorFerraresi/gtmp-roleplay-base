using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkServer;
using GTANetworkShared;
using System.Data.Entity;

namespace ProjetoRP.Business.Vehicle
{
    public class VehicleBLL
    {
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
                if (Vehicle_IsFactionOwned(v))
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

        public bool Vehicle_IsSpawned(Entities.Vehicle.Vehicle veh)
        {
            return ActiveVehicle.GetBySQLID(veh.Id).Status == Types.VehicleStatus.Spawned;            
        }

        public void Vehicle_Spawn(Entities.Vehicle.Vehicle veh)
        {
            Vector3 pos = new Vector3(veh.X, veh.Y, veh.Z);
            Vector3 rot = new Vector3(veh.rX, veh.rY, veh.rZ);

            GTANetworkServer.Vehicle serverVeh = API.shared.createVehicle(API.shared.vehicleNameToModel(veh.Name), pos, rot, veh.Color1, veh.Color2, veh.Dimension);
            API.shared.setVehicleEngineStatus(serverVeh, veh.Engine);
            API.shared.setVehicleLocked(serverVeh, veh.Locked);
            API.shared.setVehicleHealth(serverVeh, veh.Health);
            API.shared.setVehicleNumberPlate(serverVeh, veh.LicensePlate);

            ActiveVehicle av = Business.Vehicle.ActiveVehicle.GetBySQLID(veh.Id);
            
            av.VehicleHandle = serverVeh;
            av.Status = Types.VehicleStatus.Spawned;            
        }

        public bool Vehicle_IsOwner(Entities.Character character, Entities.Vehicle.Vehicle veh)
        {            
            return (Vehicle_IsCharacterOwned(veh) && veh.Owner_Id == character.Id);
        }

        public GTANetworkServer.Vehicle Vehicle_GetNearestInRange(Client player, double range)
        {
            Vector3 playerPos = API.shared.getEntityPosition(player);

            GTANetworkServer.Vehicle nearestVeh = null;

            double nearestDistance = range;

            foreach (Business.Vehicle.ActiveVehicle av in Business.Vehicle.ActiveVehicle.GetAll())
            {
                GTANetworkServer.Vehicle veh = av.VehicleHandle;

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

        public string Vehicle_GeneratePlate()
        {
            char[] charactersAvailable = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };          

            string plate = "";

            return plate;
        }
    }
}
