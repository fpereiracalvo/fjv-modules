using System.Reflection;
using Fjv.Modules.Generic;

namespace Samples.Shell.Services
{
    public class ShellService
    {
        Assembly _assembly;
        CancellationToken _cancelationToken;

        public ShellService(Assembly assembly, CancellationToken cancelationToken)
        {
            _assembly = assembly;
            _cancelationToken = cancelationToken;
        }

        public async Task BeginAsync()
        {
            WelcomeMessage();

            while (!_cancelationToken.IsCancellationRequested)
            {
                var moduleFactory = new ModuleFactory<string, string>(_assembly);

                Console.Write("❯ ");

                var input = Console.ReadLine();

                var args = input?.Split(' ') ?? new string[]{};

                if(args.Any())
                {
                    try
                    {
                        await moduleFactory.RunAsync(args, string.Empty, _cancelationToken);

                        //do something more.
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR: {ex.Message}");
                    }
                }
            }
        }

        public void WelcomeMessage()
        {
            Console.WriteLine("Welcome to Shell application sample.");
        }
    }
}