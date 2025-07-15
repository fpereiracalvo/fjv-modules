using System;

namespace Fjv.Modules.Generic.Adapters
{
    /// <summary>
    /// Adapter for legacy IDefaultModule to work with the generic system
    /// </summary>
    /// <typeparam name="TOutput">Type of output to convert to</typeparam>
    public class DefaultModuleAdapter<TOutput> : IDefaultModule<byte[], TOutput>
    {
        private readonly IDefaultModule _legacyModule;
        private readonly Func<byte[], TOutput> _outputConverter;

        /// <summary>
        /// Creates a new adapter for a legacy IDefaultModule
        /// </summary>
        /// <param name="legacyModule">The legacy module to adapt</param>
        /// <param name="outputConverter">Converter function for the output</param>
        public DefaultModuleAdapter(IDefaultModule legacyModule, Func<byte[], TOutput> outputConverter)
        {
            _legacyModule = legacyModule ?? throw new ArgumentNullException(nameof(legacyModule));
            _outputConverter = outputConverter ?? throw new ArgumentNullException(nameof(outputConverter));
        }

        /// <summary>
        /// Loads the legacy module and converts its output
        /// </summary>
        public TOutput Load(byte[] input, string[] args, int index)
        {
            var result = _legacyModule.Load(input, args, index);
            return _outputConverter(result);
        }
    }
    
    /// <summary>
    /// Adapter for legacy IArgumentableModule to work with the generic system
    /// </summary>
    /// <typeparam name="TOutput">Type of output to convert to</typeparam>
    public class ArgumentableModuleAdapter<TOutput> : IArgumentableModule<byte[], byte[], TOutput>
    {
        private readonly IArgumentableModule _legacyModule;
        private readonly Func<byte[], TOutput> _outputConverter;

        /// <summary>
        /// Creates a new adapter for a legacy IArgumentableModule
        /// </summary>
        /// <param name="legacyModule">The legacy module to adapt</param>
        /// <param name="outputConverter">Converter function for the output</param>
        public ArgumentableModuleAdapter(IArgumentableModule legacyModule, Func<byte[], TOutput> outputConverter)
        {
            _legacyModule = legacyModule ?? throw new ArgumentNullException(nameof(legacyModule));
            _outputConverter = outputConverter ?? throw new ArgumentNullException(nameof(outputConverter));
        }

        /// <summary>
        /// Loads the legacy module and converts its output
        /// </summary>
        public TOutput Load(byte[] input, byte[] moduleArgument, string[] args, int index)
        {
            var result = _legacyModule.Load(input, moduleArgument, args, index);
            return _outputConverter(result);
        }
    }

    /// <summary>
    /// Adapter for a generic module to work with different input types
    /// </summary>
    /// <typeparam name="TModuleInput">The module's input type</typeparam>
    /// <typeparam name="TFactoryInput">The factory's input type</typeparam>
    /// <typeparam name="TOutput">Output type</typeparam>
    public class InputConversionAdapter<TModuleInput, TFactoryInput, TOutput> : IDefaultModule<TFactoryInput, TOutput>
    {
        private readonly IDefaultModule<TModuleInput, TOutput> _module;
        private readonly Func<TFactoryInput, TModuleInput> _inputConverter;

        /// <summary>
        /// Creates a new adapter to convert input types
        /// </summary>
        /// <param name="module">The module to adapt</param>
        /// <param name="inputConverter">Converter function for the input</param>
        public InputConversionAdapter(IDefaultModule<TModuleInput, TOutput> module, Func<TFactoryInput, TModuleInput> inputConverter)
        {
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _inputConverter = inputConverter ?? throw new ArgumentNullException(nameof(inputConverter));
        }

        /// <summary>
        /// Converts the input and loads the module
        /// </summary>
        public TOutput Load(TFactoryInput input, string[] args, int index)
        {
            var convertedInput = _inputConverter(input);
            return _module.Load(convertedInput, args, index);
        }
    }
}
