using ProjetoRP.Entities.ItemPlacement;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Business.Item
{
    public class ItemService
    {
        private DatabaseContext DatabaseContext;

        public ItemService(DatabaseContext DbContext)
        {
            DatabaseContext = DbContext;
        }

        public void AddNewItemToCharacter(Entities.Item item, Entities.Character character, Types.EquipSlot slot)
        {
            // TODO: This wont work and possibly will spawn placement-less items on DB
            var current_item_in_slot = DatabaseContext.ItemsPlacement.OfType<CharacterInventoryItem>().Where(ip => ip.Character_Id == character.Id && ip.Slot == slot).Count();

            if(current_item_in_slot > 0)
            {
                throw new Exceptions.Item.InvalidItemOperationException("There is already an item in the specified slot.");
            }

            DatabaseContext.Items.Add(item);
            try
            {
                DatabaseContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            var ims = GetItemModelServiceForItem(item);
            ims.Character_InventoryEquip(character, slot);
        }

        public List<Tuple<Types.EquipSlot, Entities.Item>> GetItemsFromPlayer(Entities.Character character)
        {
            var requested = new List<Tuple<Types.EquipSlot, Entities.Item>>();

            var player_placements = DatabaseContext.ItemsPlacement.OfType<CharacterInventoryItem>().Where(ip => ip.Character_Id == character.Id).ToList();

            foreach(var placement in player_placements)
            {
                requested.Add(new Tuple<Types.EquipSlot, Entities.Item>(placement.Slot, placement.Item));
            }

            return requested;
        }

        public List<Entities.Item> GetCascadingItemsFromPlayer(Entities.Character character)
        {
            var requested = new List<Entities.Item>();

            var player_placements = DatabaseContext.ItemsPlacement.AsNoTracking().OfType<CharacterInventoryItem>().Where(ip => ip.Character_Id == character.Id).ToList();

            foreach (var placement in player_placements)
            {
                requested.Add(placement.Item);

                var ims = GetItemModelServiceForItem(placement.Item);
                if (ims is ContainerService)
                {
                    var container_placements = DatabaseContext.ItemsPlacement.AsNoTracking().OfType<ContainerItem>().Where(ip2 => ip2.ParentItem_Id == ims.Item.Id).ToList();

                    foreach (var container_placement in container_placements)
                    {
                        requested.Add(container_placement.Item);
                    }
                }
            }

            return requested;
        }

        /*public List<Tuple<Types.EquipSlot, ItemModelService>> GetItemsFromPlayer(Entities.Character character)
        {
            var bare_items = GetBareItemsFromPlayer(character);
            var requested = new List<Tuple<Types.EquipSlot, ItemModelService>>();

            foreach (var t in bare_items)
            {
                requested.Add(new Tuple<Types.EquipSlot, ItemModelService>(t.Item1, GetItemModelServiceForItem(t.Item2)));
            }

            return requested;
        }*/

        public ItemModelService GetItemModelServiceForItem(DatabaseContext context, Entities.Item item)
        {
            var itemType = item.GetType();

            if (itemType.Namespace == "System.Data.Entity.DynamicProxies")
                itemType = itemType.BaseType;

            Type serviceType;
            try
            {
                var fullTypeName = "ProjetoRP.Business.Item." + itemType.Name + "Service";
                serviceType = Type.GetType(fullTypeName);
                
            }
            catch (Exception e)
            {
                throw new Exceptions.Item.InvalidItemModelServiceException();
            }

            return (ItemModelService)Activator.CreateInstance(serviceType, new object[] { context, item });
        }

        private ItemModelService GetItemModelServiceForItem(Entities.Item item)
        {
            return GetItemModelServiceForItem(DatabaseContext, item);
        }
    }
}
