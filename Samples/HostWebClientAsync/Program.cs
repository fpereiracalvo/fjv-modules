using Microsoft.Extensions.Hosting;
using Fjv.Modules.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Fjv.Modules;
using Microsoft.Extensions.Configuration;
using HostWebClientAsync;

IConfiguration configuration = new ConfigurationBuilder()
   .AddJsonFile("appsettings.json", true,true)
   .AddJsonFile("appsettings.Development.json", true,true)
   .Build();

IHost _host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => {
        services.AddSingleton<IConfiguration>(configuration);
        services.AddModuleFactory(typeof(Program).Assembly);
        services.AddScoped<HttpClientFactory>();
        services.AddSingleton<HttpClient>((services)=>{
            var httpClientFactory = services.GetService<HttpClientFactory>();

            return httpClientFactory.GetHttpClient();
        });

        services.AddScoped<HostArguments>();
        services.AddHostedService<Worker>();
    })
    .Build();

await _host.RunAsync();
