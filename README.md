# Fjv.Modules

[![NuGet](https://img.shields.io/nuget/v/Fjv.Modules.svg)](https://www.nuget.org/packages/Fjv.Modules) [![NuGet](https://img.shields.io/nuget/dt/Fjv.Modules.svg)](https://www.nuget.org/packages/Fjv.Modules/) [![License](https://img.shields.io/github/license/fpereiracalvo/fjv-modules.svg)](LICENSE)

## Support This Project

If you find this library useful in your projects, please consider supporting its development. Your contribution helps maintain and improve Fjv.Modules.

[![Donate](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://paypal.me/fpereiracalvo?country.x=CL&locale.x=en_US)

Fjv.Modules is a powerful library for creating command-line applications with a modular, extensible architecture. Each module is a class that implements specific interfaces and is automatically loaded and executed based on the arguments passed to the application.

## Quick Start

### Basic Usage

```csharp
// Skip the executable name from arguments in .NET 6+
var _args = Environment.GetCommandLineArgs().Skip(1).ToArray();

// Create a factory that discovers modules in the current assembly
IModuleFactory factory = new ModuleFactory(typeof(Program).Assembly);

// Execute modules based on command line arguments
factory.Run(_args);
```

### Using Dependency Injection

```csharp
using Fjv.Modules.DependencyInjection;

// Add the module factory to your services
services.AddModuleFactory(typeof(Program).Assembly);

// Inject and use the factory in your classes
public class MyService
{
    private readonly IModuleFactory _moduleFactory;

    public MyService(IModuleFactory moduleFactory)
    {
        _moduleFactory = moduleFactory;
    }
    
    public void ProcessCommand(string[] args)
    {
        _moduleFactory.Run(args);
    }
}
```

### Using Generic Modules (Type-Safe)

```csharp
// Create a factory that works with string input and output
var factory = GenericModuleExtensions.CreateGenericFactory<string, string>(
    typeof(Program).Assembly
);

// Run the factory with typed input and output
string result = factory.Run(args, "Initial text");
Console.WriteLine($"Result: {result}");
```

## Key Features

- **Modular Command Line Applications**: Build command-line tools with a clean, extensible architecture
- **Type-Safe Operations**: Use generic modules for compile-time type checking and improved developer experience
- **Async First**: Full support for asynchronous operations with task-based patterns
- **Dependency Injection**: Seamless integration with Microsoft's DI container
- **Attribute-Based Configuration**: Use attributes to define modules, options, and help documentation
- **Event System**: Rich events for monitoring and reacting to module execution
- **Wildcard Support**: Special module support for handling unknown commands
- **Help System**: Automatic generation of help messages for modules and options

# Getting Started

## Installation

### Via NuGet Package Manager:
```
Install-Package Fjv.Modules
Install-Package Fjv.Modules.DependencyInjection
```

### Via .NET CLI:
```
dotnet add package Fjv.Modules
dotnet add package Fjv.Modules.DependencyInjection
```

### Source Code
Get the source code from GitHub:
- https://github.com/fpereiracalvo/fjv-modules


# Documentation

## What's New
- [What's new in Fjv.Modules](Documentation/WhatsNew.md) - Learn about the latest features and improvements

## Core Concepts
- [How to use a module factory](Documentation/ModuleFactory.md) - Learn about the central ModuleFactory class
- [Modules interfaces](Documentation/ModuleInterfaces.md) - Understand the available module interfaces
- [Modules attributes](Documentation/ModulesAttributes.md) - Learn about the attributes for configuring modules

## Features
- [Help messages system](Documentation/HelpMessages.md) - Add user-friendly help to your modules and options
- [Dependency injection](Documentation/DependencyInjection.md) - Integrate with Microsoft DI
- [Generic modules](Documentation/GenericModules.md) - Work with typed inputs and outputs
- [Advanced features](Documentation/AdvancedFeatures.md) - Learn about wildcards, event handling, and more

## Sample Applications
See complete example applications in the [fjv-modules-samples](https://github.com/fpereiracalvo/fjv-modules-samples) repository.

# Support This Project

If you find this library useful in your projects, please consider supporting its development. Your contribution helps maintain and improve Fjv.Modules.

[![Donate](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://paypal.me/fpereiracalvo?country.x=CL&locale.x=en_US)