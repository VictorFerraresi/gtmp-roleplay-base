using GTANetworkServer;
using ProjetoRP.Entities;
using ProjetoRP.Types;
using ProjetoRP.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Business.Vehicle
{
    public class ActiveVehicle
    {
        private const int MaxVehicles = 1000;
        private static List<ActiveVehicle> VehicleServices = new List<ActiveVehicle>();

        public Entities.Vehicle.Vehicle Vehicle { get; set; }
        public GTANetworkServer.Vehicle VehicleHandle { get; set; }        
        public VehicleStatus Status { get; set; }
        public int? Id { get; private set; }

        public ActiveVehicle(Entities.Vehicle.Vehicle vehicle)
        {
            if (Get(vehicle) != null)
            {
                throw new Exceptions.Vehicle.ActiveVehicleAlreadyExistsException();
            }

            Vehicle = vehicle;
            Status = VehicleStatus.NotSpawned;

            PushAndAssignId();
        }

        public void Dispose()
        {
            VehicleServices.Remove(this);
        }

        public static ActiveVehicle Create(Entities.Vehicle.Vehicle vehicle)
        {
            return new ActiveVehicle(vehicle);
        }

        public static ActiveVehicle Get(int id)
        {
            foreach (var av in VehicleServices)
            {
                if (av.Id == id)
                {
                    return av;
                }
            }
            return null;
        }

        public static ActiveVehicle GetSpawned(int id)
        {
            var av = Get(id);
            if (av.Status == VehicleStatus.Spawned)
            {
                return av;
            }
            else
            {
                return null;
            }
        }

        public static ActiveVehicle GetSpawned(GTANetworkServer.Vehicle vehicleHandle)
        {
            var av = Get(vehicleHandle);
            if (av.Status == VehicleStatus.Spawned)
            {
                return av;
            }
            else
            {
                return null;
            }
        }

        public static ActiveVehicle GetSpawned(Entities.Vehicle.Vehicle vehicle)
        {
            var av = Get(vehicle);
            if (av.Status == VehicleStatus.Spawned)
            {
                return av;
            }
            else
            {
                return null;
            }
        }

        public static ActiveVehicle GetSpawned(GTANetworkShared.NetHandle vehicleHandle)
        {
            foreach (var av in VehicleServices)
            {
                if (av.Status == VehicleStatus.Spawned && av.VehicleHandle.handle == vehicleHandle)
                {
                    return av;
                }
            }
            return null;
        }

        public static ActiveVehicle Get(Entities.Vehicle.Vehicle vehicle)
        {
            foreach (var av in VehicleServices)
            {
                if (av.Vehicle == vehicle)
                {
                    return av;
                }
            }
            return null;
        }

        public static ActiveVehicle Get(GTANetworkServer.Vehicle vehicleHandle)
        {
            foreach (var av in VehicleServices)
            {
                if (av.VehicleHandle == vehicleHandle)
                {
                    return av;
                }
            }
            return null;
        }        

        public static ActiveVehicle GetBySQLID(int sqlID)
        {
            foreach (var av in VehicleServices)
            {
                if (av.Vehicle.Id == sqlID)
                {
                    return av;
                }
            }
            return null;
        }

        public static List<ActiveVehicle> GetAll()
        {
            return VehicleServices;
        }

        public static List<ActiveVehicle> GetAllSpawned()
        {
            List<ActiveVehicle> SpawnedVehicles = new List<ActiveVehicle>();

            foreach(var v in VehicleServices)
            {
                if (v.Status == VehicleStatus.Spawned)
                {
                    SpawnedVehicles.Add(v);
                }
            }
            return SpawnedVehicles;
        }

        private void PushAndAssignId()
        {
            if (Id == null)
            {
                Id = GetVacantId();
                VehicleServices.Add(this);
            }
        }

        private static int GetVacantId()
        {
            for (var i = 0; i < MaxVehicles; i++)
            {
                if (Get(i) == null)
                {
                    return i;
                }
            }
            throw new Exceptions.Vehicle.NoRemainingVehicleSlotsException();
        }
    }
}
