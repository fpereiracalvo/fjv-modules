using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fjv.Modules.Commons;
using Fjv.Modules.Extensions;

namespace Fjv.Modules
{
    public abstract class ModuleFactoryBase
    {
        List<Type> _modules = new List<Type>();
        
        public ModuleFactoryBase(Assembly assembly)
        {
            var moduleTypes = assembly.GetModuleTypes();

            _modules = moduleTypes.Select(s=>s).ToList();
        }

        public ModuleFactoryBase(Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var moduleTypes = assembly.GetModuleTypes();

                _modules = moduleTypes.Select(s=>s).ToList();
            }
        }

        public ModuleFactoryBase(Type scopedToNamespace)
        {
            var moduleTypes = scopedToNamespace.GetModuleTypes();

            _modules = moduleTypes.Select(s=>s).ToList();
        }

        public virtual IModule GetModule(string modulename)
        {
            var moduleType = GetModulesAsQueryable().ToList().SingleOrDefault(s=>s.Name.Equals(modulename))?.Module;

            if(moduleType==null)
            {
                return null;
            }

            var module = (IModule)Activator.CreateInstance(moduleType);

            return module;
        }

        public virtual bool HasModule(string modulename)
        {
            return this.GetModulesAsQueryable().ToList().SingleOrDefault(s=>s.Name.Equals(modulename))!=null;
        }

        internal IQueryable<ModuleItemResult> GetModulesAsQueryable()
        {
            return _modules.Select(s=> {
                var attr = ((Attributes.ModuleAttribute)Attribute.GetCustomAttribute(s, typeof(Attributes.ModuleAttribute)));
                
                return new ModuleItemResult{
                    Module = s,
                    Name = attr.ModuleName
                };
            }).AsQueryable();
        }

        internal IQueryable<OptionItemResult> GetOptionsAsQueriable(IModule module)
        {
            return module.GetType().GetMethods()
                .Where(s=>s.GetCustomAttributes(typeof(Attributes.OptionAttribute), false).Any())
                .Select(s=>{
                    var attr = ((Attributes.OptionAttribute)Attribute.GetCustomAttribute(s, typeof(Attributes.OptionAttribute)));
                    var model = new OptionItemResult{
                        ArgumentsTypes = module.GetMethod(attr.OptionName).GetParameters().Select(s=>s.ParameterType).ToArray(),
                        Name = attr.OptionName,
                        SeparatedArguments = attr.SeparatedArgument
                    };
                    
                    return model;
                }).AsQueryable();
        }

        public virtual object Invoke(IModule module, string optionname, params object[] args)
        {
            var method = module.GetMethod(optionname);

            var result = method.Invoke(module, args);

            return result;
        }
    }
}