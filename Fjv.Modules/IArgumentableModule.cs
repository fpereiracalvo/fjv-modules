namespace Fjv.Modules
{
    public interface IArgumentableModule : IModule
    {
        byte[] Load(byte[] input, byte[] moduleArgument, string[] args, int index);
    }
}