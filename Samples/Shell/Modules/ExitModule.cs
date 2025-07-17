using Fjv.Modules.Generic;
using Fjv.Modules.Attributes;
using Fjv.Modules.Commons;
using Samples.Shell.Globals;

namespace Samples.Shell.Modules 
{
    [Module("exit", ModuleRunningControl.Unique)]
    public class ExitModule : IDefaultModuleAsync<string, string>
    {
        public async Task<string> LoadAsync(string input, string[] args, int index, CancellationToken cancellationToken = default)
        {
            RunningControl.CancellationToken.Cancel();

            return await Task.FromResult(input);
        }
    }
}