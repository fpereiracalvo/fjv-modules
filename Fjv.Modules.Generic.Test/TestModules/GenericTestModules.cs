using Fjv.Modules.Attributes;

namespace Fjv.Modules.Generic.Test.TestModules
{
    // Define a simple generic module that works with strings
    [Module("stringmodule")]
    [ModuleHelp("A test module that processes strings")]
    public class StringModule : IDefaultModule<string, string>
    {
        [Option("echo")]
        [OptionHelp("Echoes back the input string")]
        public string Echo(string input)
        {
            Console.WriteLine($"Echoing input: {input}");

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

        public string Load(string input, string[] args, int index)
        {
            Console.WriteLine($"Loading input: {input}");
            Console.WriteLine($"Arguments: {string.Join(", ", args)}");

            return string.Empty;
        }
    }

    // Default module without arguments that produces a string
    [Module("greeting")]
    [ModuleHelp("A default module that generates greetings")]
    public class GreetingModule : IDefaultModule<string, string>
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
        
        // Implementation required by IDefaultModule<TInput, TOutput> interface
        public string Load(string input, string[] args, int index)
        {
            // For a module that doesn't depend on input, we simply ignore the input
            return string.Empty; // Or we could return a default value
        }
    }

    // Argumentable module that takes an argument and produces a result
    [Module("calculator")]
    [ModuleHelp("A module that performs simple calculations")]
    public class CalculatorModule : IArgumentableModule<int, int, int>
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
        
        // Implementation required by the IArgumentableModule<TInput, TArg, TOutput> interface
        public int Load(int input, int moduleArgument, string[] args, int index)
        {
            // Use the argument as input value
            return moduleArgument;
        }
    }
    
    // Asynchronous module that simulates long-running operations
    [Module("asynctask")]
    [ModuleHelp("A module that performs asynchronous operations")]
    public class AsyncTaskModule : IArgumentableModuleAsync<string, string, string>
    {
        [Option("delay")]
        [OptionHelp("Adds a delay and returns the input")]
        public async Task<string> DelayOperation()
        {
            // Simulates a time-consuming operation
            await Task.Delay(100);

            return $"Processed";
        }
        
        // Implementation required by the IArgumentableModuleAsync<TInput, TArg, TOutput> interface
        public async Task<string> LoadAsync(string input, string moduleArgument, string[] args, int index, CancellationToken cancellationToken = default)
        {
            // Use the argument as value
            await Task.CompletedTask; // To make it asynchronous
            
            return moduleArgument;
        }
    }
}
