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
            API.consoleOutput("== ITEM.CS ==");

            DatabaseContext dc = new DatabaseContext();
            ItemService ts = new ItemService(dc);

            Character c = dc.Characters.First();

            try
            {
                ts.AddNewItemToCharacter(new Entities.ItemModel.Medkit(), c, Types.EquipSlot.Legs);
            }
            catch (Exception e)
            {
                API.consoleOutput(e.Message);
            }
            
            foreach(var t in ts.GetItemsFromPlayer(c))
            {
                API.consoleOutput(t.Item1.ToString() + " -> " + t.Item2.ToString());
            }
        }
        public void OnResourceStop()
        {

        }
    }
}
