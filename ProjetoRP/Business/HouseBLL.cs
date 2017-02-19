using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkServer;
using GTANetworkShared;

namespace ProjetoRP.Business
{
    public class HouseBLL : Entities.Property.IProperty<Entities.Property.Property>
    {
        public void DrawPickup(Entities.Property.Property house)
        {
            API.shared.createMarker(2, new Vector3(house.X, house.Y, house.Z), new Vector3(), new Vector3(), new Vector3(0.5, 0.5, 0.5), 255, 0, 255, 0, 1);
        }
    }
}