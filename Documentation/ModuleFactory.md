# How to use a module factory

The module factory is based on a interface called IModuleFactory.

This interface define many methods to load modules from assemblies, classes, applying some options and run them.

The most basic run of ModuleFactory is the next:

```csharp
// load module classes automatically.
IModuleFactory moduleFactory = new ModuleFactory(typeof(Program).Assembly);

// and run it.
byte[] buffer = moduleFactory.Run(args);
```

Another basic run is passing an assembly:

```csharp
// load module classes automatically.
IModuleFactory moduleFactory = new ModuleFactory(new Assembly[]{
    typeof(Program).Assembly,
    typeof(CustomLibrary).Assembly,
    typeof(OtherOne).Assembly
});
// and run it.
byte[] buffer = moduleFactory.Run(args);
```

We can pass a class to ModuleFactory. This scope the module scan to the class namespace.

```csharp
// load module classes automatically.
IModuleFactory moduleFactory = new ModuleFactory(typeof(MyClass));
```

When we need apply some options using a ModuleOptions, we can use the next overload:

```csharp
//load modules classes automatically from a third party class with options.
IModuleFactory moduleFactory = new ModuleFactory(typeof(ThirdPartyLibrary), new List<ModuleOptions>{
        new ModuleOptions {
            //set the module type we need to modify.
            ModuleType = typeof(ThirdPartyModule),

            //set a new name to the third party module.
            Name = "-my-option-module"
        }
    });

// and run it.
byte[] buffer = moduleFactory.Run(args);
```

The option affect only one module specified by ModuleType property, and we can apply many options one at time per module.

We can combine the two previous examples:

```csharp
//load modules classes automatically from a local and third party classes with options.
IModuleFactory moduleFactory = new ModuleFactory(new Assembly[]{
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

//and run it.
byte[] buffer = moduleFactory.Run(args);
```

## Events

ModuleFactory has events to control the modules running.

- OnModuleExecuting: Will raise when a module is executing.
- OnModuleExecuted: Will raise when a module is executed.
- OnOptionExecuting: Will raise when an option is executing.
- OnOptionExecuted: Will raise when an option is executed.
- OnError: Will raise when the module throws an exception.

To use the events, we need to subscribe to them before you run the moduleFactory. See the exmaple next:

```csharp
// load module classes automatically.
IModuleFactory moduleFactory = new ModuleFactory(typeof(Program).Assembly);

//subscribe to events.
moduleFactory.OnModuleExecuting += (sender, e) => {
    Console.WriteLine($"Module {e.Module.Name} is executing.");
};

moduleFactory.OnModuleExecuted += (sender, e) => {
    Console.WriteLine($"Module {e.Module.Name} is executed.");
};

moduleFactory.OnOptionExecuting += (sender, e) => {
    Console.WriteLine($"Option {e.Option.Name} is executing.");
};

moduleFactory.OnOptionExecuted += (sender, e) => {
    Console.WriteLine($"Option {e.Option.Name} is executed.");
};

moduleFactory.OnError += (sender, e) => {
    Console.WriteLine($"Error: {e.Exception.Message}");

    // exit the app inmediatelly.
    Environment.Exit(1);
};

// and run it.
byte[] buffer = moduleFactory.Run(args);
```

All events has a sender and a event args to take control over the module running.