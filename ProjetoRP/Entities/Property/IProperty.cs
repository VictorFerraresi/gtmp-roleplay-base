using GrandTheftMultiplayer.Server.Elements;

namespace ProjetoRP.Entities.Property
{
    public interface IProperty<Property>
    {
        void DrawPickup(Property p);
        bool TryToBuy(Client c, Property p, bool confirmed);
    }
}