using Fjv.Modules.Generic;

namespace Fjv.Modules
{
    /// <summary>
    /// Argumentable module interface for legacy compatibility
    /// </summary>
    public interface IArgumentableModule : IModule, IArgumentableModule<byte[], byte[], byte[]>
    {
        // Inherits Load method from IArgumentableModule<byte[], byte[], byte[]>
    }
}