using Fjv.Modules.Generic;

namespace Fjv.Modules
{
    /// <summary>
    /// Default module interface for legacy compatibility
    /// </summary>
    public interface IDefaultModule : IModule, IDefaultModule<byte[], byte[]>
    {
        // Inherits Load method from IDefaultModule<byte[], byte[]>
    }
}