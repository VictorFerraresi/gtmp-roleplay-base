using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using ProjetoRP.Entities;
using ProjetoRP.Types;

namespace ProjetoRP.Business.Career
{
    public class TruckerCareerBLL
    {
        public bool CanDriveTruck(Character c, ProjetoRP.Entities.Vehicle.Vehicle veh)
        {
            TruckerRank rank;
            TruckRestrictionsDictionary.TruckMinRank.TryGetValue(veh.Name, out rank);

            return c.CareerRank >= (int)rank;
        }
    }
}