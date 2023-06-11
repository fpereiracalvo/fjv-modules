using System;
using System.Collections.Generic;

namespace Fjv.Modules
{
    public class ModuleOptions
    {
        public Type ModuleType { get; set; }
        public string Name { get; set; } = string.Empty;

        internal string GetName(string @default)
        {
            return !string.IsNullOrWhiteSpace(Name) ? Name : @default;
        }
    }
}