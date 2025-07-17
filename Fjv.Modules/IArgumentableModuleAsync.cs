using System.Threading.Tasks;
using Fjv.Modules.Generic;

namespace Fjv.Modules
{
    /// <summary>
    /// Argumentable asynchronous module interface for legacy compatibility
    /// </summary>
    public interface IArgumentableModuleAsync : IModule, IArgumentableModuleAsync<byte[], byte[], byte[]>
    {
        // Inherits LoadAsync method from IArgumentableModuleAsync<byte[], byte[], byte[]>
    }
}