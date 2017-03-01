using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Exceptions.Item
{
    class InvalidItemOperationException : Exception
    {
        public InvalidItemOperationException()
        {
        }

        public InvalidItemOperationException(string message)
            : base(message)
        {
        }

        public InvalidItemOperationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
