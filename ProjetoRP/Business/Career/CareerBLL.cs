using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;

namespace ProjetoRP.Business.Career
{
    public class CareerBLL
    {
        public void LoadCareers()
        {
            Business.GlobalVariables.Instance.ServerCareers = SQL_FetchCareers();
        }

        public void SaveCareers()
        {
            using (var context = new DatabaseContext())
            {
                foreach (var career in Business.GlobalVariables.Instance.ServerCareers)
                {
                    context.Careers.Add(career);
                    context.SaveChanges();
                }
            }
        }

        public void Career_Save(Entities.Career.Career career)
        {
            using (var context = new DatabaseContext())
            {
                context.Careers.Attach(career);
                context.Entry(career).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DrawCareersPickups()
        {
            foreach (Entities.Career.Career career in Business.GlobalVariables.Instance.ServerCareers)
            {
                if (career.Public)
                {
                    DrawPickup(career);
                }
                
            }
        }        

        // SQL Functions
        public Entities.Career.Career SQL_FetchCareerData(int career_id)
        {
            Entities.Career.Career career = null;

            using (var context = new DatabaseContext())
            {
                career = (from c in context.Careers where c.Id == career_id select c).AsNoTracking().Single();                
            }

            return career;
        }

        public List<Entities.Career.Career> SQL_FetchCareers()
        {
            List<Entities.Career.Career> careers;
            using (var context = new DatabaseContext())
            {
                careers = (from c in context.Careers select c).AsNoTracking().ToList();                                                          
            }
            return careers;
        }

        public void Career_Create(Entities.Career.Career career, int dimension)
        {
            using (var context = new DatabaseContext())
            {
                context.Careers.Add(career);
                context.SaveChanges();
            }

            Business.GlobalVariables.Instance.ServerCareers.Add(career);

            DrawPickup(career);           
        }

        public void Career_Delete(Entities.Career.Career career)
        {
            using (var context = new DatabaseContext())
            {
                context.Careers.Attach(career);
                context.Careers.Remove(career);
                context.SaveChanges();
            }

            Business.GlobalVariables.Instance.ServerCareers.Remove(career);

            DeletePickup(career);
        }

        public void DrawPickup(Entities.Career.Career career)
        {
            string careerText = string.Format("~w~Emprego de ~y~{0}~w~\n\nDigite ~b~/emprego ~w~para aplicar.", career.Name);
            career.Pickup = API.shared.createMarker(0, new Vector3(career.X, career.Y, career.Z - 0.25), new Vector3(), new Vector3(), new Vector3(0.5, 0.5, 0.5), 125, 255, 255, 0, career.Dimension);
            career.TextLabel = API.shared.createTextLabel(careerText, new Vector3(career.X, career.Y, career.Z + 1.0), 20.0f, 1.0f, false, career.Dimension);            
        }

        public void DeletePickup(Entities.Career.Career career)
        {
            if (career.Pickup != null)
            {
                API.shared.deleteEntity(career.Pickup);
                career.Pickup = null;
            }
            if (career.TextLabel != null)
            {
                API.shared.deleteEntity(career.TextLabel);
                career.TextLabel = null;
            }
        }

        public void RedrawPickup(Entities.Career.Career career)
        {
            DeletePickup(career);
            DrawPickup(career);
        }

        public Entities.Career.Career FindCareerById(int id) //Should we be using C#'s predicate List find?
        {
            Entities.Career.Career found = null;

            foreach (var career in Business.GlobalVariables.Instance.ServerCareers)
            {
                if (career.Id == id)
                {
                    found = career;
                    break;
                }
            }

            return found;
        }

        public Entities.Career.Career Career_GetNearestInRange(Client player, double range)
        {
            Vector3 playerPos = API.shared.getEntityPosition(player);

            Entities.Career.Career nearestCareer = null;

            double nearestDistance = range;

            foreach (var career in Business.GlobalVariables.Instance.ServerCareers)
            {
                Vector3 careerPos;
                careerPos = new Vector3(career.X, career.Y, career.Z);

                float distance = playerPos.DistanceTo(careerPos);

                if (career.Dimension == player.dimension && distance <= range && distance <= nearestDistance)
                {
                    nearestDistance = distance;
                    nearestCareer = career;
                }
            }
            return nearestCareer;
        }        
    }
}