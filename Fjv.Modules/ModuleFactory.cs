using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fjv.Modules.Commons;
using Fjv.Modules.Exceptions;
using Fjv.Modules.Extensions;

namespace Fjv.Modules
{
    public partial class ModuleFactory : ModuleFactoryBase, IModuleFactory
    {

        public ModuleFactory(Assembly assembly, List<ModuleOptions> options = null)
            : base(assembly, options)
        { }

        public ModuleFactory(Assembly[] assemblies, List<ModuleOptions> options = null)
            : base(assemblies, options)
        { }

        public ModuleFactory(Type scopedToNamespace, List<ModuleOptions> options = null)
            : base(scopedToNamespace, options)
        { }

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