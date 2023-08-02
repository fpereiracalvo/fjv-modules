using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fjv.Modules.Commons;

namespace Fjv.Modules
{
    public interface IModuleFactory
    {
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
    }
}