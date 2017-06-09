using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Types
{
    public enum TruckerRank
    {
        Loader = 1,
        Rookie,
        Intermediate,
        Trucker,
        Expert,
        Paragon
    }

    public static class TruckerRankDictionary
    {
        public static Dictionary<TruckerRank, string> TruckerRankNames = new Dictionary<TruckerRank, string>
        {
            { TruckerRank.Loader, "Carregador de Caminhão" },
            { TruckerRank.Rookie, "Caminhoneiro Iniciante" },
            { TruckerRank.Intermediate, "Caminhoneiro" },
            { TruckerRank.Trucker, "Caminhoneiro Intermediário" },
            { TruckerRank.Expert, "Caminhoneiro Experiente" },
            { TruckerRank.Paragon, "Caminhoneiro Veterano" }
        };
    }
}