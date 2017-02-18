using GTANetworkServer;
using GTANetworkShared;
using ProjetoRP.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProjetoRP.Modules.Property
{
    public class Property : Script
    {
        public List<Entities.Property.Property> ServerProperties;

        public Property()
        {
            API.onResourceStart += OnResourceStart;
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        public void OnResourceStart()
        {
            API.consoleOutput(Messages.console_startup);
            ServerProperties = SQL_FetchProperties();
        }

        public void OnClientEventTrigger(Client player, string eventName, object[] args)
        {
            
        }                        


        // SQL Functions
        private Entities.Property.Property SQL_FetchPropertyData(int property_id)
        {
            Entities.Property.Property prop = null;

            using (var context = new DatabaseContext())
            {
                prop = (from p in context.Properties where p.Id == property_id select p).AsNoTracking().Single();
                // AsNoTracking "detaches" the entity from the Context, allowing it to be kept in memory and used as please up until reattached again @Player_Save                                
            }

            return prop;
        }

        private List<Entities.Property.Property> SQL_FetchProperties()
        {
            List<Entities.Property.Property> properties = new List<Entities.Property.Property>();

            using (var context = new DatabaseContext())
            {
                properties = (from p in context.Properties select p).AsNoTracking().ToList();
                // AsNoTracking "detaches" the entity from the Context, allowing it to be kept in memory and used as please up until reattached again @Player_Save                                
            }
            return properties;
        }


        //Commands        
    }
}
