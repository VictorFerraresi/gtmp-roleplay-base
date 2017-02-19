using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using GTANetworkServer;
using GTANetworkShared;

namespace ProjetoRP.Business
{
    public class PropertyBLL
    {
        public List<Entities.Property.Property> ServerProperties;

        public void Property_Create(Entities.Property.Property prop)
        {                        
            using (var context = new DatabaseContext())
            {
                context.Properties.Add(prop);                
                context.SaveChanges();
            }
        }        

        public void DrawPropertiesPickups()
        {
            foreach (Entities.Property.Property prop in ServerProperties)
            {
                Entities.Property.IProperty<Entities.Property.Property> bll = null;

                if (prop is Entities.Property.House)
                {
                    bll = new Business.HouseBLL();
                }
                else if (prop is Entities.Property.Business)
                {
                    bll = new Business.BusinessBLL();
                }

                bll.DrawPickup(prop);
            }
        }

        // SQL Functions
        public Entities.Property.Property SQL_FetchPropertyData(int property_id)
        {
            Entities.Property.Property prop = null;

            using (var context = new DatabaseContext())
            {
                prop = (from p in context.Properties where p.Id == property_id select p).AsNoTracking().Single();
                // AsNoTracking "detaches" the entity from the Context, allowing it to be kept in memory and used as please up until reattached again @Player_Save                                
            }

            return prop;
        }

        public List<Entities.Property.Property> SQL_FetchProperties()
        {
            List<Entities.Property.Property> properties = new List<Entities.Property.Property>();

            using (var context = new DatabaseContext())
            {
                var houses = (from p in context.Properties select p).OfType<Entities.Property.House>().Include(p => p.Owner).AsNoTracking().ToList();
                var businesses = (from p in context.Properties select p).OfType<Entities.Property.Business>().Include(p => p.Owner).AsNoTracking().ToList();
                properties = houses.Cast<Entities.Property.Property>().ToList();
                properties = properties.Concat(businesses.Cast<Entities.Property.Property>().ToList()).ToList();

                // AsNoTracking "detaches" the entity from the Context, allowing it to be kept in memory and used as please up until reattached again @Player_Save                                
            }
            return properties;
        }
    }
}
