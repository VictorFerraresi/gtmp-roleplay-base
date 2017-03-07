using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Exceptions.Player
{
    class NoRemainingCharacterSlotsException : Exception
    {
        public NoRemainingCharacterSlotsException()
        {
        }

        public NoRemainingCharacterSlotsException(string message)
            : base(message)
        {
        }

        public NoRemainingCharacterSlotsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
