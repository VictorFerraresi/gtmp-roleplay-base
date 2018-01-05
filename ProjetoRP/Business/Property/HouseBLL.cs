using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using ProjetoRP.Business.Player;

namespace ProjetoRP.Business.Property
{
    public class HouseBLL : Entities.Property.IProperty<Entities.Property.Property>
    {
        PlayerBLL PlayerBLL = new PlayerBLL();

        public void DrawPickup(Entities.Property.Property house)
        {
            Entities.Property.House h = (Entities.Property.House)house;

            house.Pickup = API.shared.createMarker(0, new Vector3(house.X, house.Y, house.Z - 0.25), new Vector3(), new Vector3(), new Vector3(0.5, 0.5, 0.5), 125, 0, 255, 0, h.Dimension);
            if(h.Owner == null)
            {
                house.TextLabel = API.shared.createTextLabel(house.Address + "\n$" + house.Price.ToString("N0"), new Vector3(house.X, house.Y, house.Z + 0.5), 20.0f, 0.5f, false, h.Dimension);
                API.shared.setTextLabelColor(house.TextLabel, 46, 184, 0, 255);
            }
            else
            {
                house.TextLabel = API.shared.createTextLabel(house.Address, new Vector3(house.X, house.Y, house.Z + 0.5), 20.0f, 0.5f, false, h.Dimension);
                API.shared.setTextLabelColor(house.TextLabel, 173, 209, 221, 255);
            }            
        }

        public bool TryToBuy(Client player, Entities.Property.Property house, bool confirmed)
        {
            Entities.Property.House h = (Entities.Property.House)house;
            Entities.Character c = ActivePlayer.GetSpawned(player).Character;            

            if (h.Owner != null)
            {
                API.shared.sendChatMessageToPlayer(player, "Esta propriedade não está a venda!");
                return false;
            }

            if (h.Price > c.Cash)
            {
                API.shared.sendChatMessageToPlayer(player, "Você não possui dinheiro suficiente para comprar esta propriedade!");
                return false;
            }

            if (confirmed)
            {
                if (h.Pickup != null)
                {
                    API.shared.deleteEntity(h.Pickup);
                    h.Pickup = null;
                }
                if (h.TextLabel != null)
                {
                    API.shared.deleteEntity(h.TextLabel);
                    h.TextLabel = null;
                }

                PlayerBLL.Player_TakeMoney(c, h.Price);
                h.Owner = c;
                h.Owner_Id = c.Id;
                DrawPickup(h);
                API.shared.sendChatMessageToPlayer(player, "Você adquiriu esta propriedade com sucesso!");
                return true;              
            }
            else
            {                    
                API.shared.triggerClientEvent(player, "SC_SHOW_BUY_PROP_CONFIRM_MENU", house.Id, h.Price);
                return false;
            }
        }
    }
}