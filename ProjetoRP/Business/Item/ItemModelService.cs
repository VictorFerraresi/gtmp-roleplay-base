using ProjetoRP.Entities;
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
        protected Entities.Item Item;

        public ItemModelService(DatabaseContext context, Entities.Item item)
        {
            DatabaseContext = context;
            Item = item;
        }

        public Entities.ItemPlacement.Placement GetPlacement()
        {
            return DatabaseContext.ItemsPlacement.Where(ip => ip.Item == Item).Single();
        }

        public Entities.Item GetItem()
        {
            return Item;
        }

        public abstract bool Character_Equippable(Character character, Types.EquipSlot slot);
        public abstract void Character_PostEquipped(Character character, Types.EquipSlot slot);
        public abstract void Character_Activate(Character character);  
    }
}
