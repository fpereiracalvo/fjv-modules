using System;
using System.Linq;
using Fjv.Modules.Generic;

namespace Fjv.Modules.DependencyInjection
{
    public class GenericModuleFactory<TInput, TOutput> : ModuleFactory<TInput, TOutput>, IModuleFactory<TInput, TOutput>
    {
        IServiceProvider _service;

        public GenericModuleFactory(IServiceProvider service, ModuleFactoryAssemblyConfiguration assemblyConfiguration)
            : base(assemblyConfiguration.Assembly, assemblyConfiguration.Options)
        {
            _service = service;
        }

        public GenericModuleFactory(IServiceProvider service, ModuleFactoryAssembliesConfiguration assembliesConfiguration)
            : base(assembliesConfiguration.Assemblies, assembliesConfiguration.Options)
        {
            _service = service;
        }

        public GenericModuleFactory(IServiceProvider service, ModuleFactoryNamespaceScopedConfiguration typeScopedConfiguration)
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