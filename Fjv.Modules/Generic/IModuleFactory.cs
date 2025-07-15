using System;
using System.Collections.Generic;
using Fjv.Modules.Commons;

namespace Fjv.Modules.Generic
{
    /// <summary>
    /// Generic interface for a module factory that supports strongly-typed input and output
    /// </summary>
    /// <typeparam name="TInput">Type of the input data for modules</typeparam>
    /// <typeparam name="TOutput">Type of the output data from modules</typeparam>
    public interface IModuleFactory<TInput, TOutput>
    {
        /// <summary>
        /// Executes modules based on command line arguments with strongly-typed input and output
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <param name="initialInput">Optional initial input data</param>
        /// <returns>The final output data</returns>
        TOutput Run(string[] args, TInput initialInput = default);

        /// <summary>
        /// Asynchronously executes modules based on command line arguments with strongly-typed input and output
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <param name="initialInput">Optional initial input data</param>
        /// <returns>A task that represents the asynchronous operation with typed output data</returns>
        System.Threading.Tasks.Task<TOutput> RunAsync(string[] args, TInput initialInput = default);

        /// <summary>
        /// Gets help information for modules based on command line arguments
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>Help information as a string</returns>
        string GetHelp(string[] args = null);

        /// <summary>
        /// Gets the list of modules found based on the command line arguments
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>List of module items</returns>
        List<ModuleItem> GetModulesItems(string[] args);
        
        // Events
        event EventHandler<ModuleEventArgument> OnModuleExecuting;
        event EventHandler<ModuleEventArgument> OnModuleExecuted;
        event EventHandler<OptionEventArgument> OnOptionExecuting;
        event EventHandler<OptionEventArgument> OnOptionExecuted;
        event EventHandler<ModuleExceptionEventArgument> OnError;
    }
}
