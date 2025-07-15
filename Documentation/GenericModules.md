# Módulos Genéricos

Los módulos genéricos permiten trabajar con tipos de datos específicos en lugar de usar `byte[]` para la entrada y salida.

## Interfaces Genéricas

Las interfaces genéricas disponibles son:

- `IModule<TInput, TOutput>`: Interfaz base para todos los módulos genéricos.
- `IDefaultModule<TInput, TOutput>`: Para módulos genéricos estándar.
- `IArgumentableModule<TInput, TArg, TOutput>`: Para módulos genéricos que aceptan argumentos directos.
- `IDefaultModuleAsync<TInput, TOutput>`: Para módulos genéricos asíncronos.
- `IArgumentableModuleAsync<TInput, TArg, TOutput>`: Para módulos genéricos asíncronos que aceptan argumentos directos.

## Ejemplo de uso

### Definición de un módulo genérico

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
}
```

### Creación de una fábrica genérica

```csharp
// Crear una fábrica que trabaja con strings como entrada y salida
var factory = GenericModuleExtensions.CreateGenericFactory<string, string>(typeof(Program).Assembly);

// Ejecutar la fábrica
string result = factory.Run(args, "Initial text");
Console.WriteLine($"Result: {result}");
```

## Compatibilidad con módulos existentes

Las interfaces originales (`IDefaultModule`, `IArgumentableModule`, etc.) ahora heredan de las interfaces genéricas usando `byte[]` como tipo, lo que garantiza la compatibilidad con el código existente.

### Adaptar módulos existentes al sistema genérico

```csharp
// Adaptar un módulo existente para trabajar con strings
IDefaultModule legacyModule = new MyExistingModule();
IDefaultModule<byte[], string> adaptedModule = legacyModule.AsGeneric<string>();

// También puedes proporcionar tu propio convertidor
IDefaultModule<byte[], CustomType> customAdaptedModule = legacyModule.AsGeneric<CustomType>(
    bytes => new CustomType(bytes) // Convertidor personalizado
);
```

## Convertidores personalizados

El sistema permite definir convertidores personalizados para transformar entre diferentes tipos de datos:

```csharp
// Definir un convertidor personalizado
Func<byte[], Person> personConverter = bytes => 
{
    var json = System.Text.Encoding.UTF8.GetString(bytes);
    return System.Text.Json.JsonSerializer.Deserialize<Person>(json);
};

// Usar el convertidor con un módulo existente
var personModule = legacyModule.AsGeneric<Person>(personConverter);
```

## Notas importantes

1. Las opciones de los módulos genéricos deben devolver el mismo tipo que el tipo de salida del módulo.
2. Para usar tipos complejos, es recomendable implementar los convertidores adecuados.
3. La biblioteca provee convertidores por defecto para tipos comunes como `string` y `byte[]`.
