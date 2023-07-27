using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fjv.Modules;
using Microsoft.Extensions.Hosting;

namespace HostWebClient
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
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _moduleFactory.Run(_hostArguments.GetArguments());

            await _host.StopAsync();
        }
    }
}