using System.Threading;
using System.Threading.Tasks;

namespace Fjv.Modules.Generic
{
    /// <summary>
    /// Generic interface for asynchronous default modules with type-safe input and output
    /// </summary>
    /// <typeparam name="TInput">Type of the input data</typeparam>
    /// <typeparam name="TOutput">Type of the output data</typeparam>
    public interface IDefaultModuleAsync<TInput, TOutput> : IModule<TInput, TOutput>
    {
        /// <summary>
        /// Asynchronously loads the module with type-safe input and output
        /// </summary>
        /// <param name="input">The typed input data</param>
        /// <param name="args">Command line arguments</param>
        /// <param name="index">Index of the current module in arguments array</param>
        /// <returns>A task that represents the asynchronous operation with typed output data</returns>
        Task<TOutput> LoadAsync(TInput input, string[] args, int index, CancellationToken cancellationToken = default);
    }
}
