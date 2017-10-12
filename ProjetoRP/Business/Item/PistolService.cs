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
    class PistolService : ItemModelService
    {
        public override string[] ValidVariations { get {
            return new string[] { "pt92", "p2000" };
        } }

        public override bool IsEquippable { get {
            return true;
        } }

        public override bool IsActivatable { get {
            return false;
        } }

        public override bool IsDroppable { get {
            return true;
        } }

        public override string ItemName { get {
                switch (Item.Variation)
                {
                    case "pt92":
                        return "PT92";
                    case "p2000":
                        return "P2000";
                    default:
                        return Messages.unnamed_item;
                }
         } }

        public override EquipSlot[] AllowedEquipSlots { get {
            return new EquipSlot[] { EquipSlot.SecondaryWeaponWaist };
        } }

        public PistolService(DatabaseContext context, Pistol item) : base(context, item) { }

        public override void Character_Activate(Character character)
        {
            throw new Exceptions.Item.InvalidItemOperationException(Messages.cant_use);
        }

        public override void Character_PostEquipped(Character character, EquipSlot slot)
        {
            // throw new Exceptions.Item.InvalidItemOperationException(Messages.cant_equip);
        }
    }
}
