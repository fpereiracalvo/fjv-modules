using System;
using System.Threading.Tasks;

namespace Fjv.Modules.Generic.Adapters
{
    /// <summary>
    /// Adapter for legacy IDefaultModuleAsync to work with the generic system
    /// </summary>
    /// <typeparam name="TOutput">Type of output to convert to</typeparam>
    public class DefaultModuleAsyncAdapter<TOutput> : IDefaultModuleAsync<byte[], TOutput>
    {
        private readonly IDefaultModuleAsync _legacyModule;
        private readonly Func<byte[], TOutput> _outputConverter;

        /// <summary>
        /// Creates a new adapter for a legacy IDefaultModuleAsync
        /// </summary>
        /// <param name="legacyModule">The legacy module to adapt</param>
        /// <param name="outputConverter">Converter function for the output</param>
        public DefaultModuleAsyncAdapter(IDefaultModuleAsync legacyModule, Func<byte[], TOutput> outputConverter)
        {
            _legacyModule = legacyModule ?? throw new ArgumentNullException(nameof(legacyModule));
            _outputConverter = outputConverter ?? throw new ArgumentNullException(nameof(outputConverter));
        }

        /// <summary>
        /// Loads the legacy module asynchronously and converts its output
        /// </summary>
        public async Task<TOutput> LoadAsync(byte[] input, string[] args, int index)
        {
            var result = await _legacyModule.LoadAsync(input, args, index);
            return _outputConverter(result);
        }
    }
    
    /// <summary>
    /// Adapter for legacy IArgumentableModuleAsync to work with the generic system
    /// </summary>
    /// <typeparam name="TOutput">Type of output to convert to</typeparam>
    public class ArgumentableModuleAsyncAdapter<TOutput> : IArgumentableModuleAsync<byte[], byte[], TOutput>
    {
        private readonly IArgumentableModuleAsync _legacyModule;
        private readonly Func<byte[], TOutput> _outputConverter;

        /// <summary>
        /// Creates a new adapter for a legacy IArgumentableModuleAsync
        /// </summary>
        /// <param name="legacyModule">The legacy module to adapt</param>
        /// <param name="outputConverter">Converter function for the output</param>
        public ArgumentableModuleAsyncAdapter(IArgumentableModuleAsync legacyModule, Func<byte[], TOutput> outputConverter)
        {
            _legacyModule = legacyModule ?? throw new ArgumentNullException(nameof(legacyModule));
            _outputConverter = outputConverter ?? throw new ArgumentNullException(nameof(outputConverter));
        }

        /// <summary>
        /// Loads the legacy module asynchronously and converts its output
        /// </summary>
        public async Task<TOutput> LoadAsync(byte[] input, byte[] moduleArgument, string[] args, int index)
        {
            var result = await _legacyModule.LoadAsync(input, moduleArgument, args, index);
            return _outputConverter(result);
        }
    }
    
    /// <summary>
    /// Adapter for a generic async module to work with different input types
    /// </summary>
    /// <typeparam name="TModuleInput">The module's input type</typeparam>
    /// <typeparam name="TFactoryInput">The factory's input type</typeparam>
    /// <typeparam name="TOutput">Output type</typeparam>
    public class InputConversionAsyncAdapter<TModuleInput, TFactoryInput, TOutput> : IDefaultModuleAsync<TFactoryInput, TOutput>
    {
        private readonly IDefaultModuleAsync<TModuleInput, TOutput> _module;
        private readonly Func<TFactoryInput, TModuleInput> _inputConverter;

        /// <summary>
        /// Creates a new adapter to convert input types asynchronously
        /// </summary>
        /// <param name="module">The module to adapt</param>
        /// <param name="inputConverter">Converter function for the input</param>
        public InputConversionAsyncAdapter(IDefaultModuleAsync<TModuleInput, TOutput> module, Func<TFactoryInput, TModuleInput> inputConverter)
        {
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _inputConverter = inputConverter ?? throw new ArgumentNullException(nameof(inputConverter));
        }

        /// <summary>
        /// Converts the input and loads the module asynchronously
        /// </summary>
        public async Task<TOutput> LoadAsync(TFactoryInput input, string[] args, int index)
        {
            var convertedInput = _inputConverter(input);
            return await _module.LoadAsync(convertedInput, args, index);
        }
    }
}
