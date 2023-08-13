# Module attributes

The module attributes are used to define the module name, the module help message, the option name and the option help message.

## Module attribute

The module attribute is used to define the module name and running control.

```csharp
// define the module name.
[Module("-print")]
```

To use the running control, we need to use the ModuleRunningControl enum.

NotSet: Default.
Input: The module will run only in first place.
Output: The module will run as a output module, runed in last place.
ControlTaker: The module will run and return inmediatelly a result. That can be use with a nested module factory.
Unique: The module will run only one time and it cannot argument more than one time.

```csharp
// define the module name and running control.
[Module("-print", ModuleRunningControl.Unique)]
```

## ModuleHelp attribute

The module help attribute is used to define the module help message.

```csharp
// define the module help message.
[ModuleHelp("Print the input text.")]
```