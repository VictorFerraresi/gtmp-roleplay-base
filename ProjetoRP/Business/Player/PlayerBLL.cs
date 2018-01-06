using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;

namespace ProjetoRP.Business.Player
{
    public class PlayerBLL
    {               
        public void Player_GiveMoney(Entities.Character c, int amount)
        {
            c.Cash += amount;
        }

        public void Player_TakeMoney(Entities.Character c, int amount)
        {
            c.Cash -= amount;
        }

        public bool Player_IsInRangeOfPlayer(Client p1, Client p2, float range = 5.0f)
        {
            return API.shared.getEntityPosition(p1).DistanceTo(API.shared.getEntityPosition(p2)) <= range;
        }

        public bool Player_IsInRangeOfPoint(Client p1, Vector3 point, float range = 5.0f)
        {
            return API.shared.getEntityPosition(p1).DistanceTo(point) <= range;
        }

        public void Player_DeleteAme(Client player)
        {            
            TextLabel label = player.getData("AME_LABEL");
            API.shared.deleteEntity(label);
            player.resetData("AME_LABEL");
        }

        /*public int? Player_GetNextFreeId()
        {
            int? a = null;
            for(int i = 0; i < 1000; i++)
            {
                if(API.shared.getAllPlayers().Find(x => x.getData("playerId") == i) != null)
                {
                    continue;
                }
                a = i;
                break;        
            }
            return a;
        }*/
    }
}