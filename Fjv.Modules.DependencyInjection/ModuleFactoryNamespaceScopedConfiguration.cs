using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fjv.Modules.DependencyInjection
{
    public class ModuleFactoryNamespaceScopedConfiguration
    {
        public Type ScopedNamespaceType { get; set; }
        public List<ModuleOptions> Options { get; set; }
    }
}