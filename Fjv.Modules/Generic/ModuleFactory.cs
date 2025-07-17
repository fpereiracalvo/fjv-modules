using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fjv.Modules.Exceptions;

namespace Fjv.Modules.Generic
{
    /// <summary>
    /// Generic module factory that supports strongly-typed input and output
    /// </summary>
    /// <typeparam name="TInput">Type of the input data for modules</typeparam>
    /// <typeparam name="TOutput">Type of the output data from modules</typeparam>
    public partial class ModuleFactory<TInput, TOutput> : ModuleFactoryBase, IModuleFactory<TInput, TOutput>
    {
        readonly string _wildcard = "*";
        
        /// <summary>
        /// Initializes a new instance of the generic module factory with a single assembly
        /// </summary>
        /// <param name="assembly">Assembly to scan for modules</param>
        /// <param name="options">Optional module options</param>
        public ModuleFactory(Assembly assembly, List<ModuleOptions> options = null)
            : base(assembly, options)
        { }

        /// <summary>
        /// Initializes a new instance of the generic module factory with multiple assemblies
        /// </summary>
        /// <param name="assemblies">Assemblies to scan for modules</param>
        /// <param name="options">Optional module options</param>
        public ModuleFactory(Assembly[] assemblies, List<ModuleOptions> options = null)
            : base(assemblies, options)
        { }

        /// <summary>
        /// Initializes a new instance of the generic module factory scoped to a namespace
        /// </summary>
        /// <param name="scopedToNamespace">Type whose namespace will be used to scope module scanning</param>
        /// <param name="options">Optional module options</param>
        public ModuleFactory(Type scopedToNamespace, List<ModuleOptions> options = null)
            : base(scopedToNamespace, options)
        { }

        /// <summary>
        /// Creates a converter to transform data between types
        /// </summary>
        /// <typeparam name="TFrom">Source type</typeparam>
        /// <typeparam name="TTo">Target type</typeparam>
        /// <returns>A function that converts from TFrom to TTo</returns>
        protected virtual Func<TFrom, TTo> CreateConverter<TFrom, TTo>()
        {
            return input => (TTo)(object)input;
        }
        
        #region IModuleFactory Implementation

        /// <summary>
        /// Gets help information for modules based on command line arguments
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>Help information as a string</returns>
        public string GetHelp(string[] args)
        {
            // Basic implementation that generates help from module attributes
            if (args == null || args.Length == 0)
            {
                return GetHelp(); // General help for all modules
            }

            // Get help for specific module
            var moduleName = args[0];
            var module = GetModule(moduleName);
            if (module == null)
                return $"Module '{moduleName}' not found.";

            var moduleType = module.GetType();
            var helpAttr = (Attributes.ModuleHelpAttribute)Attribute.GetCustomAttribute(moduleType, typeof(Attributes.ModuleHelpAttribute));
            return helpAttr?.Message ?? $"No help available for module '{moduleName}'.";
        }

        /// <summary>
        /// Gets general help information for all modules
        /// </summary>
        /// <returns>Help information as a string</returns>
        public string GetHelp()
        {
            // Basic implementation that lists all available modules
            var modules = GetModulesAsQueryable().ToList();
            if (modules.Count == 0)
                return "No modules available.";
                
            var help = "Available modules:\n\n";
            foreach (var module in modules)
            {
                help += $"- {module.Name}: {module.Message}\n";
            }
            
            return help;
        }
        
        #endregion
    }
}
