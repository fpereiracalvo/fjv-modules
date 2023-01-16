using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Fjv.Modules.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetModuleTypes(this Type typeOfNamespace)
        {
            var types = typeOfNamespace.Assembly.GetTypes().Where(s=>!string.IsNullOrWhiteSpace(s.Namespace)).ToList();

            foreach(Type type in types.Where(s=>s.Namespace.Contains(typeOfNamespace.Namespace)))
            {
                if (type.GetCustomAttributes(typeof(Attributes.ModuleAttribute), false).Length > 0)
                {
                    yield return type;
                }
            }
        }
    }
}