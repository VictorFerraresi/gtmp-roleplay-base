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
                return new string[] { "school-backpack", "traveller-backpack", "assault-backpack" };
            }
        }

        public override bool IsEquippable
        {
            get
            {
                return false;
            }
        }

        public override bool IsActivatable
        {
            get
            {
                return false;
            }
        }

        public override bool IsDroppable
        {
            get
            {
                return true;
            }
        }

        public ContainerService(DatabaseContext context, Medkit item) : base(context, item)
        {
        }

        public override void Character_Activate(Character character)
        {
            throw new Exceptions.Item.InvalidItemOperationException(Messages.cant_equip);
        }

        public override void Character_PostEquipped(Character character, EquipSlot slot)
        {
            throw new Exceptions.Item.InvalidItemOperationException(Messages.cant_equip);
        }

        public override void Character_InventoryEquip(Character character)
        {
            throw new Exceptions.Item.InvalidItemOperationException(Messages.cant_equip);
        }
    }
}
