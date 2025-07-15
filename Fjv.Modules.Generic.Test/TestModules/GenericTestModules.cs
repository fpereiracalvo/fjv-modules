using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Fjv.Modules.Attributes;
using Fjv.Modules.Generic;

namespace Fjv.Modules.Generic.Test.TestModules
{
    // Definimos un módulo genérico simple que trabaja con strings
    [Module("stringmodule")]
    [ModuleHelp("A test module that processes strings")]
    public class StringModule : IModule<string, string>, IModule
    {
        [Option("echo")]
        [OptionHelp("Echoes back the input string")]
        public string Echo(string input)
        {
            return input;
        }
        
        [Option("uppercase")]
        [OptionHelp("Converts the input string to uppercase")]
        public string ToUpper(string input)
        {
            return input?.ToUpper() ?? string.Empty;
        }
        
        [Option("reverse")]
        [OptionHelp("Reverses the input string")]
        public string Reverse(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
                
            char[] chars = input.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }
        
        // Método auxiliar para las implementaciones legacy
        private byte[] StringToBytes(string str)
        {
            return str != null ? System.Text.Encoding.UTF8.GetBytes(str) : Array.Empty<byte>();
        }
        
        private string BytesToString(byte[] bytes)
        {
            return bytes != null ? System.Text.Encoding.UTF8.GetString(bytes) : string.Empty;
        }
        
        // Implementaciones para compatibilidad con interfaces legacy
        public byte[] Invoke(string optionName, byte[] input)
        {
            var inputStr = BytesToString(input);
            string result = null;
            
            switch (optionName)
            {
                case "echo":
                    result = Echo(inputStr);
                    break;
                case "uppercase":
                    result = ToUpper(inputStr);
                    break;
                case "reverse":
                    result = Reverse(inputStr);
                    break;
            }
            
            return StringToBytes(result);
        }
    }

    // Módulo por defecto sin argumentos que produce un string
    [Module("greeting")]
    [ModuleHelp("A default module that generates greetings")]
    public class GreetingModule : IDefaultModule<string, string>, IDefaultModule
    {
        [Option("hello")]
        [OptionHelp("Says hello")]
        public string SayHello()
        {
            return "Hello, World!";
        }
        
        [Option("bye")]
        [OptionHelp("Says goodbye")]
        public string SayGoodbye()
        {
            return "Goodbye, World!";
        }
        
        // Implementación requerida por la interfaz IDefaultModule<TInput, TOutput>
        public string Load(string input, string[] args, int index)
        {
            // Para un módulo que no depende de entrada, simplemente ignoramos el input
            return string.Empty; // O podríamos devolver un valor por defecto
        }
        
        // Métodos auxiliares para las implementaciones legacy
        private byte[] StringToBytes(string str)
        {
            return str != null ? System.Text.Encoding.UTF8.GetBytes(str) : null;
        }
        
        private string BytesToString(byte[] bytes)
        {
            return bytes != null ? System.Text.Encoding.UTF8.GetString(bytes) : null;
        }
        
        // Implementaciones para compatibilidad con interfaces legacy
        public byte[] Load(byte[] input, string[] args, int index)
        {
            // Ignoramos la entrada para un módulo IDefaultModule
            return null; // Devolver null aquí hace que se use el valor de retorno de Invoke
        }
        
        public byte[] Invoke(string optionName, byte[] input)
        {
            string result = null;
            
            switch (optionName)
            {
                case "hello":
                    result = SayHello();
                    break;
                case "bye":
                    result = SayGoodbye();
                    break;
            }
            
            return StringToBytes(result);
        }
    }

    // Módulo argumentable que toma un argumento y produce un resultado
    [Module("calculator")]
    [ModuleHelp("A module that performs simple calculations")]
    public class CalculatorModule : IArgumentableModule<int, int, int>, IArgumentableModule
    {
        [Option("double")]
        [OptionHelp("Doubles the input value")]
        public int Double(int input)
        {
            return input * 2;
        }
        
        [Option("square")]
        [OptionHelp("Squares the input value")]
        public int Square(int input)
        {
            return input * input;
        }
        
        // Implementación requerida por la interfaz IArgumentableModule<TInput, TArg, TOutput>
        public int Load(int input, int moduleArgument, string[] args, int index)
        {
            // Usamos el argumento como valor de entrada
            return moduleArgument;
        }
        
        // Implementaciones para compatibilidad con interfaces legacy
        public byte[] Load(byte[] input, byte[] moduleArgument, string[] args, int index)
        {
            // En las pruebas para argumentable module, se espera que pasemos directamente el valor
            int intInput = moduleArgument != null && moduleArgument.Length >= 4 
                ? BitConverter.ToInt32(moduleArgument, 0) 
                : (input != null && input.Length >= 4 ? BitConverter.ToInt32(input, 0) : 0);
                
            return BitConverter.GetBytes(intInput);
        }
        
        public byte[] Invoke(string optionName, byte[] input)
        {
            int inputValue = input != null && input.Length >= 4 ? BitConverter.ToInt32(input, 0) : 0;
            int result = 0;
            
            switch (optionName)
            {
                case "double":
                    result = Double(inputValue);
                    break;
                case "square":
                    result = Square(inputValue);
                    break;
            }
            
            return BitConverter.GetBytes(result);
        }
    }
    
    // Módulo asíncrono que simula operaciones de larga duración
    [Module("asynctask")]
    [ModuleHelp("A module that performs asynchronous operations")]
    public class AsyncTaskModule : IArgumentableModuleAsync<string, string, string>, IArgumentableModuleAsync
    {
        [Option("delay")]
        [OptionHelp("Adds a delay and returns the input")]
        public async Task<string> DelayOperation(string input)
        {
            // Simula una operación que toma tiempo
            await Task.Delay(100);
            return $"Processed: {input}";
        }
        
        // Implementación requerida por la interfaz IArgumentableModuleAsync<TInput, TArg, TOutput>
        public async Task<string> LoadAsync(string input, string moduleArgument, string[] args, int index)
        {
            // Usamos el argumento como valor
            await Task.CompletedTask; // Para hacerlo asíncrono
            return moduleArgument;
        }
        
        // Métodos auxiliares para las implementaciones legacy
        private byte[] StringToBytes(string str)
        {
            return str != null ? System.Text.Encoding.UTF8.GetBytes(str) : null;
        }
        
        private string BytesToString(byte[] bytes)
        {
            return bytes != null ? System.Text.Encoding.UTF8.GetString(bytes) : null;
        }
        
        // Implementaciones para compatibilidad con interfaces legacy
        public async Task<byte[]> LoadAsync(byte[] input, byte[] moduleArgument, string[] args, int index)
        {
            var inputStr = BytesToString(input);
            var argStr = BytesToString(moduleArgument);
            
            // Para pruebas, simplemente pasamos la entrada después de una espera mínima
            await Task.Delay(1);
            return input;
        }
        
        public async Task<byte[]> InvokeAsync(string optionName, byte[] input)
        {
            var inputStr = BytesToString(input);
            string result = string.Empty;
            
            switch (optionName)
            {
                case "delay":
                    result = await DelayOperation(inputStr);
                    break;
            }
            
            return System.Text.Encoding.UTF8.GetBytes(result);
        }
    }
}
