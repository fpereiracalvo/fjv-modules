using System.Threading;
using Samples.Shell.Services;
using System;
using Samples.Shell.Globals;

namespace Samples.Shell
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var shell = new ShellService(typeof(Program).Assembly, RunningControl.CancellationToken.Token);

            shell.Begin();
        }
    }
}