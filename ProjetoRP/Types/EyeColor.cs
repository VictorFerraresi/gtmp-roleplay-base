using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Types
{
    public enum EyeColor
    {
        Black,
        Brown,
        Blue,
        Green
    }

    public static class EyeColorDictionary
    {
        public static Dictionary<EyeColor, string> EyeColorNames = new Dictionary<EyeColor, string>
        {
            { EyeColor.Black, "Preto" },
            { EyeColor.Brown, "Castanho" },
            { EyeColor.Blue, "Azul" },
            { EyeColor.Green, "Verde" },
        };
    }
}
