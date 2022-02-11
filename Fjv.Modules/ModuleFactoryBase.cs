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
        List<Type> _modules;
        
        public ModuleFactoryBase(Assembly assembly)
        {
            _modules = new List<Type>();

            var moduleTypes = assembly.GetModuleTypes();

            _modules = moduleTypes.Select(s=>s).ToList();
        }

        public ModuleFactoryBase(Assembly[] assemblies)
        {
            _modules = new List<Type>();

            foreach (var assembly in assemblies)
            {
                var moduleTypes = assembly.GetModuleTypes();

                _modules = moduleTypes.Select(s=>s).ToList();
            }
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
            return _modules.Select(s=> new ModuleItemResult{
                    Module = s,
                    Name = ((Attributes.ModuleAttribute)Attribute.GetCustomAttribute(s, typeof(Attributes.ModuleAttribute))).ModuleName
                }).AsQueryable();
        }

        public virtual string[] GetOptionNames(IModule module)
        {
            return module.GetOptionsMethods().Distinct().ToArray();
        }

        public virtual bool HasOptions(IModule module)
        {
            return module.GetOptionsMethods().Distinct().Count()>0;
        }

        public virtual Type GetOutputType(IModule module, string optionname)
        {
            var method = module.GetMethod(optionname);

            return method.ReturnType;
        }

        public virtual Type[] GetMethodTypesArgument(IModule module, string optionname) {
            return module.GetMethod(optionname).GetParameters().Select(s=>s.ParameterType).ToArray();
        }

        public virtual object Invoke(IModule module, string optionname, params object[] args)
        {
            var method = module.GetMethod(optionname);

            var result = method.Invoke(module, args);

            return result;
        }

        public virtual bool OptionExist(IModule module, string option)
        {
            if(!this.HasOptions(module))
            {
                return false;
            }

            var options = this.GetOptionNames(module);

            return options.Contains(option);
        }
    }
}