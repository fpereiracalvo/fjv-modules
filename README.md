
Fjv.Modules is a library to create clean command-line applications.

Default use

```csharp
var _args = Environment.GetCommandLineArgs().Skip(1).ToArray(); // skiping argument on .Net6.

IModuleFactory factory = new ModuleFactory(typeof(Program).Assembly);

factory.Run(_args);
```

With dependency injection add a scope of IModuleFactory.

```csharp
using Fjv.Modules.DependencyInjection;

//intentionally omitted.

services.AddModuleFactory(typeof(Program).Assembly);
```

# Getting started

- Install the library from NuGet:
  - https://www.nuget.org/packages/Fjv.Modules
  - https://www.nuget.org/packages/Fjv.Modules.DependencyInjection
- Get the source code from GitHub
  - https://github.com/fpereiracalvo/fjv-modules
- Donations
  - If you think Fjv.Modules has been useful to you, I invite you to make a donation in the amount you want on [Paypal](https://paypal.me/fpereiracalvo?country.x=CL&locale.x=en_US). I will be very grateful for your contribution.

# How to use

Yo need a module factory instance to scan an assembly
```csharp
// load module classes automatically.
IModuleFactory moduleFactory = new ModuleFactory(typeof(Program).Assembly);
```

assemblies
```csharp
//intentionaly omitted.
var moduleFactory = new ModuleFactory(new Assembly[]{
    typeof(Program).Assembly,
    typeof(CustomLibrary).Assembly,
    typeof(OtherOne).Assembly
});
```

or a class
```csharp
//intentionaly omitted.

IModuleFactory moduleFactory = new ModuleFactory(typeof(MyClass));
```

and run it passing the arguments. In .Net 6 you must skip the first one.
```csharp
byte[] buffer = moduleFactory.Run(args);
```

## Implementation

### Modules

All modules are an implementation of IModule.

IModule interface has two interfaces dependencies to implement that will be useful to ModuleFactory:

* IDefaultModule.
* IArgumentableModule.

### Sample

The next sample shows how whould define a simple argumentable module.

```csharp
[Module("-print")]
[ModuleHelp("Print module help message.")]
public class PrintModule : IArgumentableModule
{
    string _content;

    public byte[] Load(byte[] input, byte[] moduleArgument, string[] args, int index)
    {
        _content = MyReader(moduleArgument);

        return input;
    }

    [Option("--screen")]
    [OptionHelp("Screen option help message.")]
    public byte[] PrintScreen()
    {
        // print text on to screen.
        Console.WriteLine(_content);

        return _content.ToByteArray();
    }
}
```

And how to use:

> myconsoleapp -print myfile.txt --screen

You can add help message to all modules and options. Those can show by GetHelp from a module factory. See Sample projects.

The next sample is how to use a default module.

```csharp
[Module("-calc")]
public class CalcModule : IDefaultModule
{
    public byte[] Load(byte[] input, string[] args, int index)
    {
        return input;
    }

    [Option("--sum", true)]
    public byte[] PrintScreen(int a, int b)
    {
        // print text on to screen.
        Console.WriteLine($"{a}+{b} = {a+b}");

        return new bytep[]{};
    }
}
```

So, you can run.

> myconsoleapp -calc --sum 1 2

See more in Samples folder.

## Third party modules

You can add thirparty module libraries and set a new name to use into your console app.

```csharp
IModuleFactory moduleFactory = new ModuleFactory(new Assembly[]{
    typeof(Program).Assembly,
    typeof(CustomLibrary).Assembly,
    typeof(OtherOnde).Assembly
}, new List<ModuleOptions>{
    new ModuleOptions {
        ModuleType = typeof(ThirdPartyModule),
        Name = "-my-option-module"  //set new name
    }
});

var buffer= moduleFactory.Run(args);
```

Add a scope of IModuleFactory in the same manner using dependency injection extension.

```csharp
using Fjv.Modules.DependencyInjection;

//intentionally omitted.

services.AddModuleFactory(new Assembly[]{
    typeof(Program).Assembly,
    typeof(CustomLibrary).Assembly,
    typeof(OtherOnde).Assembly
}, new List<ModuleOptions>{
    new ModuleOptions {
        ModuleType = typeof(ThirdPartyModule),
        Name = "-my-option-module"  //set new name
    }
});
```

## Events

The ModuleFactory events give you control over the modules runing.

- OnModuleExecuting: Will raise when a module is executing.
- OnModuleExecuted: Will raise when a module is executed.
- OnOptionExecuting: Will raise when an option is executing.
- OnOptionExecuted: Will raise when an option is executed.
- OnError: Will raise when the module throws an exception.

See more on sample projects.