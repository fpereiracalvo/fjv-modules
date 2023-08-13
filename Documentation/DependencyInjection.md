# Dependency injection

Fjv.Modules.DependencyInjection is a library that provide dependency injection to Fjv.Modules.

To use the dependency injection extension for Fjv.Modules, we need to install the NuGet package [Fjv.Modules.DependencyInjection](https://www.nuget.org/packages/Fjv.Modules.DependencyInjection).

Add service to the dependency injection container using the extension method `AddModuleFactory` from `IServiceCollection`.

This injection preparation add a scope of `IModuleFactory` to the dependency injection container.

```csharp
using Fjv.Modules.DependencyInjection;

// prepare host builder.
var hostBuilder = Host.CreateDefaultBuilder(args);

// add services to the container.
hostBuilder.ConfigureServices((hostContext, services) =>
{
    // add some worker.
    services.AddHostedService<CustomWorker>();
    // add module factory.
    services.AddModuleFactory(typeof(Program).Assembly);

    // add other services...
});

// build host.
var host = hostBuilder.Build();

// and run the worker
await host.RunAsync();
```

We can specified the assembly, assemblies, and a class to scan the modules and even add options as the same way we create a ModuleFactory.

```csharp
// ommitted code...

hostBuilder.ConfigureServices((hostContext, services) =>
{
    // add some worker.
    services.AddHostedService<CustomWorker>();
    // add module factory.
    services.AddModuleFactory(new Assembly[]{
            typeof(Program).Assembly,
            typeof(CustomLibrary).Assembly,
            typeof(OtherOne).Assembly,
            typeof(ThirdPartyLibrary).Assembly
        }, new List<ModuleOptions>{
                new ModuleOptions {
                    //set the module type we need to modify.
                    ModuleType = typeof(ThirdPartyModule),

                    //set a new name to the third party module.
                    Name = "-my-option-module"
                }
            });

    // add other services...
});

// ommitted code...
```

To use the module factory in a worker, we need to inject the `IModuleFactory` in the worker constructor. See sample below:

```csharp
using Fjv.Modules.DependencyInjection;

public class CustomWorker : BackgroundService
{
    private readonly IModuleFactory _moduleFactory;

    public CustomWorker(IModuleFactory moduleFactory)
    {
        _moduleFactory = moduleFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // run the module factory.
        byte[] buffer = await _moduleFactory.RunAsync(args);
    }
}
```

As a sample, we subscribe the events of `IModuleFactory` into the worker instance to handle it.

```csharp
using Fjv.Modules.DependencyInjection;

public class CustomWorker : BackgroundService
{
    private readonly IModuleFactory _moduleFactory;

    public CustomWorker(IModuleFactory moduleFactory)
    {
        _moduleFactory = moduleFactory;

        // suscribe events.
        _moduleFactory.ModuleStarted += ModuleFactory_ModuleStarted;
        _moduleFactory.ModuleFinished += ModuleFactory_ModuleFinished;
        _moduleFactory.ModuleError += ModuleFactory_ModuleError;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // run the module factory.
        byte[] buffer = await _moduleFactory.RunAsync(args);
    }

    private void ModuleFactory_ModuleError(object sender, ModuleErrorEventArgs e)
    {
        // handle error.
    }

    private void ModuleFactory_ModuleFinished(object sender, ModuleFinishedEventArgs e)
    {
        // handle finished.
    }

    private void ModuleFactory_ModuleStarted(object sender, ModuleStartedEventArgs e)
    {
        // handle started.
    }
}
```
