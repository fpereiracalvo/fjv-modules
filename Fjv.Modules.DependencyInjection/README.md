
# Fjv.Modules.DependencyInjection

A lightweight dependency injection module for .NET projects, designed to simplify service registration and resolution.

## Features

- Simple and intuitive API for registering services
- Supports constructor injection
- Lifetime management (transient, scoped, singleton)
- Modular design for easy integration

## Installation

```bash
dotnet add package Fjv.Modules.DependencyInjection
```

## Usage

```csharp
using Fjv.Modules.DependencyInjection;

services.AddModuleFactory(typeof(Program).Assembly);
```

## Documentation

See [Documentation](https://github.com/fpereiracalvo/fjv-modules/tree/main/Documentation) for detailed usage.

## License

This project is licensed under the MIT License.