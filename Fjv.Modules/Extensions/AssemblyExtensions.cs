using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fjv.Modules.Extensions
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetModuleTypes(this Assembly assembly) {
            foreach(Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(Attributes.ModuleAttribute), false).Length > 0)
                {
                    yield return type;
                }
            }
        }
    }
}