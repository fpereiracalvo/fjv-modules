using System;
using System.Threading.Tasks;
using Fjv.Modules.Attributes;

namespace Fjv.Modules.Generic.Test.TestModules
{
    // Traditional modules that use byte[] to maintain compatibility with previous versions
    
    [Module("legacymodule")]
    [ModuleHelp("A legacy module that uses byte arrays")]
    public class LegacyByteArrayModule : IDefaultModule
    {
        public byte[] Load(byte[] input, string[] args, int index)
        {
            return input;
        }

        [Option("process")]
        [OptionHelp("Process the input bytes")]
        public byte[] Process(string input)
        {
            if (string.IsNullOrEmpty(input))
                return new byte[] { 0x01, 0x02, 0x03 };

            byte[] result = new byte[input.Length + 3];
            result[0] = 0x01;
            result[1] = 0x02;
            result[2] = 0x03;

            Array.Copy(System.Text.Encoding.UTF8.GetBytes(input), 0, result, 3, input.Length);
            
            return result;
        }
    }
    
    [Module("legacydefault")]
    [ModuleHelp("A legacy default module")]
    public class LegacyDefaultModule : IDefaultModule
    {
        [Option("data")]
        [OptionHelp("Returns some data")]
        public byte[] GetData()
        {
            return System.Text.Encoding.UTF8.GetBytes("Legacy Default Data");
        }
        
        public byte[] Load(byte[] input, string[] args, int index)
        {
            return input;
        }
    }
    
    [Module("legacyargument")]
    [ModuleHelp("A legacy argumentable module")]
    public class LegacyArgumentModule : IArgumentableModule
    {
        [Option("transform")]
        [OptionHelp("Transform the input argument")]
        public byte[] Transform(string input)
        {
            if (string.IsNullOrEmpty(input))
                return Array.Empty<byte>();
                
            byte[] result = new byte[input.Length];
            Array.Copy(System.Text.Encoding.UTF8.GetBytes(input), result, input.Length);
            Array.Reverse(result);
            
            return result;
        }
        
        public byte[] Load(byte[] input, byte[] moduleArgument, string[] args, int index)
        {
            return moduleArgument;
        }
    }
    
    [Module("legacyasync")]
    [ModuleHelp("A legacy async module")]
    public class LegacyAsyncModule : IArgumentableModuleAsync
    {
        [Option("process")]
        [OptionHelp("Process data asynchronously")]
        public async Task<byte[]> ProcessAsync(string input)
        {
            await Task.Delay(50);

            if (string.IsNullOrEmpty(input))
                return System.Text.Encoding.UTF8.GetBytes("No Input");

            string result = $"Async Processed: {input}";
            
            return System.Text.Encoding.UTF8.GetBytes(result);
        }

        public async Task<byte[]> LoadAsync(byte[] input, byte[] moduleArgument, string[] args, int index, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;

            return moduleArgument;
        }
    }
}
