using GrandTheftMultiplayer.Shared.Math;
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
            DatabaseContext.Items.Add(item);
            DatabaseContext.SaveChanges();

            var ims = GetItemModelServiceForItem(item);
            ims.Character_InventoryEquip(character, slot);
        }

        public void AddNewItemToGround(Entities.Item item, Vector3 position, int dimension)
        {
            DatabaseContext.Items.Add(item);
            DatabaseContext.SaveChanges();

            var ims = GetItemModelServiceForItem(item);
            ims.World_Drop(position.X, position.Y, position.Z, dimension);
        }

        public void AddNewItemToContainer(Entities.Item item, Entities.ItemModel.Container container, int slot)
        {
            DatabaseContext.Items.Add(item);
            DatabaseContext.SaveChanges();

            var ims = GetItemModelServiceForItem(item);
            ims.Container_Place(container, slot);
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

        public ItemModelService GetItemModelServiceForItem(Entities.Item item)
        {
            return GetItemModelServiceForItem(DatabaseContext, item);
        }
    }
}
