using System;

namespace Fjv.Modules.Commons
{
    public class OptionEventArgument : EventArgs
    {
        public ModuleItem Module { get; }
        public OptionItem Option { get; }

        public OptionEventArgument(ModuleItem module, OptionItem option)
        {
            this.Module = module;
            this.Option = option;
        }
    }
}