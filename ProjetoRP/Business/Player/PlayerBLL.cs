using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using GTANetworkServer;
using GTANetworkShared;

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