using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Types
{
    public static class TruckRestrictionsDictionary
    {
        public static Dictionary<string, TruckerRank> TruckMinRank = new Dictionary<string, TruckerRank>
        {
            { "Boxville", TruckerRank.Loader },
            { "Boxville2", TruckerRank.Loader },
            { "Boxville3", TruckerRank.Loader },
            { "Boxville4", TruckerRank.Loader },
            { "Boxville5", TruckerRank.Loader },
            { "Burrito", TruckerRank.Loader },
            { "Burrito2", TruckerRank.Loader },
            { "Burrito3", TruckerRank.Loader },
            { "Burrito4", TruckerRank.Loader },
            { "Burrito5", TruckerRank.Loader },
            { "GBurrito", TruckerRank.Loader },
            { "GBurrito2", TruckerRank.Loader },
            { "Paradise", TruckerRank.Loader },
            { "Pony", TruckerRank.Loader },
            { "Pony2", TruckerRank.Loader },
            { "Rumpo", TruckerRank.Loader },
            { "Rumpo2", TruckerRank.Loader },
            { "Rumpo3", TruckerRank.Loader },
            { "Speedo", TruckerRank.Loader },
            { "Speedo2", TruckerRank.Loader },
            { "Youga", TruckerRank.Loader },
            { "Youga2", TruckerRank.Loader },            
            { "Stockade", TruckerRank.Intermediate },
            { "Stockade3", TruckerRank.Intermediate },
            { "Mule", TruckerRank.Trucker },
            { "Mule2", TruckerRank.Trucker },
            { "Mule3", TruckerRank.Trucker },
            { "Benson", TruckerRank.Expert },
            { "Pounder", TruckerRank.Expert },
            { "Packer", TruckerRank.Paragon },
            { "Phantom", TruckerRank.Paragon },
            { "Hauler", TruckerRank.Paragon }
        };
    }

    public static class TruckSlotsDictionary
    {
        public static Dictionary<string, int> TruckSlotList = new Dictionary<string, int>
        {
            { "Boxville", 0 }, //Todo truck slots
            { "Boxville2", 0 },
            { "Boxville3", 0 },
            { "Boxville4", 0 },
            { "Boxville5", 0 },
            { "Burrito", 0 },
            { "Burrito2", 0 },
            { "Burrito3", 0 },
            { "Burrito4", 0 },
            { "Burrito5", 0 },
            { "GBurrito", 0 },
            { "GBurrito2", 0 },
            { "Paradise", 0 },
            { "Pony", 0 },
            { "Pony2", 0 },
            { "Rumpo", 0 },
            { "Rumpo2", 0 },
            { "Rumpo3", 0 },
            { "Speedo", 0 },
            { "Speedo2", 0 },
            { "Youga", 0 },
            { "Youga2", 0 },
            { "Youga2", 0 },
            { "Stockade", 0 },
            { "Stockade3", 0 },
            { "Mule", 0 },
            { "Mule2", 0 },
            { "Mule3", 0 },
            { "Benson", 0 },
            { "Pounder", 0 },
            { "Packer", 0 },
            { "Phantom", 0 },
            { "Hauler", 0 }
        };
    }
}