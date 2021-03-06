using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using ProjetoRP.Business.Player;

namespace ProjetoRP.Business.Property
{
    public class BusinessBLL : Entities.Property.IProperty<Entities.Property.Property>
    {        
        PlayerBLL PlayerBLL = new PlayerBLL();

        public void DrawPickup(Entities.Property.Property business)
        {
            Entities.Property.Business b = (Entities.Property.Business)business;

            business.Pickup = API.shared.createMarker(0, new Vector3(business.X, business.Y, business.Z - 0.25), new Vector3(), new Vector3(), new Vector3(0.5, 0.5, 0.5), 125, 0, 0, 255, b.Dimension);

            string label = string.Format("~w~Empresa\n[~b~{0}~w~]", b.Name);
            if (b.Owner == null)
            {                
                business.TextLabel = API.shared.createTextLabel(label + "\n~g~$" + business.Price.ToString("N0"), new Vector3(business.X, business.Y, business.Z + 0.5), 20.0f, 0.5f, false, b.Dimension);
                API.shared.setTextLabelColor(business.TextLabel, 255, 255, 255, 255);
            }
            else
            {                
                business.TextLabel = API.shared.createTextLabel(label, new Vector3(business.X, business.Y, business.Z + 0.5), 20.0f, 0.5f, false, b.Dimension);
                API.shared.setTextLabelColor(business.TextLabel, 173, 209, 221, 255);
            }            
        }

        public bool TryToBuy(Client player, Entities.Property.Property business, bool confirmed)
        {
            Entities.Property.Business b = (Entities.Property.Business)business;
            Entities.Character c = ActivePlayer.GetSpawned(player).Character;

            if (b.Owner != null)
            {
                API.shared.sendChatMessageToPlayer(player, "Esta propriedade não está a venda!");
                return false;
            }

            if (b.Price > c.Cash)
            {
                API.shared.sendChatMessageToPlayer(player, "Você não possui dinheiro suficiente para comprar esta propriedade!");
                return false;
            }

            if (confirmed)
            {
                if (b.Pickup != null)
                {
                    API.shared.deleteEntity(b.Pickup);
                    b.Pickup = null;
                }
                if (b.TextLabel != null)
                {
                    API.shared.deleteEntity(b.TextLabel);
                    b.TextLabel = null;
                }

                PlayerBLL.Player_TakeMoney(c, b.Price);                
                b.Owner = c;
                b.Owner_Id = c.Id;               
                DrawPickup(b);
                API.shared.sendChatMessageToPlayer(player, "Você adquiriu esta propriedade com sucesso!");
                return true;
            }
            else
            {
                API.shared.triggerClientEvent(player, "SC_SHOW_BUY_PROP_CONFIRM_MENU", business.Id, b.Price);
                return false;
            }
        }
    }
}