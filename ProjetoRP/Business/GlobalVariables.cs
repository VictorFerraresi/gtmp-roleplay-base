using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Business
{
    public class GlobalVariables
    {
        private static GlobalVariables instance;
        public List<Entities.Property.Property> ServerProperties;

        private GlobalVariables() { }

        public static GlobalVariables Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GlobalVariables();
                }
                return instance;
            }
        }
    }
}
