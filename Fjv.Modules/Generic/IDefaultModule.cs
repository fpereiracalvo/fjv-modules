namespace Fjv.Modules.Generic
{
    /// <summary>
    /// Generic interface for default modules with type-safe input and output
    /// </summary>
    /// <typeparam name="TInput">Type of the input data</typeparam>
    /// <typeparam name="TOutput">Type of the output data</typeparam>
    public interface IDefaultModule<TInput, TOutput> : IModule<TInput, TOutput>
    {
        /// <summary>
        /// Loads the module with type-safe input and output
        /// </summary>
        /// <param name="input">The typed input data</param>
        /// <param name="args">Command line arguments</param>
        /// <param name="index">Index of the current module in arguments array</param>
        /// <returns>The typed output data</returns>
        TOutput Load(TInput input, string[] args, int index);
    }
}
