using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fjv.Modules.Commons;
using Fjv.Modules.Extensions;

namespace Fjv.Modules
{
    public partial class ModuleFactory : ModuleFactoryBase
    {
        public ModuleFactory(Assembly assembly)
            : base(assembly)
        { }

        public ModuleFactory(Assembly[] assemblies)
            : base(assemblies)
        { }

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
                    if(moduleItem.Module.HasRunnningControl(ModuleRunningControl.Unique))
                    {
                        var hasModule = modules.Where(s=>s.Module.GetType().Equals(moduleItem.Module.GetType())).Count()>0;

                        if(hasModule)
                        {
                            throw new Exception($"The module {item} doesn't allow attach one more than exist.");
                        }
                    }

                    if(moduleItem.Module.IsArgumentableModule())
                    {
                        var argument = args[i+1];

                        if(this.GetModule(argument)!=null || base.OptionExist(moduleItem.Module, argument))
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


                                if(argumentTypes.Length > 1)
                                {
                                    y++;
                                    var index=0;
                                    var values = args[y].Split(',');
                                    optionItem.Arguments = argumentTypes.Select(s=>Convert.ChangeType(values[index++], s)).ToArray();
                                }
                                else if(argumentTypes.Any())
                                {
                                    y++;

                                    optionItem.Arguments = new object[]{ Convert.ChangeType(args[y], argumentTypes.SingleOrDefault()) };
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

        public virtual byte[] Run(string[] args, byte[] buffer = null)
        {
            var modules = this.GetModulesItems(args);

            foreach (var item in modules)
            {
                if(item.Module.HasRunnningControl(ModuleRunningControl.ControlTaker))
                {
                    return Run(item, this, buffer);
                }

                buffer = Run(item, this, buffer);
            }

            return buffer;
        }

        public virtual byte[] Run(ModuleItem module, ModuleFactory moduleFactory, byte[] input)
        {
            byte[] result = null;

            if(module.Module.IsArgumentableModule())
            {
                result = (module.Module as IArgumentableModule).Load(input, module.ModuleArgument, module.GlobalArguments, module.IndexArgument);
            }
            else
            {   
                result = (module.Module as IDefaultModule).Load(input ?? module.ModuleArgument, module.GlobalArguments, module.IndexArgument);
            }

            foreach (var option in module.Options)
            {
                result = (byte[])moduleFactory.Invoke(module.Module, option.Name, option.Arguments);
            }

            //todo: dispose module.

            return result;
        }
    }
}