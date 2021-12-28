namespace Fjv.Modules
{
    public interface IModule
    {
        byte[] Load(byte[] input, string[] args, int index);
        byte[] Load(byte[] input, byte[] moduleArgument, string[] args, int index);
    }
}