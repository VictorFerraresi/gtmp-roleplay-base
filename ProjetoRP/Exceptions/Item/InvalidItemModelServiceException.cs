using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Exceptions.Item
{
    class InvalidItemModelServiceException : Exception
    {
        public InvalidItemModelServiceException()
        {
        }

        public InvalidItemModelServiceException(string message)
            : base(message)
        {
        }

        public InvalidItemModelServiceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
