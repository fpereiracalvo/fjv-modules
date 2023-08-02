using System.Threading.Tasks;

namespace Fjv.Modules
{
    public interface IDefaultModuleAsync : IModule
    {
        Task<byte[]> LoadAsync(byte[] input, string[] args, int index);
    }
}