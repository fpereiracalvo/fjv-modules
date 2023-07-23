using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Fjv.Modules.DependencyInjection
{
    public class ModuleFactoryAssembliesConfiguration
    {
        public Assembly[] Assemblies { get; set; }
        public List<ModuleOptions> Options { get; set; }
    }
}