using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using GTANetworkServer;
using GTANetworkShared;

namespace ProjetoRP.Business.Property
{
    public class PropertyBLL
    {        
        public Entities.Property.IProperty<Entities.Property.Property> HouseBll = new HouseBLL();
        public Entities.Property.IProperty<Entities.Property.Property> BusinessBll = new BusinessBLL();

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

        public void Property_Save(Entities.Property.Property prop)
        {            
            using (var context = new DatabaseContext())
            {
                context.Properties.Attach(prop);                
                context.Entry(prop).State = EntityState.Modified;
                context.SaveChanges();
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

        public bool Property_Validate(string address, int type, string price, out string msg)
        {           
            if (Business.GlobalVariables.Instance.ServerProperties.Find(x => x.Address == address) != null)
            {                
                msg = "Já existe uma propriedade com este endereço!";
                return false;
            }
            if (!Enum.IsDefined(typeof(Entities.Property.PropertyType), type))
            {                
                msg = "Este tipo de é propriedade inválido!";
                return false;
            }

            int priceVal = 0;

            if (!int.TryParse(price, out priceVal))
            {                
                msg = "Digite apenas números no campo do preço!";
                return false;
            }
            if (priceVal < 1)
            {                
                msg = "Escolha um valor para o preço maior do que 0!";
                return false;
            }            
            msg = "Você criou esta propriedade com sucesso!";
            return true;
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

            DrawPickup(prop);
            DoorBLL DoorBLL = new DoorBLL();
            DoorBLL.Door_Create(prop, 0, true, new Vector3(prop.X, prop.Y, prop.Z), dimension, new Vector3(-18.77586, -581.755, 90.11491), prop.Id);
        }

        public void Property_Delete(Entities.Property.Property prop)
        {
            DoorBLL DoorBLL = new DoorBLL();
            DoorBLL.Door_DeleteFromProperty(prop);

            using (var context = new DatabaseContext())
            {                
                context.Properties.Attach(prop);
                context.Properties.Remove(prop);                
                context.SaveChanges();
            }
            
            Business.GlobalVariables.Instance.ServerProperties.Remove(prop);

            DeletePickup(prop);
        }

        public void DrawPickup(Entities.Property.Property prop)
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

        public void RedrawPickup(Entities.Property.Property prop)
        {
            DeletePickup(prop);
            DrawPickup(prop);
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

        public Entities.Property.Property Property_GetNearestInRange(Client player, double range)
        {
            Vector3 playerPos = API.shared.getEntityPosition(player);            

            Entities.Property.Property nearestProp = null;

            double nearestDistance = range;

            foreach (var prop in Business.GlobalVariables.Instance.ServerProperties)
            {                
                Vector3 propPos;
                propPos = new Vector3(prop.X, prop.Y, prop.Z);

                float distance = playerPos.DistanceTo(propPos);                

                if (prop.Dimension == player.dimension && distance <= range && distance <= nearestDistance)
                {
                    nearestDistance = distance;
                    nearestProp = prop;
                }
            }
            return nearestProp;
        }

        public void Property_BuyCommand(Client player, Entities.Property.Property prop, bool confirmed)
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

            bool success = bll.TryToBuy(player, prop, confirmed);

            if (success)
            {                
                Property_Save(prop);                
            }
        }        
    }
}