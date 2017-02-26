using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using GTANetworkServer;
using GTANetworkShared;

namespace ProjetoRP.Business
{
    public class PlayerBLL
    {               
        public void Player_GiveMoney(Entities.Character c, int amount)
        {
            c.Bank += amount;
        }

        public void Player_TakeMoney(Entities.Character c, int amount)
        {
            c.Bank -= amount;
        }
    }
}