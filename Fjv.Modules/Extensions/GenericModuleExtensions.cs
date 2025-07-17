using System;
using System.Collections.Generic;
using System.Reflection;
using Fjv.Modules.Generic;
using Fjv.Modules.Generic.Adapters;

namespace Fjv.Modules.Extensions
{
    /// <summary>
    /// Extension methods for generic module factory functionality
    /// </summary>
    public static class GenericModuleExtensions
    {
        /// <summary>
        /// Creates a typed module factory from a legacy module factory
        /// </summary>
        /// <typeparam name="TOutput">The desired output type</typeparam>
        /// <param name="factory">The legacy factory</param>
        /// <param name="converter">Optional custom converter from byte[] to TOutput</param>
        /// <returns>A generic module factory with byte[] input and TOutput output</returns>
        public static IModuleFactory<byte[], TOutput> AsGeneric<TOutput>(
            this IModuleFactory factory, 
            Func<byte[], TOutput> converter = null)
        {
            // Create a new factory and register adapters
            var genericFactory = new ModuleFactory<byte[], TOutput>(
                typeof(GenericModuleExtensions).Assembly); // Or use reflection to get the assemblies from the original factory
                
            // More complex implementation would be needed to adapt the entire factory
            throw new NotImplementedException("Factory adaptation is not yet implemented");
        }
        
        /// <summary>
        /// Creates a generic module factory for the specified types
        /// </summary>
        /// <typeparam name="TInput">The input type for modules</typeparam>
        /// <typeparam name="TOutput">The output type for modules</typeparam>
        /// <param name="assembly">Assembly to scan for modules</param>
        /// <param name="options">Optional module options</param>
        /// <returns>A new generic module factory</returns>
        public static IModuleFactory<TInput, TOutput> CreateGenericFactory<TInput, TOutput>(
            Assembly assembly, 
            List<ModuleOptions> options = null)
        {
            return new ModuleFactory<TInput, TOutput>(assembly, options);
        }
        
        /// <summary>
        /// Creates a generic module factory for the specified types
        /// </summary>
        /// <typeparam name="TInput">The input type for modules</typeparam>
        /// <typeparam name="TOutput">The output type for modules</typeparam>
        /// <param name="assemblies">Assemblies to scan for modules</param>
        /// <param name="options">Optional module options</param>
        /// <returns>A new generic module factory</returns>
        public static IModuleFactory<TInput, TOutput> CreateGenericFactory<TInput, TOutput>(
            Assembly[] assemblies, 
            List<ModuleOptions> options = null)
        {
            return new ModuleFactory<TInput, TOutput>(assemblies, options);
        }
        
        /// <summary>
        /// Creates a generic module factory for the specified types
        /// </summary>
        /// <typeparam name="TInput">The input type for modules</typeparam>
        /// <typeparam name="TOutput">The output type for modules</typeparam>
        /// <param name="scopedToNamespace">Type whose namespace will be used to scope module scanning</param>
        /// <param name="options">Optional module options</param>
        /// <returns>A new generic module factory</returns>
        public static IModuleFactory<TInput, TOutput> CreateGenericFactory<TInput, TOutput>(
            Type scopedToNamespace, 
            List<ModuleOptions> options = null)
        {
            return new ModuleFactory<TInput, TOutput>(scopedToNamespace, options);
        }
        
        /// <summary>
        /// Adapts a legacy module to work with the generic system
        /// </summary>
        /// <typeparam name="TOutput">The desired output type</typeparam>
        /// <param name="module">The legacy module</param>
        /// <param name="converter">Optional custom converter from byte[] to TOutput</param>
        /// <returns>A generic module adapter</returns>
        public static IDefaultModule<byte[], TOutput> AsGeneric<TOutput>(
            this IDefaultModule module,
            Func<byte[], TOutput> converter = null)
        {
            converter ??= GetDefaultConverter<TOutput>();
            return new DefaultModuleAdapter<TOutput>(module, converter);
        }
        
        /// <summary>
        /// Adapts a legacy argumentable module to work with the generic system
        /// </summary>
        /// <typeparam name="TOutput">The desired output type</typeparam>
        /// <param name="module">The legacy argumentable module</param>
        /// <param name="converter">Optional custom converter from byte[] to TOutput</param>
        /// <returns>A generic argumentable module adapter</returns>
        public static IArgumentableModule<byte[], byte[], TOutput> AsGeneric<TOutput>(
            this IArgumentableModule module,
            Func<byte[], TOutput> converter = null)
        {
            converter ??= GetDefaultConverter<TOutput>();
            return new ArgumentableModuleAdapter<TOutput>(module, converter);
        }
        
        /// <summary>
        /// Gets a default converter for common types
        /// </summary>
        private static Func<byte[], TOutput> GetDefaultConverter<TOutput>()
        {
            if (typeof(TOutput) == typeof(byte[]))
                return b => (TOutput)(object)b;
                
            if (typeof(TOutput) == typeof(string))
                return b => (TOutput)(object)System.Text.Encoding.UTF8.GetString(b);
                
            // Add more default conversions as needed
                
            throw new InvalidOperationException(
                $"No default converter available from byte[] to {typeof(TOutput).Name}. " +
                $"Please provide a custom converter.");
        }
    }
}
