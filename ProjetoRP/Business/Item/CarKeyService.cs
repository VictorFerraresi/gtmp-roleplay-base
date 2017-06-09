using ProjetoRP.Entities;
using ProjetoRP.Entities.ItemModel;
using ProjetoRP.Types;

namespace ProjetoRP.Business.Item
{
    class CarKeyService : ItemModelService
    {
        public override string[] ValidVariations
        {
            get
            {
                return new string[] { "default" };
            }
        }

        public override bool IsEquippable { get {
                return false;
        } }

        public override bool IsActivatable { get {
                return false;
        } }

        public override bool IsDroppable { get {
                return true;
        } }

        public override string ItemName { get {
                return "Chave";
        } }

        public CarKeyService(DatabaseContext context, CarKey item) : base(context, item)
        {
        }

        public override void Character_Activate(Character character)
        {
            throw new Exceptions.Item.InvalidItemOperationException(Messages.cant_use);
        }

        public override void Character_PostEquipped(Character character, EquipSlot slot)
        {
            throw new Exceptions.Item.InvalidItemOperationException(Messages.cant_equip);
        }
    }
}
