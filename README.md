
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


# Documentation

- [How to use a module factory](Documentation/ModuleFactory.md)
- [Modules interfaces](Documentation/ModuleInterfaces.md)
- [Modules attributes](Documentation/ModulesAttributes.md)
- [Modules and options help messages](Documentation/HelpsMessages.md)
