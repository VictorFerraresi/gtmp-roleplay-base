using ProjetoRP.Entities;
using ProjetoRP.Entities.ItemPlacement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Business.Item
{
    public abstract class ItemModelService
    {
        protected DatabaseContext DatabaseContext;
        public Entities.Item Item { get; private set; }
        public Entities.ItemPlacement.Placement Placement
        {
            get
            {
                return DatabaseContext.ItemsPlacement.Where(ip => ip.Item == Item).Single();
            }
            private set {}
        }

        public virtual string[] ValidVariations { get {
            return new string[] { "default" };
        } }

        public virtual bool IsEquippable { get {
                return false;
        } }

        public virtual bool IsActivatable { get {
                return false;
        } }

        public virtual bool IsDroppable { get {
                return false;
        } }

        public virtual int EquipSlotStack { get {
                return 1;
        } }

        public virtual Types.EquipSlot[] AllowedEquipSlots { get {
                return new Types.EquipSlot[] { };
        } }

        public virtual int ContainerSlotStack { get {
                return 1;
        } }

        public ItemModelService(DatabaseContext context, Entities.Item item)
        {
            DatabaseContext = context;
            Item = item;
        }

        public void World_Drop(double x, double y, double z, int dimension)
        {
            if(!IsDroppable)
            {
                throw new Exceptions.Item.InvalidItemOperationException();
            }

            CleanPlacement();

            DatabaseContext.ItemsPlacement.Add(new Entities.ItemPlacement.Drop()
                { Item_Id = Item.Id, X = x, Y = y, Z = z, Dimension = dimension }
            );

            DatabaseContext.SaveChanges();
        }

        public void Character_InventoryEquip(Character character, Types.EquipSlot slot)
        {
            if (!IsEquippable)
            {
                throw new Exceptions.Item.InvalidItemOperationException();
            }

            if (!AllowedEquipSlots.Contains(slot))
            {
                throw new Exceptions.Item.InvalidItemOperationException();
            }

            var count = DatabaseContext.ItemsPlacement.OfType<CharacterInventoryItem>().Where(ip => ip.Character_Id == character.Id && ip.Slot == slot).Count();

            if (count <= EquipSlotStack)
            {
                CleanPlacement();

                DatabaseContext.ItemsPlacement.Add(new CharacterInventoryItem()
                { Character_Id = character.Id, Item_Id = Item.Id, Slot = slot }
                );

                DatabaseContext.SaveChanges();

                Character_PostEquipped(character, slot);
            }
        }

        public void Container_Place(Entities.ItemModel.Container container, int slot)
        {
            var inception_service = new ItemService(DatabaseContext);
            var container_service = (ContainerService) inception_service.GetItemModelServiceForItem(container);

            if (0 <= slot && slot < container_service.MaxSlots) {
                var current_items = DatabaseContext.ItemsPlacement.OfType<ContainerItem>().Where(ci => ci.ParentItem_Id == container.Id && ci.Slot == slot).ToList();
                if (current_items.Count <= container_service.ContainerSlotStack) {
                    var original_type = GetType().Name; // Get this service's name;
                    var dirty = false;

                    foreach (var current_items_instance in current_items)
                    {
                        var this_type = inception_service.GetItemModelServiceForItem(current_items_instance.Item).GetType().Name;
                        if (!this_type.Equals(original_type))
                        {
                            dirty = true;
                            break;
                        }
                    }

                    if (false == dirty) // Only if the items in a certain slot is homogenus 
                    {
                        CleanPlacement();

                        DatabaseContext.ItemsPlacement.Add(new ContainerItem()
                        { ParentItem_Id = container.Id, Item_Id = Item.Id, Slot = slot }
                        );

                        DatabaseContext.SaveChanges();
                    }
                } // If there are more items than allowed for that one for a single container 
            }
        }

        private void CleanPlacement()
        {
            DatabaseContext.ItemsPlacement.RemoveRange(DatabaseContext.ItemsPlacement.Where(ip => ip.Item.Id == Item.Id));
            DatabaseContext.SaveChanges();
        }

        // Destroy this item (generally called after using)
        protected void Consume()
        {
            DatabaseContext.Items.Remove(Item);
            DatabaseContext.SaveChanges();
            Item = null;
        }

        // Checks if Variation exists for this ItemModel
        protected void Validate()
        {
            if(null == Item)
            {
                throw new Exceptions.Item.InvalidItemModelServiceException(Messages.null_item);
            }

            if(false == ValidVariations.Contains(Item.Variation)) 
            {
                throw new Exceptions.Item.InvalidItemModelServiceException(Messages.invalid_variation);
            }
        }

        public abstract void Character_PostEquipped(Character character, Types.EquipSlot slot);
        public abstract void Character_Activate(Character character);
    }
}
