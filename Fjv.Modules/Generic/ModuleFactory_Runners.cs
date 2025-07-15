using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Fjv.Modules.Commons;
using Fjv.Modules.Exceptions;
using Fjv.Modules.Extensions;
using Fjv.Modules.Generic.Adapters;

namespace Fjv.Modules.Generic
{
    /// <summary>
    /// Generic ModuleFactory implementation with runners for different module types
    /// </summary>
    /// <typeparam name="TInput">Type of input for modules</typeparam>
    /// <typeparam name="TOutput">Type of output from modules</typeparam>
    public partial class ModuleFactory<TInput, TOutput> : ModuleFactoryBase, IModuleFactory<TInput, TOutput>
    {
        private bool _continueOnError = true;
        
        /// <summary>
        /// Convert between types with proper handling
        /// </summary>
        private TTo Convert<TFrom, TTo>(TFrom value)
        {
            if (value == null)
                return default;
                
            if (typeof(TFrom) == typeof(TTo))
                return (TTo)(object)value;
                
            return CreateConverter<TFrom, TTo>()(value);
        }
        
        /// <summary>
        /// Gets module items from arguments
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>List of module items detected from arguments</returns>
        public List<ModuleItem> GetModulesItems(string[] args)
        {
            var modules = new List<ModuleItem>();
            string wildcard = "*"; // Wildcard para módulos especiales

            // Este es un código simplificado basado en la implementación de ModuleFactory
            // Implementamos el algoritmo básico sin todas las verificaciones especiales
            for (var i = 0; i < args.Length; i++)
            {
                var moduleItem = new ModuleItem(){
                    IndexArgument = i,
                    GlobalArguments = args
                };

                var item = args[i];

                // Obtenemos el módulo y su resultado
                moduleItem.Module = this.GetModule(item);
                var moduleItemResult = this.GetModelItemResult(item);

                // Si no encontramos el módulo, probamos con el wildcard
                if (moduleItem.Module == null)
                {
                    moduleItem.Module = this.GetModule(wildcard);
                    moduleItemResult = this.GetModelItemResult(wildcard);

                    // Si el módulo wildcard existe, el argumento actual es su argumento
                    moduleItem.ModuleArgument = moduleItem.Module != null ? 
                        System.Text.Encoding.UTF8.GetBytes(args[i]) : null;
                }

                // Si encontramos un módulo, lo añadimos a la lista
                if (moduleItem.Module != null)
                {
                    if (moduleItemResult != null)
                    {
                        moduleItem.Name = moduleItemResult.Name;
                        moduleItem.Message = moduleItemResult.Message;
                    }
                    
                    modules.Add(moduleItem);
                    
                    // Aquí podrían ir comprobaciones específicas para módulos genéricos
                    // ...
                }
            }

            return modules;
        }
        
