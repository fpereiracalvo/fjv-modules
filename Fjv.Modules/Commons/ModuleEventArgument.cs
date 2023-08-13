using System;

namespace Fjv.Modules.Commons
{
    public class ModuleEventArgument : EventArgs
    {
        readonly public ModuleItem Module { get; }

        public ModuleEventArgument(ModuleItem module)
        {
            this.Module = module;
        }
    }
}