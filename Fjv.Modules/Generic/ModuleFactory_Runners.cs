using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Fjv.Modules.Commons;
using Fjv.Modules.Extensions;

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
        /// Runs modules based on command line arguments
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <param name="initialInput">Optional initial input data</param>
        /// <returns>The output data</returns>
        public virtual TOutput Run(string[] args, TInput initialInput = default)
        {
            var modules = GetModulesItems(args);

            TOutput output = initialInput != null ?
                             Convert<TInput, TOutput>(initialInput) :
                             default;

            foreach (var module in modules)
            {
                try
                {
                    OnModuleExecuting?.Invoke(this, new ModuleEventArgument(module));

                    if (module.Module.IsArgumentableModule() && module.Module is IArgumentableModule<TInput, TInput, TOutput> genericArgModule)
                    {
                        TInput moduleInput = Convert<TOutput, TInput>(output);

                        output = genericArgModule.Load(moduleInput, moduleInput, args, module.IndexArgument);
                    }
                    else if (module.Module is IDefaultModule<TInput, TOutput> genericModule)
                    {
                        TInput moduleInput = Convert<TOutput, TInput>(output);

                        output = genericModule.Load(moduleInput, args, module.IndexArgument);
                    }

                    OnModuleExecuted?.Invoke(this, new ModuleEventArgument(module));

                    foreach (var option in module.Options ?? Enumerable.Empty<OptionItem>())
                    {
                        OnOptionExecuting?.Invoke(this, new OptionEventArgument(module, option));

                        if (option.Arguments == null || !option.Arguments.Any())
                        {
                            output = (TOutput)this.Invoke(module.Module, option.Name);
                        }
                        else
                        {
                            output = (TOutput)this.Invoke(module.Module, option.Name, option.Arguments);
                        }


                        OnOptionExecuted?.Invoke(this, new OptionEventArgument(module, option));
                    }
                }
                catch (Exception ex)
                {
                    this.OnError?.Invoke(this, new ModuleExceptionEventArgument(module, ex));
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
        public virtual async Task<TOutput> RunAsync(string[] args, TInput initialInput = default, CancellationToken cancellationToken = default)
        {
            var modules = GetModulesItems(args);

            TOutput output = initialInput != null ?
                             Convert<TInput, TOutput>(initialInput) :
                             default;

            foreach (var module in modules)
            {
                try
                {
                    // Raise module executing event
                    OnModuleExecuting?.Invoke(this, new ModuleEventArgument(module));

                    if (module.Module is IArgumentableModuleAsync<TInput, TInput, TOutput> genericArgAsyncModule)
                    {
                        TInput moduleInput = Convert<TOutput, TInput>(output);

                        output = await genericArgAsyncModule.LoadAsync(moduleInput, moduleInput, args, module.IndexArgument, cancellationToken);
                    }
                    else if (module.Module is IDefaultModuleAsync<TInput, TOutput> genericAsyncModule)
                    {
                        TInput moduleInput = Convert<TOutput, TInput>(output);

                        output = await genericAsyncModule.LoadAsync(moduleInput, args, module.IndexArgument, cancellationToken);
                    }

                    // Raise module executed event
                    OnModuleExecuted?.Invoke(this, new ModuleEventArgument(module));

                    // Execute options directly after processing modules
                    foreach (var option in module.Options ?? Enumerable.Empty<OptionItem>())
                    {
                        OnOptionExecuting?.Invoke(this, new OptionEventArgument(module, option));

                        if( option.Arguments == null || !option.Arguments.Any())
                        {
                            output = await (Task<TOutput>)this.Invoke(module.Module, option.Name);
                        }
                        else
                        {
                            output = await (Task<TOutput>)this.Invoke(module.Module, option.Name, option.Arguments);
                        }

                        OnOptionExecuted?.Invoke(this, new OptionEventArgument(module, option));
                    }
                }
                catch (Exception ex)
                {
                    this.OnError?.Invoke(this, new ModuleExceptionEventArgument(module, ex));

                    throw;
                }
            }

            return output;
        }

        public event EventHandler<ModuleEventArgument> OnModuleExecuting;
        public event EventHandler<ModuleEventArgument> OnModuleExecuted;
        public event EventHandler<OptionEventArgument> OnOptionExecuting;
        public event EventHandler<OptionEventArgument> OnOptionExecuted;
        public event EventHandler<ModuleExceptionEventArgument> OnError;
    }
}