        /// <summary>
        /// Runs modules based on command line arguments
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <param name="initialInput">Optional initial input data</param>
        /// <returns>The output data</returns>
    public virtual TOutput Run(string[] args, TInput initialInput = default)
    {
        var modules = GetModulesItems(args);
        
        // Obtenemos las opciones para cada módulo
        foreach (var module in modules ?? Enumerable.Empty<ModuleItem>())
        {
            if (args.Length > module.IndexArgument + 1 && module.Module != null)
            {
                var optionName = args[module.IndexArgument + 1];
                
                // Agregar la opción al módulo para poder invocarla después
                var option = new OptionItem
                {
                    Name = optionName,
                    Arguments = null // Aquí podríamos incluir argumentos adicionales si los hubiera
                };
                
                if (module.Options == null)
                    module.Options = new List<OptionItem>();
                    
                module.Options.Add(option);
            }
        }
        
        if (modules == null || !modules.Any())
            return default;

            // Use our safe conversion method
            TOutput output = initialInput != null ? 
                             Convert<TInput, TOutput>(initialInput) : 
                             default;
            
            foreach (var module in modules)
            {
                // Skip invalid modules
                if (module.Module == null)
                    continue;

                try
                {
                    // Raise module executing event
                    OnModuleExecuting?.Invoke(this, new ModuleEventArgument(module));

                    // Convert output to byte[] if needed for legacy modules
                    byte[] byteInput = default;
                    if (output != null)
                    {
                        if (typeof(TOutput) == typeof(byte[]))
                        {
                            byteInput = output as byte[];
                        }
                        else
                        {
                            var toByteConverter = CreateConverter<TOutput, byte[]>();
                            byteInput = toByteConverter(output);
                        }
                    }
                    else
                    {
                        byteInput = new byte[0];
                    }

                    // Handle different module types
                    if (module.Module is IDefaultModule<TInput, TOutput> genericModule)
                    {
                        // Convert current output back to input type for the next module
                        TInput moduleInput = Convert<TOutput, TInput>(output);
                        output = genericModule.Load(moduleInput, args, module.IndexArgument);
                    }
                    else if (module.Module is IArgumentableModule<TInput, TInput, TOutput> genericArgModule)
                    {
                        TInput moduleInput = Convert<TOutput, TInput>(output);
                        output = genericArgModule.Load(moduleInput, moduleInput, args, module.IndexArgument);
                    }
                    else if (module.Module is IArgumentableModule<TInput, byte[], TOutput> genericByteArgModule)
                    {
                        TInput moduleInput = Convert<TOutput, TInput>(output);
                        output = genericByteArgModule.Load(moduleInput, module.ModuleArgument, args, module.IndexArgument);
                    }
                    else if (module.Module is IDefaultModule defaultModule)
                    {
                        byte[] result = defaultModule.Load(byteInput, args, module.IndexArgument);
                        output = Convert<byte[], TOutput>(result);
                    }
                    else if (module.Module is IArgumentableModule argModule)
                    {
                        byte[] result = argModule.Load(byteInput, module.ModuleArgument, args, module.IndexArgument);
                        output = Convert<byte[], TOutput>(result);
                    }
                    
                    // Raise module executed event
                    OnModuleExecuted?.Invoke(this, new ModuleEventArgument(module));

                    // Execute options directly after processing modules
                    foreach (var option in module.Options ?? Enumerable.Empty<OptionItem>())
                    {
                        try
                        {
                            // Raise option executing event
                            OnOptionExecuting?.Invoke(this, new OptionEventArgument(module, option));
                            
                            // Process the option
                            if (module.Module is IModule<TInput, TOutput> optionGenericModule)
                            {
                                // Convert output to input for next module
                                TInput optionConvertedInput = Convert<TOutput, TInput>(output);
                                
                                // Use reflection to invoke the option method
                                var method = module.Module.GetType().GetMethod(option.Name);
                                if (method != null)
                                {
                                    output = (TOutput)method.Invoke(module.Module, new object[] { optionConvertedInput });
                                }
                            }
                            else
                            {
                                // Legacy module handling
                                var optionByteInput = Convert<TOutput, byte[]>(output);
                                var invokeMethod = module.Module.GetType().GetMethod("Invoke");
                                if (invokeMethod != null && invokeMethod.GetParameters().Length == 2)
                                {
                                    var result = (byte[])invokeMethod.Invoke(module.Module, 
                                                  new object[] { option.Name, optionByteInput });
                                    output = Convert<byte[], TOutput>(result);
                                }
                            }
                            
                            // Raise option executed event
                            OnOptionExecuted?.Invoke(this, new OptionEventArgument(module, option));
                        }
                        catch (Exception ex)
                        {
                            var eventArg = new ModuleExceptionEventArgument(module, ex);
                            OnError?.Invoke(this, eventArg);
                            
                            if (!_continueOnError)
                                throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle errors
                    var eventArg = new ModuleExceptionEventArgument(module, ex);
                    OnError?.Invoke(this, eventArg);

                    if (!_continueOnError)
                        throw;
                }
            }

            return output;
        }

        /// <summary>
        /// Runs modules asynchronously based on command line arguments
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <param name="initialInput">Optional initial input data</param>
        /// <returns>A task with the output data</returns>
    public virtual async Task<TOutput> RunAsync(string[] args, TInput initialInput = default)
    {
        var modules = GetModulesItems(args);
        
        // Obtenemos las opciones para cada módulo
        foreach (var module in modules ?? Enumerable.Empty<ModuleItem>())
        {
            if (args.Length > module.IndexArgument + 1 && module.Module != null)
            {
                var optionName = args[module.IndexArgument + 1];
                
                // Agregar la opción al módulo para poder invocarla después
                var option = new OptionItem
                {
                    Name = optionName,
                    Arguments = null // Aquí podríamos incluir argumentos adicionales si los hubiera
                };
                
                if (module.Options == null)
                    module.Options = new List<OptionItem>();
                    
                module.Options.Add(option);
            }
        }
        
        if (modules == null || !modules.Any())
            return default;

            TOutput output = initialInput != null ?
                             Convert<TInput, TOutput>(initialInput) :
                             default;
            
            foreach (var module in modules)
            {
                // Skip invalid modules
                if (module.Module == null)
                    continue;

                try
                {
                    // Raise module executing event
                    OnModuleExecuting?.Invoke(this, new ModuleEventArgument(module));

                    // Convert output to byte[] if needed for legacy modules
                    byte[] byteInput = default;
                    if (output != null)
                    {
                        if (typeof(TOutput) == typeof(byte[]))
                        {
                            byteInput = output as byte[];
                        }
                        else
                        {
                            var toByteConverter = CreateConverter<TOutput, byte[]>();
                            byteInput = toByteConverter(output);
                        }
                    }
                    else
                    {
                        byteInput = new byte[0];
                    }

                    // Handle different module types
                    if (module.Module is IDefaultModuleAsync<TInput, TOutput> genericAsyncModule)
                    {
                        TInput moduleInput = Convert<TOutput, TInput>(output);
                        output = await genericAsyncModule.LoadAsync(moduleInput, args, module.IndexArgument);
                    }
                    else if (module.Module is IArgumentableModuleAsync<TInput, TInput, TOutput> genericArgAsyncModule)
                    {
                        TInput moduleInput = Convert<TOutput, TInput>(output);
                        output = await genericArgAsyncModule.LoadAsync(moduleInput, moduleInput, args, module.IndexArgument);
                    }
                    else if (module.Module is IArgumentableModuleAsync<TInput, byte[], TOutput> genericByteArgAsyncModule)
                    {
                        TInput moduleInput = Convert<TOutput, TInput>(output);
                        output = await genericByteArgAsyncModule.LoadAsync(moduleInput, module.ModuleArgument, args, module.IndexArgument);
                    }
                    else if (module.Module is IDefaultModuleAsync asyncModule)
                    {
                        byte[] result = await asyncModule.LoadAsync(byteInput, args, module.IndexArgument);
                        output = Convert<byte[], TOutput>(result);
                    }
                    else if (module.Module is IArgumentableModuleAsync asyncArgModule)
                    {
                        byte[] result = await asyncArgModule.LoadAsync(byteInput, module.ModuleArgument, args, module.IndexArgument);
                        output = Convert<byte[], TOutput>(result);
                    }
                    else
                    {
                        // Fall back to synchronous execution for non-async modules
                        TInput moduleInput = Convert<TOutput, TInput>(output);
                        output = Run(args, moduleInput);
                    }

                    // Raise module executed event
                    OnModuleExecuted?.Invoke(this, new ModuleEventArgument(module));

                    // Execute options directly after processing modules
                    foreach (var option in module.Options ?? Enumerable.Empty<OptionItem>())
                    {
                        try
                        {
                            // Raise option executing event
                            OnOptionExecuting?.Invoke(this, new OptionEventArgument(module, option));
                            
                            // Process the option asynchronously
                            if (module.Module is IModule<TInput, TOutput> asyncOptionGenericModule)
                            {
                                // Convert output to input for next module
                                TInput asyncOptionConvertedInput = Convert<TOutput, TInput>(output);
                                
                                // Use reflection to invoke the option method
                                var method = module.Module.GetType().GetMethod(option.Name);
                                if (method != null)
                                {
                                    // Check if method returns Task<TOutput>
                                    if (method.ReturnType.IsGenericType && 
                                        method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                                    {
                                        // Invoke and await the task
                                        var task = (Task<TOutput>)method.Invoke(module.Module, new object[] { asyncOptionConvertedInput });
                                        output = await task;
                                    }
                                    else
                                    {
                                        // Regular synchronous method
                                        output = (TOutput)method.Invoke(module.Module, new object[] { asyncOptionConvertedInput });
                                    }
                                }
                            }
                            else
                            {
                                // Legacy module handling
                                var asyncOptionByteInput = Convert<TOutput, byte[]>(output);
                                
                                // Try InvokeAsync first
                                var invokeAsyncMethod = module.Module.GetType().GetMethod("InvokeAsync");
                                if (invokeAsyncMethod != null && invokeAsyncMethod.GetParameters().Length == 2)
                                {
                                    var task = (Task<byte[]>)invokeAsyncMethod.Invoke(module.Module, 
                                              new object[] { option.Name, asyncOptionByteInput });
                                    var result = await task;
                                    output = Convert<byte[], TOutput>(result);
                                }
                                else
                                {
                                    // Fall back to synchronous
                                    var invokeMethod = module.Module.GetType().GetMethod("Invoke");
                                    if (invokeMethod != null && invokeMethod.GetParameters().Length == 2)
                                    {
                                        var result = (byte[])invokeMethod.Invoke(module.Module, 
                                                    new object[] { option.Name, asyncOptionByteInput });
                                        output = Convert<byte[], TOutput>(result);
                                    }
                                }
                            }
                            
                            // Raise option executed event
                            OnOptionExecuted?.Invoke(this, new OptionEventArgument(module, option));
                        }
                        catch (Exception ex)
                        {
                            var eventArg = new ModuleExceptionEventArgument(module, ex);
                            OnError?.Invoke(this, eventArg);
                            
                            if (!_continueOnError)
                                throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle errors
                    var eventArg = new ModuleExceptionEventArgument(module, ex);
                    OnError?.Invoke(this, eventArg);

                    if (!_continueOnError)
                        throw;
                }
            }

            return output;
        }

        // Nota: Las funciones RunOptions y RunOptionsAsync se han eliminado ya que
        // hemos integrado su lógica directamente en los métodos Run y RunAsync

        // Events implementation
        public event EventHandler<ModuleEventArgument> OnModuleExecuting;
        public event EventHandler<ModuleEventArgument> OnModuleExecuted;
        public event EventHandler<OptionEventArgument> OnOptionExecuting;
        public event EventHandler<OptionEventArgument> OnOptionExecuted;
        public event EventHandler<ModuleExceptionEventArgument> OnError;
    }
}
