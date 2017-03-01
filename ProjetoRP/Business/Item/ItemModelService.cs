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

        public virtual string[] ValidVariations { get {
            return new string[] { "default" };
        } }

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

        public void DropOnTheGround(double x, double y, double z, int dimension)
        {
            CleanPlacement();

            DatabaseContext.ItemsPlacement.Add(new Entities.ItemPlacement.Drop()
                { Item = Item, X = x, Y = y, Z = z, Dimension = dimension }
            );

            DatabaseContext.SaveChanges();
        }

        protected void CleanPlacement()
        {
            DatabaseContext.ItemsPlacement.RemoveRange(DatabaseContext.ItemsPlacement.Where(ip => ip.Item == Item));
            DatabaseContext.SaveChanges();
        }

        protected void Consume()
        {
            DatabaseContext.Items.Remove(Item);
            DatabaseContext.SaveChanges();
        }

        public abstract bool Character_Equippable(Character character, Types.EquipSlot slot);
        public abstract void Character_InventoryEquip(Character character);
        public abstract void Character_PostEquipped(Character character, Types.EquipSlot slot);
        public abstract void Character_Activate(Character character);  
    }
}
