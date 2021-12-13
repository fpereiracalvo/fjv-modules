namespace Fjv.Modules
{
    public interface IModule
    {
        bool IsOutput { get; }
        bool IsInput { get; }
        bool IsControlTaker { get; }
        bool NeedArgument { get; }

        byte[] Load(byte[] input, string[] args, int index);
        byte[] Load(byte[] input, byte[] moduleArgument, string[] args, int index);
    }
}