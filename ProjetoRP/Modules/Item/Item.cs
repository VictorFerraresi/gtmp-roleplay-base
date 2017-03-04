using GTANetworkServer;
using ProjetoRP.Business.Item;
using ProjetoRP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Modules.Item
{
    public class Item : Script
    {
        public Item()
        {
            API.onResourceStart += OnResourceStart;
            API.onResourceStop += OnResourceStop;
        }

        public void OnResourceStart()
        {
            
        }
        public void OnResourceStop()
        {

        }
    }
}
