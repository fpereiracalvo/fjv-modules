using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fjv.Modules.Attributes
{
    public class ModuleHelpAttribute : HelpAttribute
    {
        public ModuleHelpAttribute(string message)
            : base(message)
        { }
    }
}