# Module attributes

The module attributes are used to define the module name, the module help message, the option name and the option help message.

## Module attribute

The module attribute is used to define the module name and running control.

```csharp
// define the module name.
[Module("-print")]
```

This name is use to invoke the module in the command line arguments.
    
```bash
> myapp -print
```

To use the running control, we need to use the ModuleRunningControl enum.

- **NotSet**: Default.
- **Input**: The module will run only in first place.
- **Output**: The module will run as a output module, runed in last place.
- **ControlTaker**: The module will run and return inmediatelly a result. That can be use with a nested module factory.
- **Unique**: The module will run only one time and it cannot argument more than one time. Throw an error if the same argment exists more than one.

```csharp
// define the module name and running control.
[Module("-print", ModuleRunningControl.Unique)]
```

## ModuleHelp attribute

The module help attribute is used to define the module help message.

```csharp
// define the module help message.
[ModuleHelp("Print the module help message.")]
```

## Option attribute

The option attribute is used to define the option name and running control.

```csharp
// define the option name.
[Option("--text")]
public byte[] MyOption(string text)
```

This name is use to invoke the option in the command line arguments.
    
```bash
> myapp -print --text "Hello world!"
```

The option has an optional boolean parameter to set the format input arguments. By default is comma separated, but if it true, the format will be space separated.

```csharp
// define the option name.
[Option("--sum")]
public byte[] CustomOption(int a, int b)
```

```bash
> myapp -print --sum 1,2
```

```csharp
// define the option name.
[Option("--sum", true)]
public byte[] CustomOption(int a, int b)
```

```bash
> myapp -print --sum 1 2
```

## OptionHelp attribute

The option help attribute is used to define the option help message.

```csharp
// define the option help message.
[OptionHelp("Print the option help message.")]
```
