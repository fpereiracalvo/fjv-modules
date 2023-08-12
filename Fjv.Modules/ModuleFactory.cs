using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Fjv.Modules.Commons;
using Fjv.Modules.Exceptions;
using Fjv.Modules.Extensions;

namespace Fjv.Modules
{
    public partial class ModuleFactory : ModuleFactoryBase, IModuleFactory
    {
        readonly string _wildcard = "*";

        public ModuleFactory(Assembly assembly, List<ModuleOptions> options = null)
            : base(assembly, options)
        { }

        public ModuleFactory(Assembly[] assemblies, List<ModuleOptions> options = null)
            : base(assemblies, options)
        { }

        public ModuleFactory(Type scopedToNamespace, List<ModuleOptions> options = null)
            : base(scopedToNamespace, options)
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

        public virtual string GetHelp(string[] args)
        {
            var modules = this.GetModulesItems(args).Distinct().ToList();

            var stringBuilder = new StringBuilder();

            modules.Where(s=>!string.IsNullOrWhiteSpace(s.Message)).ToList().ForEach(m=> {
                stringBuilder.AppendLine($"{m.Name}\t{m.Message}");

                m.Options.Distinct().ToList().ForEach(o=>stringBuilder.AppendLine($"\t{o.Name}\t{o.Message}"));
            });

            return stringBuilder.ToString();
        }

        public virtual string GetHelp()
        {
            var modules = this.GetModulesAsQueryable().ToList();

            var stringBuilder = new StringBuilder();

            modules.Where(s=>!string.IsNullOrWhiteSpace(s.Message)).OrderBy(s=>s.Name).ToList().ForEach(m=> {
                stringBuilder.AppendLine($"{m.Name}\t{m.Message}");

                this.GetOptionsAsQueriable(m.Module).OrderBy(s=>s.Name).ToList().ForEach(o=>stringBuilder.AppendLine($"\t{o.Name}\t{o.Message}"));
            });

            return stringBuilder.ToString();
        }
    }
}