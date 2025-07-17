using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fjv.Modules.Commons
{
    public interface IModuleFactoryCommons
    {
        List<ModuleItem> GetModulesItems(string[] args);
        IModule GetModule(string modulename);
        bool HasModule(string modulename);
        IQueryable<ModuleItemResult> GetModulesAsQueryable();
        IQueryable<OptionItemResult> GetOptionsAsQueriable(IModule module);
        object Invoke(IModule module, string optionname, params object[] args);
        string GetHelp(string[] args);
        string GetHelp();
    }
}