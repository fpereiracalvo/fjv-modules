# Advanced Features

This document covers advanced features and patterns for working with Fjv.Modules.

## Wildcard Modules

Wildcard modules allow you to catch any arguments that don't match specific modules. To create a wildcard module, use the `"*"` module name:

```csharp
[Module("*")]
[ModuleHelp("Handles any unknown command")]
public class WildcardModule : IArgumentableModule
{
    public byte[] Load(byte[] input, byte[] moduleArgument, string[] args, int index)
    {
        string command = System.Text.Encoding.UTF8.GetString(moduleArgument);
        Console.WriteLine($"Unknown command: {command}");
        return input;
    }
}
```

## Chaining Modules

You can create processing pipelines by chaining modules, where the output of one module becomes the input of the next:

```csharp
// Pass multiple modules as command line arguments
// Example: app -read-file --file text.txt -process-text --uppercase -write-file --file output.txt
// Each module processes and passes data to the next one
```

## Custom Event Handling

The ModuleFactory provides events that allow you to monitor and react to module execution:

```csharp
IModuleFactory factory = new ModuleFactory(typeof(Program).Assembly);

// Monitor module execution
factory.OnModuleExecuting += (sender, e) => {
    Console.WriteLine($"Starting module: {e.Module.Name}");
    // You can access properties like e.Module, e.Args, e.Index
};

factory.OnModuleExecuted += (sender, e) => {
    Console.WriteLine($"Finished module: {e.Module.Name}");
    // You can check e.Output for the result
};

// Monitor option execution
factory.OnOptionExecuting += (sender, e) => {
    Console.WriteLine($"Starting option: {e.MethodName}");
};

factory.OnOptionExecuted += (sender, e) => {
    Console.WriteLine($"Finished option: {e.MethodName}");
};

// Handle errors
factory.OnError += (sender, e) => {
    Console.WriteLine($"Error in {e.Module.Name}: {e.Exception.Message}");
    // You can implement custom error handling here
};
```

## Custom Module Resolution

You can override how modules are resolved by creating a custom ModuleFactory:

```csharp
public class CustomModuleFactory : ModuleFactory 
{
    public CustomModuleFactory(Assembly assembly) : base(assembly) { }
    
    protected override Type GetModuleType(string moduleName)
    {
        // Custom module resolution logic
        if (moduleName.StartsWith("-special:"))
        {
            // Special handling for certain module formats
            return base.GetModuleType(moduleName.Substring(9));
        }
        
        return base.GetModuleType(moduleName);
    }
}
```

## Integration with Configuration Systems

You can integrate with configuration systems like Microsoft.Extensions.Configuration:

```csharp
public class ConfigurableModule : IDefaultModule<string, string>
{
    private readonly IConfiguration _config;
    
    public ConfigurableModule(IConfiguration config)
    {
        _config = config;
    }
    
    public string Load(string input, string[] args, int index)
    {
        // Access configuration values
        string setting = _config["App:SomeSetting"];
        return $"{input} - {setting}";
    }
}
```

## Working with Streams

For processing large files or streams efficiently:

```csharp
[Module("-stream-processor")]
public class StreamProcessorModule : IDefaultModule<Stream, Stream>
{
    private Stream _inputStream;
    
    public Stream Load(Stream input, string[] args, int index)
    {
        _inputStream = input;
        return _inputStream;
    }
    
    [Option("--transform")]
    public Stream Transform()
    {
        var outputStream = new MemoryStream();
        
        // Process the stream without loading it entirely into memory
        _inputStream.CopyTo(outputStream);
        outputStream.Position = 0;
        
        return outputStream;
    }
}
```

## Best Practices

1. **Module Granularity**: Create focused modules that do one thing well rather than monolithic modules.
2. **Error Handling**: Implement proper error handling in modules to ensure robust applications.
3. **Logging**: Use the event system to implement logging of module execution.
4. **Testing**: Create unit tests for modules to ensure they work correctly.
5. **Documentation**: Use the Help attributes to document your modules and options.
