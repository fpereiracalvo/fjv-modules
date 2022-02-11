namespace Fjv.Modules
{
    public interface IDefaultModule : IModule
    {
        byte[] Load(byte[] input, string[] args, int index);
    }
}