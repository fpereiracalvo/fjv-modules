using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fjv.Modules;
using Microsoft.Extensions.Hosting;

namespace HostWebClientAsync
{
    public class Worker : BackgroundService
    {
        IHost _host;
        IModuleFactory _moduleFactory;
        HostArguments _hostArguments;

        public Worker(IHost host, IModuleFactory moduleFactory, HostArguments hostArguments)
        {
            _host = host;
            _moduleFactory = moduleFactory;
            _hostArguments = hostArguments;

            _moduleFactory.OnError += (sender, e) => {
                Console.WriteLine(e.Exception.Message);

                Environment.Exit(1);
            };

            _moduleFactory.OnModuleExecuting += (sender, e) => {
                Console.WriteLine($"Executing {e.Module.Name}...");
            };

            _moduleFactory.OnOptionExecuting += (sender, e) => {
                Console.WriteLine($"Executing {e.Option.Name}...");
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if(!_hostArguments.HasArgument())
            {
                Console.WriteLine(_moduleFactory.GetHelp());
            }
            else
            {
                await _moduleFactory.RunAsync(_hostArguments.GetArguments());
            }

            await _host.StopAsync();
        }
    }
}