using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Fjv.Modules.DependencyInjection
{
    public class ModuleFactory : Fjv.Modules.ModuleFactory, IModuleFactory
    {
        IServiceProvider _service;

        public ModuleFactory(IServiceProvider service, ModuleFactoryAssemblyConfiguration assemblyConfiguration)
            : base(assemblyConfiguration.Assembly, assemblyConfiguration.Options)
        { 
            _service = service;
        }

        public ModuleFactory(IServiceProvider service, ModuleFactoryAssembliesConfiguration assembliesConfiguration)
            : base(assembliesConfiguration.Assemblies, assembliesConfiguration.Options)
        {
            _service = service;
        }

        public ModuleFactory(IServiceProvider service, ModuleFactoryNamespaceScopedConfiguration typeScopedConfiguration)
            : base(typeScopedConfiguration.ScopedNamespaceType, typeScopedConfiguration.Options)
        { 
            _service = service;
        }

        public override IModule GetModule(string modulename)
        {
            var moduleType = GetModulesAsQueryable().ToList().SingleOrDefault(s=>s.Name.Equals(modulename))?.Module;

            if(moduleType==null)
            {
                return null;
            }

            return (IModule)_service.GetService(moduleType);
        }
    }
}