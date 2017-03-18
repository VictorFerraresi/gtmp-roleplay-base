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

            /*
            using (var context = new DatabaseContext())
            {
                var service = new ItemService(context);

                var character = context.Characters.First();

                var key = new Entities.ItemModel.Container()
                {
                   Placement = null, // Will be assigned by service function
                   Variation = "school-backpack"
                };

                service.AddNewItemToCharacter(key, character, Types.EquipSlot.Back);

                var data = service.GetCascadingItemsFromPlayer(character);
                foreach (var item in data)
                {
                    Console.WriteLine(" - " + item.Id + " - " + item.GetType().Name);
                }
            }
            */
            
        }
        public void OnResourceStop()
        {

        }
    }
}
