# Modules and options help messages

We can show help messages for modules and options. The help messages are defined by ModuleHelp and OptionHelp attributes.

The next sample shows how to define help messages:

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

To show all help messages we need to use the next code:

```csharp
// load module classes automatically...
// validate the arguments and show help messages.

// show the help message.
moduleFactory.GetHelp();
```

If we just show the help message based on arguments, so we need to use the next code:

```csharp
// load module classes automatically...
// validate the arguments and show help messages.

// show the help message by arguments.
moduleFactory.GetHelp(args);
```