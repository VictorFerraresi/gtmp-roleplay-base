using GrandTheftMultiplayer.Server.API;
using ProjetoRP.Business.Item;
using ProjetoRP.Business.Player;
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

        [Command("inventario")]
        public void Command_Inventory(Client sender)
        {
            var player = ActivePlayer.GetSpawned(sender);    

            if(player != null)
            {
                using (var context = new DatabaseContext())
                {
                    var service = new ItemService(context);
                    var items = service.GetItemsFromPlayer(player.Character);

                    // int pageCount = (records + recordsPerPage - 1) / recordsPerPage;
                    const int items_per_line = 3;
                    var lines = (items.Count + items_per_line - 1) / items_per_line;

                    for(var i = 0; i < lines; i++)
                    {
                        var line = "";
                        for (var j = i * items_per_line; j < ((i + 1) * items_per_line); j++)
                        {
                            var item = items[j];
                            var item_service = service.GetItemModelServiceForItem(item.Item2);

                            line += item_service.ItemName;

                            var count = item_service.GetChildren().Count;
                            if (count > 0)
                            {
                                line += " ~b~(" + count + ")~w~";
                            }

                            if((j % items_per_line) != (items_per_line - 1))
                            {
                                line += " - ";
                            }
                        }
                        sender.sendChatMessage(line);
                    }
                }
            }
        }
    }
}
