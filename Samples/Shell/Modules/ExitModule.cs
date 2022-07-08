using Fjv.Modules;
using Fjv.Modules.Attributes;
using Fjv.Modules.Commons;
using Samples.Shell.Globals;

namespace Samples.Shell.Modules 
{
    [Module("exit", ModuleRunningControl.Unique)]
    public class ExitModule : IDefaultModule
    {
        public byte[] Load(byte[] input, string[] args, int index)
        {
            RunningControl.CancellationToken.Cancel();

            return input;
        }
    }
}