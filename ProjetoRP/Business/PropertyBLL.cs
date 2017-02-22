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
        public Entities.Property.IProperty<Entities.Property.Property> HouseBll = new Business.HouseBLL();
        public Entities.Property.IProperty<Entities.Property.Property> BusinessBll = new Business.BusinessBLL();        

        public void LoadProperties()
        {
            Business.GlobalVariables.Instance.ServerProperties = SQL_FetchProperties();
        }

        public void SaveProperties()
        {
            using (var context = new DatabaseContext())
            {
                foreach (var prop in Business.GlobalVariables.Instance.ServerProperties)
                {
                    context.Properties.Add(prop);
                    context.SaveChanges();
                }
            }
        }

        public void DrawPropertiesPickups()
        {            
            foreach (Entities.Property.Property prop in Business.GlobalVariables.Instance.ServerProperties)
            {
                Entities.Property.IProperty<Entities.Property.Property> bll = null;

                if (prop is Entities.Property.House)
                {
                    bll = HouseBll;
                }
                else if (prop is Entities.Property.Business)
                {
                    bll = BusinessBll;
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

        public void Property_Create(Entities.Property.Property prop, int dimension)
        {
            using (var context = new DatabaseContext())
            {
                context.Properties.Add(prop);
                context.SaveChanges();                
            }

            Business.GlobalVariables.Instance.ServerProperties.Add(prop);

            Entities.Property.IProperty<Entities.Property.Property> bll = null;

            if (prop is Entities.Property.House)
            {
                bll = HouseBll;
            }
            else if (prop is Entities.Property.Business)
            {
                bll = BusinessBll;
            }

            bll.DrawPickup(prop);
            Business.DoorBLL DoorBLL = new Business.DoorBLL();
            DoorBLL.Door_Create(prop, 0, true, new Vector3(prop.X, prop.Y, prop.Z), dimension, new Vector3(-18.77586, -581.755, 90.11491), prop.Id);
        }

        public void Property_Delete(Entities.Property.Property prop)
        {
            using (var context = new DatabaseContext())
            {
                context.Properties.Attach(prop);
                context.Properties.Remove(prop);
                context.SaveChanges();
            }

            Business.DoorBLL DoorBLL = new Business.DoorBLL();
            DoorBLL.Door_DeleteFromProperty(prop);

            Business.GlobalVariables.Instance.ServerProperties.Remove(prop);

            DeletePickup(prop);
        }

        public void DeletePickup(Entities.Property.Property prop)
        {
            if(prop.Pickup != null)
            {
                API.shared.deleteEntity(prop.Pickup);
                prop.Pickup = null;                
            }
            if(prop.TextLabel != null)
            {
                API.shared.deleteEntity(prop.TextLabel);
                prop.TextLabel = null;
            }
        }

        public Entities.Property.Property FindPropertyById(int id) //Should we be using C#'s predicate List find?
        {
            Entities.Property.Property found = null;

            foreach(var prop in Business.GlobalVariables.Instance.ServerProperties)
            {
                if(prop.Id == id)
                {
                    found = prop;
                    break;
                }
            }

            return found;
        }
    }
}
