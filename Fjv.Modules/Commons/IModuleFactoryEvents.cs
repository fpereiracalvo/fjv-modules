using System;

namespace Fjv.Modules.Commons
{
    public interface IModuleFactoryEvents
    {
        event EventHandler<ModuleEventArgument> OnModuleExecuting;
        event EventHandler<ModuleEventArgument> OnModuleExecuted;
        event EventHandler<OptionEventArgument> OnOptionExecuting;
        event EventHandler<OptionEventArgument> OnOptionExecuted;
        event EventHandler<ModuleExceptionEventArgument> OnError;
    }
}