using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fjv.Modules.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class OptionHelpAttribute : HelpAttribute
    {
        public OptionHelpAttribute(string message)
            : base(message)
        { }
    }
}