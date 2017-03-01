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
    class MedkitService : ItemModelService
    {
        public override string[] ValidVariations
        {
            get
            {
                return new string[] { "aspirin", "adrenaline-shot" };
            }
        }

        public MedkitService(DatabaseContext context, Medkit item) : base(context, item)
        {
        }

        public override void Character_Activate(Character character)
        {
            Consume();
        }

        public override bool Character_Equippable(Character character, EquipSlot slot)
        {
            return false;
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
