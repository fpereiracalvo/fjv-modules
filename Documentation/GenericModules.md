# Generic Modules

Generic modules allow you to work with specific data types instead of using `byte[]` for input and output. This provides type safety and better integration with your application's data structures.

## Generic Interfaces

The available generic interfaces are:

- `IModule<TInput, TOutput>`: Base interface for all generic modules.
- `IDefaultModule<TInput, TOutput>`: For standard generic modules.
- `IArgumentableModule<TInput, TArg, TOutput>`: For generic modules that accept direct arguments.
- `IDefaultModuleAsync<TInput, TOutput>`: For asynchronous generic modules.
- `IArgumentableModuleAsync<TInput, TArg, TOutput>`: For asynchronous generic modules that accept direct arguments.

## Usage Examples

### Defining a Generic Module

```csharp
[Module("-string-processor")]
[ModuleHelp("Process string data with various operations")]
public class StringProcessorModule : IDefaultModule<string, string>
{
    private string _content;

    public string Load(string input, string[] args, int index)
    {
        _content = input ?? string.Empty;
        Console.WriteLine($"Loaded string content: {_content}");
        return _content;
    }

    [Option("--upper")]
    [OptionHelp("Convert the string to uppercase")]
    public string ToUpper()
    {
        return _content.ToUpper();
    }
    
    [Option("--lower")]
    [OptionHelp("Convert the string to lowercase")]
    public string ToLower()
    {
        return _content.ToLower();
    }
}
```

### Creating a Generic Factory

```csharp
// Create a factory that works with strings for both input and output
var factory = GenericModuleExtensions.CreateGenericFactory<string, string>(typeof(Program).Assembly);

// Execute the factory with initial text
string result = factory.Run(args, "Initial Text");
Console.WriteLine($"Result: {result}");
```

### Using Different Input and Output Types

```csharp
// Create a factory that accepts a Person object as input and returns a JSON string
var personFactory = GenericModuleExtensions.CreateGenericFactory<Person, string>(typeof(Program).Assembly);

// Run with a Person object
var person = new Person { Name = "John", Age = 30 };
string json = personFactory.Run(args, person);
```

## Compatibility with Existing Modules

The original interfaces (`IDefaultModule`, `IArgumentableModule`, etc.) now inherit from the generic interfaces using `byte[]` as the type, ensuring compatibility with existing code.

### Adapting Existing Modules to the Generic System

```csharp
// Adapt an existing module to work with strings
IDefaultModule legacyModule = new MyExistingModule();
IDefaultModule<byte[], string> adaptedModule = legacyModule.AsGeneric<string>();

// You can also provide your own converter
IDefaultModule<byte[], CustomType> customAdaptedModule = legacyModule.AsGeneric<CustomType>(
    bytes => new CustomType(bytes) // Custom converter
);
```

## Custom Converters

The system allows you to define custom converters to transform between different data types:

```csharp
// Define a custom converter
Func<byte[], Person> personConverter = bytes => 
{
    var json = System.Text.Encoding.UTF8.GetString(bytes);
    return System.Text.Json.JsonSerializer.Deserialize<Person>(json);
};

// Use the converter with an existing module
var personModule = legacyModule.AsGeneric<Person>(personConverter);
```

## Working with Asynchronous Generic Modules

```csharp
[Module("-async-data-processor")]
[ModuleHelp("Process data asynchronously")]
public class AsyncDataProcessor : IDefaultModuleAsync<string, string>
{
    private string _data;

    public async Task<string> LoadAsync(string input, string[] args, int index)
    {
        _data = input ?? string.Empty;
        await Task.Delay(100); // Simulating some async work
        return _data;
    }

    [Option("--process")]
    [OptionHelp("Process the data asynchronously")]
    public async Task<string> ProcessAsync()
    {
        await Task.Delay(500); // Simulating processing
        return $"Processed: {_data}";
    }
}

// Using the async module
var asyncFactory = GenericModuleExtensions.CreateGenericAsyncFactory<string, string>(
    typeof(Program).Assembly
);

string result = await asyncFactory.RunAsync(args, "Initial data");
```

## Important Notes

1. Options in generic modules must return the same type as the module's output type.
2. For complex types, it's recommended to implement appropriate converters.
3. The library provides default converters for common types like `string` and `byte[]`.
4. Generic modules provide compile-time type safety, reducing the need for type casting.
5. You can mix regular and generic modules in the same application.
6. Type conversions are handled automatically by the library when possible.
