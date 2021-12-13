using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fjv.Modules.Extensions
{
    public static class IModuleExtensions
    {
        public static IEnumerable<string> GetOptionsMethods(this IModule type) {
            foreach(var item in type.GetType().GetMethods())
            {
                if (item.GetCustomAttributes(typeof(Attributes.OptionAttribute), false).Length > 0)
                {
                    yield return ((Attributes.OptionAttribute)Attribute.GetCustomAttribute(item, typeof(Attributes.OptionAttribute))).OptionName;
                }
            }
        }

        public static MethodInfo GetMethod(this IModule module, string optionname) {
            foreach(var item in module.GetType().GetMethods())
            {
                if (item.GetCustomAttributes(typeof(Attributes.OptionAttribute), false).Length > 0)
                {
                    if(((Attributes.OptionAttribute)Attribute.GetCustomAttribute(item, typeof(Attributes.OptionAttribute))).OptionName.Equals(optionname))
                    {
                        return item;
                    }
                }
            }

            return null;
        }
    }
}