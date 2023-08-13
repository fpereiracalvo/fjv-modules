# Module interfaces

## Default

The default modules interfaces are:

- IDefaultModule
- IDefaultModuleAsync

This interfaces define the most basic method for the modules. This method allow us execute any business what we will need, perhaps initilization of the module before execute the options or execute some an action.

The IDefaultModuleAsync define LoadAsync method for asynchronous execution.

## Argumentable

The argumentable interfaces are:

- IArgumentableModule
- IArgumentableModuleAsync

This interfaces define the same basic method, but with a little bit change. That can receive an argument by moduleArgument parameter to use as direct argument when the module it is used.

## Arguments of Load method

The Load method of the modules can receive the next arguments:
- input: The input of the module. This is the output of the previous module.
- moduleArgument: The argument of the module. This is the argument of the module when it is used with a direct argument.
- args: The arguments of the console application.
- index: The index of the module in the console application arguments.

## Implementations

### Implementation of IDefaultModule
    
```csharp
[Module("-print")]
[ModuleHelp("Print module help message.")]
public class PrintModule : IDefaultModule
{
    string _content;

    public byte[] Load(byte[] input, string[] args, int index)
    {
        _content = MyReader(args[index]);

        return input;
    }

    [Option("--screen")]
    [OptionHelp("Screen option help message.")]
    public byte[] PrintScreen()
    {
        return SomeMethod(_content);
    }
}
```

### Implementation of IDefaultModuleAsync
    
```csharp
[Module("-print")]
[ModuleHelp("Print module help message.")]
public class PrintModule : IDefaultModuleAsync
{
    string _content;

    public async Task<byte[]> LoadAsync(byte[] input, string[] args, int index)
    {
        return await Task.Run()
        {
            _content = MyReader(args[index]);

            return input;
        };
    }

    [Option("--screen")]
    [OptionHelp("Screen option help message.")]
    public async Task<byte[]> PrintScreenAsync()
    {
        return await SomeAsyncMethod(_content);
    }
}
```

### Implementation of IArgumentableModule
    
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

### Implementation of IArgumentableModuleAsync
    
```csharp
[Module("-print")]
[ModuleHelp("Print module help message.")]
public class PrintModule : IArgumentableModuleAsync
{
    string _content;

    public async Task<byte[]> LoadAsync(byte[] input, byte[] moduleArgument, string[] args, int index)
    {
        return await Task.Run()
        {
            _content = MyReader(moduleArgument);

            return input;
        };
    }

    [Option("--screen")]
    [OptionHelp("Screen option help message.")]
    public async Task<byte[]> PrintScreenAsync()
    {
        return await SomeAsyncMethod(_content);
    }
}
```
