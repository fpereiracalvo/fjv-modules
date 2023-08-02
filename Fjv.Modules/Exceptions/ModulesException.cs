using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fjv.Modules.Exceptions
{
    public class ModulesException : Exception
    {
        public ModulesException()
            : base ()
        { }

        public ModulesException(string message)
            : base (message)
        { }

        public ModulesException(string message, Exception innerException)
            : base (message, innerException)
        { }
    }
}