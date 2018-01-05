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
        public bool CanDriveTruck(Character c, Entities.Vehicle.Vehicle veh)
        {
            TruckerRank rank;
            TruckRestrictionsDictionary.TruckMinRank.TryGetValue(veh.Name, out rank);

            return c.CareerRank >= (int)rank;
        }

        public bool IsValidTruck(Entities.Vehicle.Vehicle veh)
        {            
            return TruckRestrictionsDictionary.TruckMinRank.ContainsKey(veh.Name);
        }        

        public bool CanCarryClass(Entities.Vehicle.Vehicle veh, ProductClass prodClass)
        {
            ProductClass toCheck;
            TruckRestrictionsDictionary.TruckCarryType.TryGetValue(veh.Name, out toCheck);

            return toCheck == prodClass;
        }

        public int GetTruckCapacity(Entities.Vehicle.Vehicle veh)
        {
            int slots;
            TruckRestrictionsDictionary.TruckSlotList.TryGetValue(veh.Name, out slots);

            return slots;
        }

        public int CountCargoOfType(Entities.Vehicle.Vehicle veh, ProductType prodType)
        {
            int count = 0;

            foreach(var prod in veh.Cargo)
            {
                if(prod == prodType)
                {
                    count++;
                }
            }

            return count;
        }

        public Entities.Property.Business GetRandomBusiness(ProductType prodType)
        {            
            List<Entities.Property.Business> compatibleBusinesses = new List<Entities.Property.Business>();

            foreach(var biz in Business.GlobalVariables.Instance.ServerProperties)
            {
                if(biz is Entities.Property.Business)
                {
                    if (BusinessNeededSupplies((Entities.Property.Business)biz).Contains(prodType))
                    {
                        compatibleBusinesses.Add((Entities.Property.Business)biz);
                    }
                }
            }

            if (!compatibleBusinesses.Any())
            {
                return null;
            }

            int l = compatibleBusinesses.Count;
            Random r = new Random();
            int num = r.Next(l);            

            return compatibleBusinesses[num];
        }

        public List<ProductType> BusinessNeededSupplies(Entities.Property.Business b)
        {
            List<ProductType> neededSupplies = new List<ProductType>();
            switch (b.BusinessType)
            {
                case Entities.Property.BusinessType.BUSINESS_TYPE_BAR:
                    neededSupplies.Add(ProductType.Milk);
                    neededSupplies.Add(ProductType.Fruit);                    
                    neededSupplies.Add(ProductType.Meal);
                    break;
                case Entities.Property.BusinessType.BUSINESS_TYPE_BARBER:
                    break;
                case Entities.Property.BusinessType.BUSINESS_TYPE_CLOTHING:
                    break;
                case Entities.Property.BusinessType.BUSINESS_TYPE_DEALERSHIP:
                    neededSupplies.Add(ProductType.Vehicle);
                    break;
                case Entities.Property.BusinessType.BUSINESS_TYPE_ELECTRONICS:
                    break;
                case Entities.Property.BusinessType.BUSINESS_TYPE_FOOD:
                    neededSupplies.Add(ProductType.Milk);
                    neededSupplies.Add(ProductType.Fruit);
                    neededSupplies.Add(ProductType.Corn);
                    neededSupplies.Add(ProductType.Meal);
                    neededSupplies.Add(ProductType.Soy);
                    neededSupplies.Add(ProductType.Wheat);
                    break;
                case Entities.Property.BusinessType.BUSINESS_TYPE_GAS:
                    neededSupplies.Add(ProductType.Gas);
                    break;
                case Entities.Property.BusinessType.BUSINESS_TYPE_GUNSHOP:
                    neededSupplies.Add(ProductType.Weapon);
                    break;
                case Entities.Property.BusinessType.BUSINESS_TYPE_HARDWARE:
                    break;
                case Entities.Property.BusinessType.BUSINESS_TYPE_NIGHTCLUB:
                    neededSupplies.Add(ProductType.Milk);
                    neededSupplies.Add(ProductType.Fruit);
                    neededSupplies.Add(ProductType.Meal);
                    break;
                case Entities.Property.BusinessType.BUSINESS_TYPE_PHARMACY:                    
                    break;
                case Entities.Property.BusinessType.BUSINESS_TYPE_STORE:
                    neededSupplies.Add(ProductType.Milk);
                    neededSupplies.Add(ProductType.Fruit);
                    neededSupplies.Add(ProductType.Corn);
                    neededSupplies.Add(ProductType.Meal);
                    neededSupplies.Add(ProductType.Soy);
                    neededSupplies.Add(ProductType.Wheat);                    
                    break;
                case Entities.Property.BusinessType.BUSINESS_TYPE_TATTOO:
                    break;
                case Entities.Property.BusinessType.BUSINESS_TYPE_WORKSHOP:
                    neededSupplies.Add(ProductType.MetalScrap);
                    break;                
            }

            return neededSupplies;
        }
    }
}
