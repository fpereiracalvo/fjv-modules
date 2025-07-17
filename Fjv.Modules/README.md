# Fjv.Modules

A modular framework for building scalable .NET applications.

## Features

- Plug-and-play module architecture
- Dependency injection support
- Easy configuration and extension
- Built-in lifecycle management

## Getting Started

1. **Install the package:**
```bash
dotnet add package Fjv.Modules
```

2. **Add module discovery and execution code:**
```csharp
// Skip the executable name from arguments in .NET 6+
var _args = Environment.GetCommandLineArgs().Skip(1).ToArray();

// Create a factory that discovers modules in the current assembly
IModuleFactory factory = new ModuleFactory(typeof(Program).Assembly);

// Execute modules based on command line arguments
factory.Run(_args);
```

## Documentation

See [Documentation](https://github.com/fpereiracalvo/fjv-modules/tree/main/Documentation) for detailed guides and API reference.

## License

This project is licensed under the MIT License.