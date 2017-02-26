using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Types
{
    public enum HairColor
    {
        Black,
        Brown,
        Red,
        Blonde,
        Gray
    }

    public static class HairColorDictionary
    {
        public static Dictionary<HairColor, string> HairColorNames = new Dictionary<HairColor, string>
        {
            { HairColor.Black, "Preto" },
            { HairColor.Brown, "Castanho" },
            { HairColor.Red, "Ruivo" },
            { HairColor.Blonde, "Loiro" },
            { HairColor.Gray, "Cinza" },
        };
    }
}
