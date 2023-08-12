using System;

namespace Fjv.Modules.Commons
{
    public class ModuleEventArgument : EventArgs
    {
        public ModuleItem Module { get; }

        public ModuleEventArgument(ModuleItem module)
        {
            this.Module = module;
        }
    }
}