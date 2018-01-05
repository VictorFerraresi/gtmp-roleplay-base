using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Shared.Math;
using ProjetoRP.Types;
using GrandTheftMultiplayer.Server.Elements;

namespace ProjetoRP.Business.Industry
{
    public class IndustryBLL
    {
        public void LoadIndustries()
        {
            Business.GlobalVariables.Instance.ServerIndustries = SQL_FetchIndustries();
        }

        public void SaveIndustries()
        {
            using (var context = new DatabaseContext())
            {
                foreach (var industry in Business.GlobalVariables.Instance.ServerIndustries)
                {
                    Industry_Save(industry);
                }
            }
        }

        public void Industry_Save(Entities.Industry.Industry industry)
        {
            using (var context = new DatabaseContext())
            {
                context.Industries.Attach(industry);
                context.Entry(industry).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DrawIndustriesPickups()
        {
            foreach (Entities.Industry.Industry industry in Business.GlobalVariables.Instance.ServerIndustries)
            {
                string label = string.Format("~w~Indústria\n[~b~{0}~w~]", industry.Name);

                industry.Pickup = API.shared.createMarker(0, new Vector3(industry.X, industry.Y, industry.Z - 0.25), new Vector3(), new Vector3(), new Vector3(0.5, 0.5, 0.5), 125, 255, 255, 255, industry.Dimension);
                industry.TextLabel = API.shared.createTextLabel(label, new Vector3(industry.X, industry.Y, industry.Z + 0.5), 20.0f, 0.5f, false, industry.Dimension);
                API.shared.setTextLabelColor(industry.TextLabel, 255, 255, 255, 255);
            }
        }

        public void Industry_Create(string name, Vector3 pos, int dimension)
        {
            Entities.Industry.Industry industry = new Entities.Industry.Industry();

            industry.Name = name;
            industry.X = pos.X;
            industry.Y = pos.Y;
            industry.Z = pos.Z;
            industry.Dimension = dimension;

            using (var context = new DatabaseContext())
            {
                context.Industries.Attach(industry);
                context.Industries.Add(industry);
                context.SaveChanges();
            }            

            Business.GlobalVariables.Instance.ServerIndustries.Add(industry);
        }

        public void Industry_Delete(Entities.Industry.Industry industry)
        {
            using (var context = new DatabaseContext())
            {
                context.Industries.Attach(industry);
                context.Industries.Remove(industry);
                context.SaveChanges();
            }

            Business.GlobalVariables.Instance.ServerIndustries.Remove(industry);
        }   
        
        public string LoadPoint_GetProductName(ProductType prodType)
        {
            string name;
            ProductTypeDictionary.ProductTypeNames.TryGetValue(prodType, out name);
            return name;
        }

        public void LoadPoint_Save(Entities.Industry.LoadPoint loadpoint)
        {
            using (var context = new DatabaseContext())
            {
                context.LoadPoints.Attach(loadpoint);
                context.Entry(loadpoint).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DrawLoadPointsPickups()
        {
            foreach (Entities.Industry.Industry industry in Business.GlobalVariables.Instance.ServerIndustries)
            {
                foreach(Entities.Industry.LoadPoint loadpoint in industry.LoadPoints)
                {
                    string label = string.Format("Ponto de Carga\n~w~[~y~{0}~w~]", LoadPoint_GetProductName(loadpoint.ProductType)); 
                                     
                    loadpoint.Pickup = API.shared.createMarker(20, new Vector3(loadpoint.X, loadpoint.Y, loadpoint.Z), new Vector3(), new Vector3(), new Vector3(0.5, 0.5, 0.5), 125, 255, 255, 255, loadpoint.Dimension);
                    loadpoint.TextLabel = API.shared.createTextLabel(label, new Vector3(loadpoint.X, loadpoint.Y, loadpoint.Z + 0.5), 20.0f, 0.5f, false, loadpoint.Dimension);
                    API.shared.setTextLabelColor(loadpoint.TextLabel, 255, 255, 255, 255);
                }                
            }
        }

        public void LoadPoint_Create(Entities.Industry.LoadPoint loadpoint)
        {
            using (var context = new DatabaseContext())
            {
                context.LoadPoints.Attach(loadpoint);
                context.LoadPoints.Add(loadpoint);
                context.SaveChanges();
            }
        }

        public void LoadPoint_Delete(Entities.Industry.LoadPoint loadpoint)
        {
            using (var context = new DatabaseContext())
            {
                context.LoadPoints.Attach(loadpoint);
                context.LoadPoints.Remove(loadpoint);
                context.SaveChanges();
            }
        }

        public Entities.Industry.Industry FindIndustryById(int id) //Should we be using C#'s predicate List find?
        {
            Entities.Industry.Industry found = null;

            foreach (var industry in Business.GlobalVariables.Instance.ServerIndustries)
            {
                if (industry.Id == id)
                {
                    found = industry;
                    break;
                }
            }

            return found;
        }

        public Entities.Industry.LoadPoint FindLoadPointById(int id) //Should we be using C#'s predicate List find?
        {
            Entities.Industry.LoadPoint found = null;

            foreach (var industry in Business.GlobalVariables.Instance.ServerIndustries)
            {
                foreach (var loadpoint in industry.LoadPoints)
                {
                    if(loadpoint.Id == id)
                    {
                        found = loadpoint;
                        break;
                    }
                }
            }

            return found;
        }

        public Entities.Industry.LoadPoint LoadPoint_GetNearestInRange(Client player, double range)
        {
            Vector3 playerPos = API.shared.getEntityPosition(player);

            Entities.Industry.LoadPoint nearestLp = null;

            double nearestDistance = range;

            foreach (Entities.Industry.Industry industry in Business.GlobalVariables.Instance.ServerIndustries)
            {         
                foreach (Entities.Industry.LoadPoint lp in industry.LoadPoints)
                {
                    Vector3 lpPos = new Vector3(lp.X, lp.Y, lp.Z);
                    float distance = playerPos.DistanceTo(lpPos);

                    if (distance <= range && distance <= nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestLp = lp;
                    }
                }                
            }
            return nearestLp;
        }

        public ProductClass GetProductClassFromType(ProductType prodType)
        {
            ProductClass prodClass;
            ProductTypeDictionary.ProductTypeClasses.TryGetValue(prodType, out prodClass);

            return prodClass;
        }

        public string GetProductClassName(ProductClass prodClass)
        {
            string prodName;
            ProductTypeDictionary.ProductClassNames.TryGetValue(prodClass, out prodName);
            return prodName;
        }

        // SQL Functions
        public Entities.Industry.Industry SQL_FetchIndustryData(int industry_id)
        {
            Entities.Industry.Industry industry = null;

            using (var context = new DatabaseContext())
            {
                industry = (from i in context.Industries where i.Id == industry_id select i).Include(i => i.LoadPoints).AsNoTracking().Single();
            }

            return industry;
        }

        public List<Entities.Industry.Industry> SQL_FetchIndustries()
        {
            List<Entities.Industry.Industry> industries = new List<Entities.Industry.Industry>();

            using (var context = new DatabaseContext())
            {
                industries = (from i in context.Industries select i).Include(i => i.LoadPoints).AsNoTracking().ToList();
            }
            return industries;
        }        
                
    }
}