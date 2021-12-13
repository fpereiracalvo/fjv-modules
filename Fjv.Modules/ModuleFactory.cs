using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fjv.Modules.Commons;
using Fjv.Modules.Extensions;

namespace Fjv.Modules
{
    public class ModuleFactory
    {
        List<Type> _modules;
        
        public ModuleFactory(Assembly assembly)
        {
            _modules = new List<Type>();

            var moduleTypes = assembly.GetModuleTypes();

            _modules = moduleTypes.Select(s=>s).ToList();
        }

        public ModuleFactory(Assembly[] assemblies)
        {
            _modules = new List<Type>();

            foreach (var assembly in assemblies)
            {
                var moduleTypes = assembly.GetModuleTypes();

                _modules = moduleTypes.Select(s=>s).ToList();
            }
        }

        public virtual byte[] Run(string[] args, byte[] buffer = null)
        {
            var modules = this.GetModulesItems(args);

            foreach (var item in modules)
            {
                if(item.Module.IsControlTaker)
                {
                    return Run(item, this, buffer);
                }

                buffer = Run(item, this, buffer);
            }

            return buffer;
        }

        private byte[] Run(ModuleItem module, ModuleFactory moduleFactory, byte[] input)
        {
            byte[] result = null;

            if(module.Module.NeedArgument)
            {
                result = module.Module.Load(input, module.ModuleArgument, module.GlobalArguments, module.IndexArgument);
            }
            else
            {   
                result = module.Module.Load(input ?? module.ModuleArgument, module.GlobalArguments, module.IndexArgument);
            }

            foreach (var option in module.Options)
            {
                result = (byte[])moduleFactory.Invoke(module.Module, option.Name, option.Arguments);
            }

            //todo: dispose module.

            return result;
        }

        public virtual IModule GetModule(string modulename)
        {
            var moduleType = GetModulesQuery(modulename).ToList().SingleOrDefault(s=>s.Name.Equals(modulename))?.Module;

            if(moduleType==null)
            {
                return null;
            }

            var module = (IModule)Activator.CreateInstance(moduleType);

            return module;
        }

        public virtual bool HasModule(string modulename)
        {
            return GetModulesQuery(modulename).ToList().SingleOrDefault(s=>s.Name.Equals(modulename))!=null;
        }

        internal IQueryable<ModuleItemResult> GetModulesQuery(string modulename)
        {
            return _modules.Select(s=> new ModuleItemResult{
                    Module = s,
                    Name = ((Attributes.ModuleAttribute)Attribute.GetCustomAttribute(s, typeof(Attributes.ModuleAttribute))).ModuleName
                }).AsQueryable();
        }

        private string[] GetOptionNames(IModule module)
        {
            return module.GetOptionsMethods().Distinct().ToArray();
        }

        private bool HasOptions(IModule module)
        {
            return module.GetOptionsMethods().Distinct().Count()>0;
        }

        private Type GetOutputType(IModule module, string optionname)
        {
            var method = module.GetMethod(optionname);

            return method.ReturnType;
        }

        private Type[] GetMethodTypesArgument(IModule module, string optionname) {
            return module.GetMethod(optionname).GetParameters().Select(s=>s.ParameterType).ToArray();
        }

        public object Invoke(IModule module, string optionname, params object[] args)
        {
            var method = module.GetMethod(optionname);

            var result = method.Invoke(module, args);

            return result;
        }

        public virtual List<ModuleItem> GetModulesItems(string[] args)
        {
            var modules = new List<ModuleItem>();

            for (var i = 0; i < args.Length; i++)
            {
                var moduleItem = new ModuleItem(){
                    IndexArgument = i,
                    GlobalArguments = args
                };

                var item = args[i];

                moduleItem.Module = this.GetModule(item);

                if(moduleItem.Module == null)
                {
                    moduleItem.Module = this.GetModule("*");
                    moduleItem.ModuleArgument = moduleItem.Module != null ? System.Text.Encoding.UTF8.GetBytes(args[i]) : null;
                }

                if(moduleItem.Module!=null)
                {
                    if(moduleItem.Module.NeedArgument)
                    {
                        var argument = args[i+1];

                        if(this.GetModule(argument)!=null || this.OptionExist(moduleItem.Module, argument))
                        {
                            Console.WriteLine("Argument error!");

                            Environment.Exit(1);
                        }

                        i++;

                        moduleItem.ModuleArgument = System.Text.Encoding.UTF8.GetBytes(argument);
                    }

                    if(this.HasOptions(moduleItem.Module))
                    {
                        var options = this.GetOptionNames(moduleItem.Module);

                        for (var y = i+1; y < args.Length; y++)
                        {
                            if(options.Contains(args[y]))
                            {
                                var optionItem = new OptionItem();

                                optionItem.Name = args[y];
                                var argumentTypes = this.GetMethodTypesArgument(moduleItem.Module, optionItem.Name);

                                y++;
                                i=y;

                                if(argumentTypes.Length > 1)
                                {
                                    var index=0;
                                    var values = args[y].Split(',');
                                    optionItem.Arguments = argumentTypes.Select(s=>Convert.ChangeType(values[index++], s)).ToArray();
                                }
                                else if(argumentTypes.Any())
                                {
                                    optionItem.Arguments = new object[]{ Convert.ChangeType(args[y], argumentTypes.SingleOrDefault()) };
                                }

                                moduleItem.Options.Add(optionItem);

                                if(args.Length<y && this.HasModule(args[y]))
                                {
                                    i--;
                                    
                                    break;
                                }
                            }
                            else
                            {
                                i = y-1;
                                break;
                            }
                        }
                    }

                    modules.Add(moduleItem);
                }
            }

            return modules.Where(m=>m.Module.IsInput)
                .Concat(modules.Where(m=>!m.Module.IsOutput&&!m.Module.IsInput))
                .Concat(modules.Where(m=>m.Module.IsOutput)).ToList();
        }

        private bool OptionExist(IModule module, string option)
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