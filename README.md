# General

Fjv.Modules helps you to create command-line applications using attributes to set classes and methods as modules and options.

```csharp
// load module classes automatically.
ModuleFactory moduleFactory = new ModuleFactory(typeof(Program).Assembly);

// run arguments command-line.
byte[] buffer = moduleFactory.Run(args);
```

# Getting started

Install the library from NuGet.
* https://www.nuget.org/packages/Fjv.Modules/

You can see the code source from GitHub.
* https://github.com/fpereiracalvo/fjv-modules.

## Implementation

### IModule interface

All the modules are an implementation of IModule.

IModule interface has two interfaces dependencies to implement that will be useful to ModuleFactory:

* IDefaultModule.
* IArgumentableModule.

### Load method

The load method is used to run the module from ModuleFactory. Each one interfaces has one Load method with a little diference.

Load method of IDefaultModule just take the input that can be passing from another module runned before. The Load method of IArgumentedModule take one more paramater named *moduleArgument* that take a argument from command-line.

You must implement IDefaultModule when the class do not need nothing more to run. In the other way, if you need passing a specified parameter to your module from the command-line, you must implement IArgumentableModule.

### Module attribute

The Module attribute is used to give a name, or in other words, a string caller name, and also give runing control to your class module if you like. It is not allowing more than one module with the same name.

#### Module sample

The next sample shows how whould define a simple argumentable module.

```csharp
[Module("-print")]
public class PrintModule : IArgumentableModule
{
    CustomObject _content;

    public byte[] Load(byte[] input, byte[] moduleArgument, string[] args, int index)
    {
        _content = SomeCustomObjectLoader(moduleArgument);

        return input;
    }
}
```

So, now when you run the program and passign the "-print some_file.txt" (without quotes) the Load method will be called. The input will null, but moduleArgument will have the string some_file.txt as byte array.

For this sample SomeCustomObjectLoader process the byte array content passing as argument of the module. In this case is a string with the name of a file. Suppose the method SomeCustomObjectLoader return a object that is save into _content.

Now we need to print the content over screen. So now, we must add an option to do that.

### Option attribute

An option is a method that can receive parameters to do some process. All methods would you like use as option must decorate with Option attribute and give it a name. It is not allowing more than one option with the same name.

#### Option sample

```csharp
[Module("-print")]
public class PrintModule : IArgumentableModule
{
    CustomObject _content;

    public byte[] Load(byte[] input, byte[] moduleArgument, string[] args, int index)
    {
        // load the content.
        _content = SomeCustomObjectLoader(moduleArgument);

        return input;
    }

    [Option("--screen")]
    public byte[] PrintScreen()
    {
        // print text on to screen.
        Console.WriteLine(_content.ToString());

        return _content.ToByteArray();
    }
}
```

Now if we would like to save the content into a file, we need to create a new class module to separate the responsibility. In this case we just will use the default input.

```csharp
[Module("-save")]
public class PrintModule : IDefaultModule
{
    byte[] _content;

    public byte[] Load(byte[] input, string[] args, int index)
    {
        _content = input;
    }

    [Option("--file")]
    public byte[] PrintScreen(string path)
    {
        // print text on to screen.
        File.WriteAllBytes(path, _content);

        return _content;
    }
}
```

So, you can run.

```shell
myprogram -print some_file.txt --screen -save --file copy.txt
```

The byte array content of -print module is passed to the input parameter of -save module. If you like make changes into the byte array data you have entered freedom to that.

## Byte array result

Each Load(...) and Option method executed would return a byte array that probably will be used as an input to the next module, like a chain reaction.

To illustrate this, we create the *TextProcessModule* with an option method to remove some part of the text. The other class will be *FileModule*, that will be responsible to save the input byte array to a file.

See the sample code below:

```csharp
//some source

[Module("*", ModuleRunningControl.Input)]
public class TextProcessModule : IDefaultModule
{
    //intentionaly omitted.

    [Option("--remove")]
    public byte[] RemoveFromText(string remove)
    {
        return Encoding.UTF8.GetBytes(_inputContent.Replace(remove, ""));
    }
}

//other source

[Module("-file", ModuleRunningControl.Output)]
public class FileModule : IDefaultModule
{
    byte[] _content;

    //intentionaly omitted.

    public byte[] Load(byte[] input, string[] args, int index)
    {
        //load the byte array content as string.
        _content = input;

        return input;
    }

    [Option("--save")]
    public byte[] SaveFile(string filename)
    {
        //save the text content into file.
        File.WriteAllBytes(filename, _content);

        return _content;
    }
}
```

You can use object inside your modules if you'll like, but the output always must be a byte array.

# Program sample

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

            var buffer = moduleFactory.Run(args);
        }
    }
}
```

# Running control

The Module can receive a second paramenter to give running control using ModuleRunningControl enum, detailed below:

* Input: it mark the module as an input. The module and options will be run in first place before the rest of modules encountered.
* Output: it mark the module as an output The module and options will be run in last place.
* ControlTaker: it mark the module as control taker. The application return immediately a result if it module is present in the command-line.
* Unique: it mark the module to doesn't attach more than one time. The module factory will throw an Exception if occur.

You can combine the enums members, except Input and Output.

# Features

## Wildcard

This library allow wildcard, that useful to take unknow strings as inputs for your program. You must specified the name as "*". It is not allowing more than one.

```csharp
namespace SomeExample
{
    [Module("*", ModuleRunningControl.Input)]
    public class OpenFileModule : IDefaultModule
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

When the module is a ControlTaker the application will run just only that module if it present in the command-line argument. It can use for special application way.

```csharp
//intentionaly omitted.

namespace SomeExample
{
    [Module("files",  ModuleRunningControl.Input |  ModuleRunningControl.ControlTaker )]
    public class OpenDirectoryFilesModule : IArgumentableModule
    {
        public byte[] Load(byte[] input, byte[] moduleArgument, string[] args, int index)
        {
            //get required argument.

            //do stuff.

            return someResult;
        }
    }
}
```

The next sample shows how could you use the control taker.

```csharp
//intentionaly omitted.

namespace SomeExample
{
    // run the command-line argument over each file in the directory.
    [Module("files",  ModuleRunningControl.Input |  ModuleRunningControl.ControlTaker )]
    public class OpenDirectoryFilesModule : IArgumentableModule
    {
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
            var moduleFactory = new ModuleFactory(typeof(Custom).Assembly);

            foreach (var filepath in files)
            {
                var filename = System.IO.Path.Combine(path, filepath);
                Fjv.Images.Globals.Inputs.CurrentFileName = filename;
                
                var file = System.IO.File.ReadAllBytes(filename);

                var result = moduleFactory.Run(auxArgs, file);

                // do something with the result or nothing.
            }

            // nothing to return.
            return null;
        }
    }
}
```

As you can see, it's possible run the ModuleFactory inside the same or other assemblies that has new modules group.

Enjoy!
