using System;
using Fjv.Modules.Commons;

namespace Fjv.Modules
{
    public partial class ModuleFactory : ModuleFactoryBase, IModuleFactory
    {
        public event EventHandler<ModuleEventArgument> OnModuleExecuting;
        public event EventHandler<ModuleEventArgument> OnModuleExecuted;
        public event EventHandler<OptionEventArgument> OnOptionExecuting;
        public event EventHandler<OptionEventArgument> OnOptionExecuted;
    }
}