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

        public override bool IsEquippable { get {
                return false;
        } }

        public override bool IsActivatable { get {
                return true;
        } }

        public override bool IsDroppable { get {
                return true;
        } }

        public override string ItemName { get {
                switch (Item.Variation)
                {
                    case "aspirin":
                        return "Aspirina";
                    case "adrenaline-shot":
                        return "Injeção de Adrenalina";
                    default:
                        return Messages.unnamed_item;
                }
        } }

        public MedkitService(DatabaseContext context, Medkit item) : base(context, item)
        {
        }

        public override void Character_Activate(Entities.Character character)
        {
            Validate();
            Consume();

            // Give player health based on variation
        }

        public override void Character_PostEquipped(Character character, EquipSlot slot)
        {
            throw new Exceptions.Item.InvalidItemOperationException(Messages.cant_equip);
        }
    }
}