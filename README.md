# Fjv.Modules

[![NuGet](https://img.shields.io/nuget/v/Fjv.Modules.svg)](https://www.nuget.org/packages/Fjv.Modules/) [![NuGet](https://img.shields.io/nuget/dt/Fjv.Modules.svg)](https://www.nuget.org/packages/Fjv.Modules/) [![License](https://img.shields.io/github/license/fpereiracalvo/fjv-modules.svg)](LICENSE)

With this library you can create a command-line application with a modular architecture. Each module is a class that implements interfaces, and it loaded and executed by the arguments passed to the application.

```csharp
var _args = Environment.GetCommandLineArgs().Skip(1).ToArray(); // skiping argument on .Net6.

IModuleFactory factory = new ModuleFactory(typeof(Program).Assembly);

factory.Run(_args);
```

```csharp

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


# Documentation

- [How to use a module factory](Documentation/ModuleFactory.md)
- [Modules interfaces](Documentation/ModuleInterfaces.md)
- [Modules attributes](Documentation/ModulesAttributes.md)
- [Modules and options help messages](Documentation/HelpMessages.md)
- [Dependency injection](Documentation/DependencyInjection.md)

# Donations

Plase, consider a donation to support this project.

Donation is as per your goodwill. I will very much appreciate your donation.

[![](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://paypal.me/fpereiracalvo?country.x=CL&locale.x=en_US)