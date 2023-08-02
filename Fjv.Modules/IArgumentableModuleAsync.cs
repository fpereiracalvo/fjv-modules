using System.Threading.Tasks;

namespace Fjv.Modules
{
    public interface IArgumentableModuleAsync : IModule
    {
        Task<byte[]> LoadAsync(byte[] input, byte[] moduleArgument, string[] args, int index);
    }
}