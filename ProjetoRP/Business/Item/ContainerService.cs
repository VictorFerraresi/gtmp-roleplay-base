using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetoRP.Entities;
using ProjetoRP.Entities.ItemModel;
using ProjetoRP.Types;

namespace ProjetoRP.Business.Item
{
    class ContainerService : ItemModelService
    {
        public override string[] ValidVariations
        {
            get
            {
                return new string[] { "school-backpack", "traveler-backpack", "assault-backpack", "snackpack-toreality" };
            }
        }

        public override bool IsEquippable { get {
                return true;
        } }

        public override bool IsActivatable { get {
                return false;
        } }

        public override bool IsDroppable { get {
                return true;
        } }

        public override EquipSlot[] AllowedEquipSlots { get {
                return new EquipSlot[] { EquipSlot.Back };
        } }

        public int MaxSlots { get {
                switch(Item.Variation)
                {
                    case "school-backpack":
                        return 10;
                    case "traveler-backpack":
                        return 15;
                    case "assault-backpack":
                        return 20;
                    case "snackpack-toreality":
                        return 50;
                    default:
                        return 0;
                }
        } }

        public ContainerService(DatabaseContext context, Container item) : base(context, item)
        {
        }

        public override void Character_Activate(Character character)
        {
            throw new Exceptions.Item.InvalidItemOperationException(Messages.cant_equip);
        }

        public override void Character_PostEquipped(Character character, EquipSlot slot)
        {
            // throw new Exceptions.Item.InvalidItemOperationException(Messages.cant_equip);
        }

        public override bool Character_Equippable(Entities.Character character, Types.EquipSlot slot) //recheck
        {
            return false;
        }

        public override void Character_InventoryEquip(Entities.Character character) //recheck
        {
        }
    }
}