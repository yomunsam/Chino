using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chino.IdentityServer.Exceptions.Common
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException() : base(){ }

        public AlreadyExistsException(string message) : base(message){ }
    }
}
