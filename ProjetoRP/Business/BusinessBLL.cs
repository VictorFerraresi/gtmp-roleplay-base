using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkServer;
using GTANetworkShared;

namespace ProjetoRP.Business
{
    public class BusinessBLL : Entities.Property.IProperty<Entities.Property.Property>
    {
        public void DrawPickup(Entities.Property.Property business)
        {
            business.Pickup = API.shared.createMarker(2, new Vector3(business.X, business.Y, business.Z), new Vector3(), new Vector3(), new Vector3(0.5, 0.5, 0.5), 255, 0, 0, 255, 1);
            business.TextLabel = API.shared.createTextLabel(business.Address, new Vector3(business.X, business.Y, business.Z + 0.5), 20.0f, 0.5f, false);
            //API.shared.setTextLabelColor(business.TextLabel, 0, 0, 0, 0);
        }
    }
}