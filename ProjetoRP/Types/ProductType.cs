using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Types
{
    public enum ProductType
    {
        Corn = 0,
        Wheat,
        Soy,
        Fruit,
        Meal,
        Milk,
        Wood,
        Gas,
        MetalScrap,
        Vehicle,
        Money,
        Weapon
    }

    public enum ProductClass
    {
        Crate = 0,
        Loose,
        Liquid,
        Log,
        Vehicle,
        SafeBox
    }

    public static class ProductTypeDictionary
    {
        public static Dictionary<ProductType, ProductClass> ProductTypeClasses = new Dictionary<ProductType, ProductClass>
        {
            { ProductType.Corn, ProductClass.Crate},
            { ProductType.Wheat, ProductClass.Crate},
            { ProductType.Soy, ProductClass.Crate},
            { ProductType.Fruit, ProductClass.Crate},
            { ProductType.Meal, ProductClass.Crate},
            { ProductType.Milk, ProductClass.Liquid},
            { ProductType.Wood, ProductClass.Log},
            { ProductType.Gas, ProductClass.Liquid},
            { ProductType.MetalScrap, ProductClass.Loose},
            { ProductType.Vehicle, ProductClass.Vehicle },
            { ProductType.Money, ProductClass.SafeBox},
            { ProductType.Weapon, ProductClass.SafeBox}
        };

        public static Dictionary<ProductType, string> ProductTypeNames = new Dictionary<ProductType, string>
        {
            { ProductType.Corn, "Milho"},
            { ProductType.Wheat, "Trigo"},
            { ProductType.Soy, "Soja"},
            { ProductType.Fruit, "Frutas"},
            { ProductType.Meal, "Lanches"},
            { ProductType.Milk, "Leite"},
            { ProductType.Wood, "Troncos de Madeira"},
            { ProductType.Gas, "Gasolina"},
            { ProductType.MetalScrap, "Ferro Velho"},
            { ProductType.Vehicle, "Veículos" },
            { ProductType.Money, "Cédulas"},
            { ProductType.Weapon, "Armas"}
        };

        public static Dictionary<ProductClass, string> ProductClassNames = new Dictionary<ProductClass, string>
        {
            { ProductClass.Crate, "Caixa"},
            { ProductClass.Liquid, "Líquido"},
            { ProductClass.Log, "Tronco"},
            { ProductClass.Loose, "Carga Solta"},
            { ProductClass.SafeBox, "Caixa Segura"},
            { ProductClass.Vehicle, "Veículo"}
        };
    }
}
