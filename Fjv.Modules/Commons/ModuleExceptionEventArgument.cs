using System;

namespace Fjv.Modules.Commons
{
    public class ModuleExceptionEventArgument : ModuleEventArgument
    {
        public Exception Exception { get; }

        public ModuleExceptionEventArgument(ModuleItem module, Exception exception)
            : base(module)
        {
            this.Exception = exception;
        }
    }
}