using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Fjv.Modules.DependencyInjection
{
    public class ModuleFactoryAssemblyConfiguration
    {
        public Assembly Assembly { get; set; }
        public List<ModuleOptions> Options { get; set; }
    }
}