using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD
=======
using GTANetworkServer;
using GTANetworkShared;
>>>>>>> master

namespace ProjetoRP.Entities.Property
{
    public interface IProperty<Property>
    {
        void DrawPickup(Property p);
        bool TryToBuy(Client c, Property p, bool confirmed);
    }
}