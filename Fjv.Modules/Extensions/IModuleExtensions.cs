using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fjv.Modules.Commons;

namespace Fjv.Modules.Extensions
{
    public static class IModuleExtensions
    {
        public static IEnumerable<string> GetOptionsMethods(this IModule type)
        {
            foreach(var item in type.GetType().GetMethods())
            {
                if (item.GetCustomAttributes(typeof(Attributes.OptionAttribute), false).Length > 0)
                {
                    yield return ((Attributes.OptionAttribute)Attribute.GetCustomAttribute(item, typeof(Attributes.OptionAttribute))).OptionName;
                }
            }
        }

        public static bool IsArgumentableModule(this IModule module)
        {
            return module.GetType().GetInterfaces().Any(s=>s.Name.Equals(nameof(IArgumentableModule)) || s.Name.Equals(nameof(IArgumentableModuleAsync)));
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

        public static MethodInfo GetModuleMethod(this Type module, string optionname) {
            foreach(var item in module.GetMethods())
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

        public static bool HasRunnningControl(this IModule module, ModuleRunningControl runnningControl)
        {
            var moduleRunningControl = ((Attributes.ModuleAttribute)Attribute.GetCustomAttribute(module.GetType(), typeof(Attributes.ModuleAttribute))).RunningControl;

            if((runnningControl & moduleRunningControl) == ModuleRunningControl.ControlTaker)
            {
                return true;
            }

            if((runnningControl & moduleRunningControl) == ModuleRunningControl.Input)
            {
                return true;
            }

            if((runnningControl & moduleRunningControl) == ModuleRunningControl.Output)
            {
                return true;
            }

            if((runnningControl & moduleRunningControl) == ModuleRunningControl.Unique)
            {
                return true;
            }

            return false;
        }
    }
}