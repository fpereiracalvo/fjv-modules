using System.Threading.Tasks;
using Fjv.Modules.Generic;

namespace Fjv.Modules
{
    /// <summary>
    /// Default asynchronous module interface for legacy compatibility
    /// </summary>
    public interface IDefaultModuleAsync : IModule, IDefaultModuleAsync<byte[], byte[]>
    {
        // Inherits LoadAsync method from IDefaultModuleAsync<byte[], byte[]>
    }
}