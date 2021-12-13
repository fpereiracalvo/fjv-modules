# General

Fjv.Modules is a library that offering a pattern to binding classes and methods that will be activated through program command-line argument read.

# Getting started

Install the library.

Your class must be implement IModule interface. IModule interface has basics properties and methods neceseries to load and control de execution of the module.

Bind your module class with Module attribte and give it a name, like the example below:

```csharp
[Module("-print")]
public class PrintModule : IModule
{
    CustomObject _content;

    //intentionaly omitted.

    public byte[] Load(byte[] input, string[] args, int index)
    {
        _content = SomeCustomObjectLoader(input);

        return input;
    }

    public byte[] Load(byte[] input, byte[] moduleArgument, string[] args, int index)
    {
        throw new NotImplementedException();
    }
}
```
You have created the **-print** module to the program.

The input and output must be byte array content. The reason why resides to ensure the data will passing from module to anothee without type incompatibilities.
All methods could been an option of the command-line argument. For this, you must be decorated this with Option attribute.

```csharp
[Module("-print")]
public class PrintModule : IModule
{
    //intentionaly omitted.

    [Option("--screen")]
    public byte[] PrintScreen()
    {
        Console.WriteLine(_content.ToString());
    }

    [Option("--file")]
    public byte[] SaveFile(string filename)
    {
        _content.Save(filename);
    }
}
```
You have created the **--screen** and **--file** options to the **-print** module.

The parameters of the options are taken from the command-line argument and it's converted to the correspond Type automatically.

```shell
mycommnand sample.txt --print "hello world!" --file filename.txt
```

If the method has many parameters so they must be separated by commas:

```shell
mycommnand sample.txt --print --someOption parameter1,parameter2,parameter3
```

The parameters of the option must have the same quantity of parameters of the binded method.

# Code sample

The most basic implementation of the module factory it's look like:

```csharp
using System;
using System.Linq;
using Fjv.Modules;

namespace SomeExample
{
    class Program
    {
        static void Main(string[] args)
        {
            if(!args.Any())
            {
                return;
            }
            
            var moduleFactory = new ModuleFactory(typeof(Program).Assembly);

            var buffer= moduleFactory.Run(args);
        }
    }
}
```

Or the next below to take the modules from differents assemblies:
```csharp
using System;
using System.Linq;
using Fjv.Modules;

namespace SomeExample
{
    class Program
    {
        static void Main(string[] args)
        {
            if(!args.Any())
            {
                return;
            }
            
            var moduleFactory = new ModuleFactory(new Assembly[]{
                typeof(Program).Assembly,
                typeof(CustomLibrary).Assembly,
                typeof(OtherOne).Assembly
            });

            var buffer= moduleFactory.Run(args);
        }
    }
}
```

