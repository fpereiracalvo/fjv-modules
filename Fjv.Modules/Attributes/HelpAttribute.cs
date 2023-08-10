using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fjv.Modules.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class HelpAttribute : System.Attribute
    {
        string _message;
        public string Message => _message;

        public HelpAttribute(string message)
        {
            _message = message;
        }
    }
}