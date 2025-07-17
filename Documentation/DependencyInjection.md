# Dependency Injection

Fjv.Modules.DependencyInjection is a library that provides dependency injection support for Fjv.Modules.

To use the dependency injection extension for Fjv.Modules, you need to install the NuGet package [Fjv.Modules.DependencyInjection](https://www.nuget.org/packages/Fjv.Modules.DependencyInjection).

## Standard Module Factory

Add services to the dependency injection container using the extension method `AddModuleFactory` from `IServiceCollection`.

This injection preparation adds a scoped registration of `IModuleFactory` to the dependency injection container.

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

## Generic Module Factory

For type-safe operations, you can register generic module factories that work with specific input and output types. Use the extension method `AddModuleFactory<TIn, TOut>` from `IServiceCollection`.

```csharp
using Fjv.Modules.DependencyInjection;

// prepare host builder.
var hostBuilder = Host.CreateDefaultBuilder(args);

// add services to the container.
hostBuilder.ConfigureServices((hostContext, services) =>
{
    // add some worker.
    services.AddHostedService<CustomStringWorker>();
    
    // add generic module factory for string operations
    services.AddModuleFactory<string, string>(typeof(Program).Assembly);

    // add other services...
});

// build host.
var host = hostBuilder.Build();

// and run the worker
await host.RunAsync();
```

Just like with standard module factories, you can specify assemblies, namespace scoping, and options:

```csharp
// register a generic module factory with multiple assemblies
services.AddModuleFactory<string, string>(
    new Assembly[] {
        typeof(Program).Assembly,
        typeof(ExternalLibrary).Assembly
    },
    new List<ModuleOptions> {
        new ModuleOptions {
            ModuleType = typeof(StringProcessorModule),
            Name = "-process-text"
        }
    });

// or with namespace scoping
services.AddModuleFactory<string, string>(
    typeof(TextProcessingNamespace),
    options);
```

To use the generic module factory in your services, inject `IModuleFactory<TInput, TOutput>`:

```csharp
public class CustomStringWorker : BackgroundService
{
    private readonly IModuleFactory<string, string> _moduleFactory;

    public CustomStringWorker(IModuleFactory<string, string> moduleFactory)
    {
        _moduleFactory = moduleFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // run the module factory with string input and get string output
        string result = await _moduleFactory.RunAsync(args, "Initial text", stoppingToken);
        
        // process the result...
        Console.WriteLine($"Result: {result}");
    }
}
```

## Using Both Standard and Generic Module Factories

You can register both standard and generic module factories in the same application:

```csharp
services.AddModuleFactory(typeof(Program).Assembly);
services.AddModuleFactory<string, string>(typeof(Program).Assembly);
services.AddModuleFactory<HttpRequestMessage, HttpResponseMessage>(typeof(ApiModules).Assembly);
```

This allows you to:
- Use legacy modules with the standard `IModuleFactory`
- Process string data with `IModuleFactory<string, string>`
- Handle HTTP operations with `IModuleFactory<HttpRequestMessage, HttpResponseMessage>`

Each factory type is registered as a separate service, so you can inject the specific one you need into your classes.
