using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fjv.Modules.Commons;

namespace Fjv.Modules
{
    public interface IModuleFactory
    {
        event EventHandler<ModuleEventArgument> OnModuleExecuting;
        event EventHandler<ModuleEventArgument> OnModuleExecuted;
        event EventHandler<OptionEventArgument> OnOptionExecuting;
        event EventHandler<OptionEventArgument> OnOptionExecuted;
        event EventHandler<ModuleExceptionEventArgument> OnError;

        List<ModuleItem> GetModulesItems(string[] args);
        byte[] Run(string[] args, byte[] buffer = null);
        byte[] Run(ModuleItem module, ModuleFactory moduleFactory, byte[] input);
        Task<byte[]> RunAsync(string[] args, byte[] buffer = null);
        Task<byte[]> RunAsync(ModuleItem module, ModuleFactory moduleFactory, byte[] input);
        IModule GetModule(string modulename);
        bool HasModule(string modulename);
        IQueryable<ModuleItemResult> GetModulesAsQueryable();
        IQueryable<OptionItemResult> GetOptionsAsQueriable(IModule module);
        object Invoke(IModule module, string optionname, params object[] args);
        string GetHelp(string[] args);
        string GetHelp();
    }
}