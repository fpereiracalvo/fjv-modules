using System;

namespace Fjv.Modules.Commons
{
    public class OptionEventArgument : EventArgs
    {
        readonly public ModuleItem Module { get; }
        readonly public OptionItem Option { get; }

        public OptionEventArgument(ModuleItem module, OptionItem option)
        {
            this.Module = module;
            this.Option = option;
        }
    }
}