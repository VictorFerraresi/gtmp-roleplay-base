using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Modules.Player
{
    namespace Types
    {
        public enum PlayerStatus
        {
            PreLoad = 0,
            Login,
            CharacterSelection,
            AccountOptions,
            Spawned,
            AdminDuty
        }
    }
}
