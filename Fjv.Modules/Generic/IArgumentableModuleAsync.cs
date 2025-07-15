using System.Threading.Tasks;

namespace Fjv.Modules.Generic
{
    /// <summary>
    /// Generic interface for asynchronous modules that accept a direct argument with type-safe input, argument and output
    /// </summary>
    /// <typeparam name="TInput">Type of the input data</typeparam>
    /// <typeparam name="TArg">Type of the module argument</typeparam>
    /// <typeparam name="TOutput">Type of the output data</typeparam>
    public interface IArgumentableModuleAsync<TInput, TArg, TOutput> : IModule<TInput, TOutput>
    {
        /// <summary>
        /// Asynchronously loads the module with type-safe input, argument and output
        /// </summary>
        /// <param name="input">The typed input data</param>
        /// <param name="moduleArgument">The typed module argument</param>
        /// <param name="args">Command line arguments</param>
        /// <param name="index">Index of the current module in arguments array</param>
        /// <returns>A task that represents the asynchronous operation with typed output data</returns>
        Task<TOutput> LoadAsync(TInput input, TArg moduleArgument, string[] args, int index);
    }
}
