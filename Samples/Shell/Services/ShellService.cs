using System;
using System.Reflection;
using Fjv.Modules;
using System.Threading;
using System.Linq;

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

        public void Begin()
        {
            byte[] buffer = new byte[]{};

            WelcomeMessage();

            while (!_cancelationToken.IsCancellationRequested)
            {
                var moduleFactory = new ModuleFactory(_assembly);

                Console.Write("❯ ");

                var input = Console.ReadLine();

                var args = input?.Split(' ') ?? new string[]{};

                if(args.Any())
                {
                    try
                    {
                        buffer = moduleFactory.Run(args, buffer);

                        //do something with buffer.
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