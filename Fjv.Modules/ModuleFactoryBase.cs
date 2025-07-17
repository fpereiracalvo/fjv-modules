using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Fjv.Modules.Commons;
using Fjv.Modules.Exceptions;
using Fjv.Modules.Extensions;

namespace Fjv.Modules
{
    public abstract class ModuleFactoryBase
    {
        readonly string _wildcard = "*";
        
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

            _modules = moduleTypes.Select(s => s).ToList();
        }

        public ModuleFactoryBase(Assembly[] assemblies, List<ModuleOptions> options)
            : this(options)
        {
            foreach (var assembly in assemblies)
            {
                var moduleTypes = assembly.GetModuleTypes();

                _modules = moduleTypes.Select(s => s).ToList();
            }
        }

        public ModuleFactoryBase(Type scopedToNamespace, List<ModuleOptions> options)
            : this(options)
        {
            var moduleTypes = scopedToNamespace.GetModuleTypes();

            _modules = moduleTypes.Select(s => s).ToList();
        }

        public virtual IModule GetModule(string modulename)
        {
            var moduleType = GetModelItemResult(modulename)?.Module;

            if (moduleType == null)
            {
                return null;
            }

            var module = (IModule)Activator.CreateInstance(moduleType);

            return module;
        }

        public virtual bool HasModule(string modulename)
        {
            return this.GetModulesAsQueryable().ToList().SingleOrDefault(s => s.Name.Equals(modulename)) != null;
        }

        public IQueryable<ModuleItemResult> GetModulesAsQueryable()
        {
            return _modules.Select(s =>
            {
                var attr = ((Attributes.ModuleAttribute)Attribute.GetCustomAttribute(s, typeof(Attributes.ModuleAttribute)));
                var message = ((Attributes.ModuleHelpAttribute)Attribute.GetCustomAttribute(s, typeof(Attributes.ModuleHelpAttribute)))?.Message ?? string.Empty;

                var option = _options.FirstOrDefault(x => x.ModuleType.FullName.Equals(s.FullName)) ?? new ModuleOptions();

                return new ModuleItemResult
                {
                    Module = s,
                    Name = option.GetName(attr.ModuleName),
                    Message = message
                };
            }).AsQueryable();
        }

        public ModuleItemResult GetModelItemResult(string modulename)
        {
            return this.GetModulesAsQueryable().ToList().SingleOrDefault(s => s.Name.Equals(modulename));
        }

        public IQueryable<OptionItemResult> GetOptionsAsQueriable(IModule module)
        {
            return GetOptionsAsQueriable(module.GetType());
        }

        public IQueryable<OptionItemResult> GetOptionsAsQueriable(Type module)
        {
            return module.GetMethods()
                .Where(s => s.GetCustomAttributes(typeof(Attributes.OptionAttribute), false).Any())
                .Select(s =>
                {
                    var attr = ((Attributes.OptionAttribute)Attribute.GetCustomAttribute(s, typeof(Attributes.OptionAttribute)));
                    var message = ((Attributes.OptionHelpAttribute)Attribute.GetCustomAttribute(s, typeof(Attributes.OptionHelpAttribute)))?.Message ?? string.Empty;

                    var model = new OptionItemResult
                    {
                        ArgumentsTypes = module.GetModuleMethod(attr.OptionName).GetParameters().Select(s => s.ParameterType).ToArray(),
                        Name = attr.OptionName,
                        SeparatedArguments = attr.SeparatedArgument,
                        Message = message
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
                ModuleItemResult moduleItemResult = this.GetModelItemResult(item);

                if(moduleItem.Module == null)
                {
                    moduleItem.Module = this.GetModule(_wildcard);
                    moduleItemResult = this.GetModelItemResult(_wildcard);

                    moduleItem.ModuleArgument = moduleItem.Module != null ? System.Text.Encoding.UTF8.GetBytes(args[i]) : null;
                }

                if(moduleItem.Module!=null)
                {
                    moduleItem.Name = moduleItemResult.Name;
                    moduleItem.Message = moduleItemResult.Message;

                    var moduleOptions = GetOptionsAsQueriable(moduleItem.Module);

                    if(moduleItem.Module.HasRunnningControl(ModuleRunningControl.Unique))
                    {
                        var hasModule = modules.Where(s=>s.Module.GetType().Equals(moduleItem.Module.GetType())).Count()>0;

                        if(hasModule)
                        {
                            throw new ModulesException($"The module {item} doesn't allow attach one more than exist.");
                        }
                    }

                    if(moduleItem.Module.IsArgumentableModule())
                    {
                        var argument = args[i+1];

                        if(this.GetModule(argument)!=null || moduleOptions.Any(s=>s.Name.Equals(argument)))
                        {
                            Console.WriteLine("Argument error!");

                            Environment.Exit(1);
                        }

                        i++;

                        moduleItem.ModuleArgument = System.Text.Encoding.UTF8.GetBytes(argument);
                    }

                    if(moduleOptions.Any())
                    {
                        for (var y = i+1; y < args.Length; y++)
                        {
                            var optionResult = moduleOptions.SingleOrDefault(s=>s.Name.Equals(args[y]));

                            if(optionResult!=null)
                            {
                                var optionItem = new OptionItem(){
                                    Name = optionResult.Name,
                                    Message = optionResult.Message,
                                };

                                if(optionResult.ArgumentsTypes.Length > 1)
                                {
                                    y++;
                                    var index=0;

                                    if(optionResult.SeparatedArguments)
                                    {
                                        optionItem.Arguments = optionResult.ArgumentsTypes.Select(s=>Convert.ChangeType(args[y + index++], s)).ToArray();
                                        y++;
                                    }
                                    else
                                    {
                                        var values = args[y].Split(',');
                                        optionItem.Arguments = optionResult.ArgumentsTypes.Select(s=>Convert.ChangeType(values[index++], s)).ToArray();
                                    }
                                }
                                else if(optionResult.ArgumentsTypes.Any())
                                {
                                    y++;

                                    optionItem.Arguments = new object[]{ Convert.ChangeType(args[y], optionResult.ArgumentsTypes.SingleOrDefault()) };
                                }

                                i=y;
                                moduleItem.Options.Add(optionItem);
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

            return modules.Where(m=>m.Module.HasRunnningControl(ModuleRunningControl.Input))
                .Concat(modules.Where(m=>
                    !m.Module.HasRunnningControl(ModuleRunningControl.Input) && 
                    !m.Module.HasRunnningControl(ModuleRunningControl.Output)))
                .Concat(modules.Where(m=>m.Module.HasRunnningControl(ModuleRunningControl.Output))).ToList();
        }
    }
}