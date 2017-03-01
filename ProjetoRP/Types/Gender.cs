using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Types
{
    public enum Gender
    {
        Male,
        Female
    }

    public static class GenderDictionary
    {
        public static Dictionary<Gender, string> GenderNames = new Dictionary<Gender, string>
        {
            { Gender.Male, "Homem" },
            { Gender.Male, "Mulher" },
        };
    }
}
