# General

Fjv.Modules is a library that offer a pattern to bind classes and methods that will be activated by command-line argument.

Just put or remove the attribute to change the command-line program behavior without dealing with complicated manual binding.

# Getting started

Install this library from NuGet https://www.nuget.org/packages/Fjv.Modules/ or download the code source from https://github.com/fpereiracalvo/fjv-modules.

## Implementation
The class must be implement IModule interface. IModule interface has thow methods neceseries to load the module.

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

The input and output must be a byte array content. The reason why resides to ensure the data will be passing from module to another independently manner.
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
myprogram --print "hello world!" --file filename.txt
```

If the method has many parameters so they must be separated by commas:

```shell
myprogram --print --someOption parameter1,parameter2,parameter3
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

            var buffer = moduleFactory.Run(args);
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

# Running control

It's posibble give running control to the modules using ModuleRunningControl enum on Module attribute.

* Input: it mark the module as an input. The module and options will be run in first place before the rest of modules encountered.
* Output: it mark the module as an output The module and options will be run in last place.
* ControlTaker: it mark the module as control taker. This module return immediately a result.
* RequireArgument: it mark the module to take aditional input argument. Run the method byte[] Load(byte[] input, byte[] moduleArgument, string[] args, int index). passing the content bytes to moduleArgument.
* Unique: it mark the module to doesn't attach more than one time. The module factory will throw an Exception if occur.

You can combine all enums, except Input and Output.

# Features

## Wildcard

This library allow to you using the "*" (asterisk) wildcard onto Module attribute. It useful to take unknow strings as inputs for your program.

```csharp
namespace SomeExample
{
    [Module("*", ModuleRunningControl.Input)]
    public class OpenFileModule : IModule
    {
        public byte[] Load(byte[] input, string[] args, int index)
        {
            var filename = System.Text.Encoding.UTF8.GetString(input);

            if(!System.IO.File.Exists(filename))
            {
                Console.WriteLine($"{filename} cannot open.");
                
                Environment.Exit(1);
            }

            Fjv.Images.Globals.Inputs.CurrentFileName = filename;

            return System.IO.File.ReadAllBytes(filename);
        }

        public byte[] Load(byte[] input, byte[] moduleArgument, string[] args, int index)
        {
            // this method will not run.

            throw new NotImplementedException();
        }
    }
}
```

The result is:
```shell
myprogram /somepath/sample.txt
```

## Control taker

The next sample shows how could you use the control taker.

```csharp
namespace SomeExample
{
    // run the command-line argument over each file in the directory.
    [Module("files",  ModuleRunningControl.Input |  ModuleRunningControl.ControlTaker |  ModuleRunningControl.RequireArgument)]
    public class OpenDirectoryFilesModule : IModule
    {
        public byte[] Load(byte[] input, string[] args, int index)
        {
            // this method will not run.

            throw new NotImplementedException();
        }

        public byte[] Load(byte[] input, byte[] moduleArgument, string[] args, int index)
        {
            //take a path folder string.
            var inputString = System.Text.Encoding.UTF8.GetString(moduleArgument);

            var path = System.IO.Path.GetDirectoryName(inputString) + System.IO.Path.DirectorySeparatorChar;
            var search = inputString.Length > path.Length ? inputString.Replace(path, "") : "";

            string[] files = null;

            if(!string.IsNullOrWhiteSpace(search))
            {
                files = System.IO.Directory.GetFiles(path, search).Select(s => System.IO.Path.GetFileName(s)).ToArray();
            }
            else
            {
                files = System.IO.Directory.GetFiles(path).Select(s => System.IO.Path.GetFileName(s)).ToArray();
            }

            // take the arguments to run over each file.
            var auxArgs = args.Take(index).ToArray().Concat(args.Skip(index+2).ToArray()).ToArray();

            // initialize an internal module factory.
            var moduleFactory = new ModuleFactory(typeof(Program).Assembly);

            foreach (var filepath in files)
            {
                var filename = System.IO.Path.Combine(path, filepath);
                Fjv.Images.Globals.Inputs.CurrentFileName = filename;
                
                var file = System.IO.File.ReadAllBytes(filename);

                moduleFactory.Run(auxArgs, file);
            }

            return null;
        }
    }
}
```

As you see. It's possible run the module factory even inside an other module.
