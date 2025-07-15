# What's New in Fjv.Modules

## Latest Features

### Generic Module System

The generic module system allows you to work with strongly-typed inputs and outputs instead of raw `byte[]` arrays:

- Type-safe module interfaces: `IModule<TInput, TOutput>`, `IDefaultModule<TInput, TOutput>`, etc.
- Generic factory creation for type-safe module execution
- Automatic type conversion between standard and generic modules
- Support for custom type converters

[Learn more about generic modules](GenericModules.md)

### Asynchronous Module Support

Full support for asynchronous operations in modules:

- Async interfaces: `IDefaultModuleAsync<TInput, TOutput>`, `IArgumentableModuleAsync<TInput, TArg, TOutput>`
- Async factory methods: `RunAsync`, `GetModuleResultAsync`
- Task-based asynchronous pattern throughout the library

### Enhanced Dependency Injection

Improved integration with Microsoft's dependency injection system:

- Automatic module registration with `AddModuleFactory`
- Support for scoped services in modules
- Multiple assembly scanning options
- Namespace-scoped module discovery

[Learn more about dependency injection](DependencyInjection.md)

### Module Event System

Enhanced event system for monitoring and reacting to module execution:

- `OnModuleExecuting` and `OnModuleExecuted` events
- `OnOptionExecuting` and `OnOptionExecuted` events
- Error handling with the `OnError` event
- Rich event arguments with detailed context information

### Wildcard Module Support

Catch-all modules for handling unknown commands:

- Register a module with the wildcard pattern `"*"`
- Receive unknown arguments directly for custom processing
- Create fallback behavior or help systems

### Documentation and Help System

Improved documentation and in-app help system:

- Module help attributes for rich documentation
- Option help attributes for command details
- Automatic help text generation
