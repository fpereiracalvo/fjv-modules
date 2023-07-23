using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Fjv.Modules.Commons;
using Fjv.Modules.Extensions;

namespace Fjv.Modules
{
    public abstract class ModuleFactoryBase
    {
        List<ModuleOptions> _options;
        List<Type> _modules = new List<Type>();

        private ModuleFactoryBase(List<ModuleOptions> options)
        {
            _options = options ?? new List<ModuleOptions>();
        }
        
        public ModuleFactoryBase(Assembly assembly, List<ModuleOptions> options)
            : this(options)
        {
            var moduleTypes = assembly.GetModuleTypes();

            _modules = moduleTypes.Select(s=>s).ToList();
        }

        public ModuleFactoryBase(Assembly[] assemblies, List<ModuleOptions> options)
            : this(options)
        {
            foreach (var assembly in assemblies)
            {
                var moduleTypes = assembly.GetModuleTypes();

                _modules = moduleTypes.Select(s=>s).ToList();
            }
        }

        public ModuleFactoryBase(Type scopedToNamespace, List<ModuleOptions> options)
            : this(options)
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

        public IQueryable<ModuleItemResult> GetModulesAsQueryable()
        {
            return _modules.Select(s=> {
                var attr = ((Attributes.ModuleAttribute)Attribute.GetCustomAttribute(s, typeof(Attributes.ModuleAttribute)));

                var option = _options.FirstOrDefault(x=>x.ModuleType.FullName.Equals(s.FullName)) ?? new ModuleOptions();
                
                return new ModuleItemResult{
                    Module = s,
                    Name = option.GetName(attr.ModuleName)
                };
            }).AsQueryable();
        }

        public IQueryable<OptionItemResult> GetOptionsAsQueriable(IModule module)
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