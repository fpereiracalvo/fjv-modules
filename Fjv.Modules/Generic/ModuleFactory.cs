using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Fjv.Modules.Commons;
using Fjv.Modules.Exceptions;
using Fjv.Modules.Extensions;

namespace Fjv.Modules.Generic
{
    /// <summary>
    /// Generic module factory that supports strongly-typed input and output
    /// </summary>
    /// <typeparam name="TInput">Type of the input data for modules</typeparam>
    /// <typeparam name="TOutput">Type of the output data from modules</typeparam>
    public partial class ModuleFactory<TInput, TOutput> : ModuleFactoryBase, IModuleFactory<TInput, TOutput>, IModuleFactory
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
            // Default implementation for byte[] conversion
            if (typeof(TFrom) == typeof(byte[]) && typeof(TTo) == typeof(string))
            {
                return (TFrom input) => (TTo)(object)System.Text.Encoding.UTF8.GetString((byte[])(object)input);
            }
            else if (typeof(TFrom) == typeof(string) && typeof(TTo) == typeof(byte[]))
            {
                return (TFrom input) => (TTo)(object)System.Text.Encoding.UTF8.GetBytes((string)(object)input);
            }
            else if (typeof(TFrom) == typeof(TTo))
            {
                return (TFrom input) => (TTo)(object)input;
            }
            
            // Add more conversions as needed
            
            throw new ModulesException($"No conversion available from {typeof(TFrom).Name} to {typeof(TTo).Name}");
        }
        
        #region IModuleFactory Implementation
        
        /// <summary>
        /// Runs modules based on command line arguments using byte arrays for legacy compatibility
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <param name="buffer">Input buffer</param>
        /// <returns>Output buffer</returns>
        public byte[] Run(string[] args, byte[] buffer = null)
        {
            // Convert and delegate to the typed version
            TInput input = Convert<byte[], TInput>(buffer);
            TOutput output = Run(args, input);
            return Convert<TOutput, byte[]>(output);
        }

        /// <summary>
        /// Runs a specific module with the given input
        /// </summary>
        /// <param name="module">Module item to run</param>
        /// <param name="moduleFactory">Module factory (used for some operations)</param>
        /// <param name="input">Input data</param>
        /// <returns>Output data</returns>
        public byte[] Run(ModuleItem module, ModuleFactory moduleFactory, byte[] input)
        {
            // Create a delegate to pass to the module
            Func<byte[], byte[]> runnerDelegate = (data) => {
                // Convert byte[] to typed data, process, and convert back
                var typedInput = Convert<byte[], TInput>(data);
                // Module processing would happen here
                // For now, just return the input as is
                var typedOutput = default(TOutput);
                return Convert<TOutput, byte[]>(typedOutput);
            };
            
            // Execute the module with our delegate
            // For now, we just return the input as is
            return input;
        }

        /// <summary>
        /// Asynchronously runs modules based on command line arguments using byte arrays for legacy compatibility
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <param name="buffer">Input buffer</param>
        /// <returns>Task with output buffer</returns>
        public async Task<byte[]> RunAsync(string[] args, byte[] buffer = null)
        {
            // Convert and delegate to the typed version
            TInput input = Convert<byte[], TInput>(buffer);
            TOutput output = await RunAsync(args, input);
            return Convert<TOutput, byte[]>(output);
        }

        /// <summary>
        /// Asynchronously runs a specific module with the given input
        /// </summary>
        /// <param name="module">Module item to run</param>
        /// <param name="moduleFactory">Module factory (used for some operations)</param>
        /// <param name="input">Input data</param>
        /// <returns>Task with output data</returns>
        public async Task<byte[]> RunAsync(ModuleItem module, ModuleFactory moduleFactory, byte[] input)
        {
            // Create a delegate to pass to the module
            Func<byte[], Task<byte[]>> runnerDelegate = (data) => {
                // Convert byte[] to typed data, process, and convert back
                var typedInput = Convert<byte[], TInput>(data);
                // Module processing would happen here
                // For now, just return the input as is
                var typedOutput = default(TOutput);
                return Task.FromResult(Convert<TOutput, byte[]>(typedOutput));
            };
            
            // Execute the module with our delegate asynchronously
            // For now, we just return the input as is
            return await Task.FromResult(input);
        }

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
