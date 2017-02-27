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
        public MedkitService(DatabaseContext context, Medkit item) : base(context, item)
        {
        }

        public override void Character_Activate(Character character)
        {
            throw new NotImplementedException();
        }

        public override bool Character_Equippable(Character character, EquipSlot slot)
        {
            throw new NotImplementedException();
        }

        public override void Character_PostEquipped(Character character, EquipSlot slot)
        {
            throw new NotImplementedException();
        }
    }
}
