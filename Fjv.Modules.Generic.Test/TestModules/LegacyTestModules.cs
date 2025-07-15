using System;
using System.Threading.Tasks;
using Fjv.Modules.Attributes;

namespace Fjv.Modules.Generic.Test.TestModules
{
    // Módulos tradicionales que usan byte[] para mantener la compatibilidad con versiones anteriores
    
    [Module("legacymodule")]
    [ModuleHelp("A legacy module that uses byte arrays")]
    public class LegacyByteArrayModule : IModule
    {
        [Option("process")]
        [OptionHelp("Process the input bytes")]
        public byte[] Process(byte[] input)
        {
            // Simplemente agregamos un prefijo a los bytes para demostrar procesamiento
            if (input == null || input.Length == 0)
                return new byte[] { 0x01, 0x02, 0x03 };
                
            byte[] result = new byte[input.Length + 3];
            result[0] = 0x01;
            result[1] = 0x02;
            result[2] = 0x03;
            Array.Copy(input, 0, result, 3, input.Length);
            
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
            // Implementación requerida por la interfaz
            return input;
        }
    }
    
    [Module("legacyargument")]
    [ModuleHelp("A legacy argumentable module")]
    public class LegacyArgumentModule : IArgumentableModule
    {
        [Option("transform")]
        [OptionHelp("Transform the input argument")]
        public byte[] Transform(byte[] input)
        {
            if (input == null || input.Length == 0)
                return Array.Empty<byte>();
                
            // Transformamos los bytes invirtiéndolos
            byte[] result = new byte[input.Length];
            Array.Copy(input, result, input.Length);
            Array.Reverse(result);
            
            return result;
        }
        
        public byte[] Load(byte[] input, byte[] moduleArgument, string[] args, int index)
        {
            // Implementación requerida por la interfaz
            return moduleArgument;
        }
    }
    
    [Module("legacyasync")]
    [ModuleHelp("A legacy async module")]
    public class LegacyAsyncModule : IArgumentableModuleAsync
    {
        [Option("process")]
        [OptionHelp("Process data asynchronously")]
        public async Task<byte[]> ProcessAsync(byte[] input)
        {
            await Task.Delay(50); // Simula algún trabajo asincrónico
            
            if (input == null || input.Length == 0)
                return System.Text.Encoding.UTF8.GetBytes("No Input");
                
            // Devolvemos el input como una cadena con un prefijo
            string inputStr = System.Text.Encoding.UTF8.GetString(input);
            string result = $"Async Processed: {inputStr}";
            
            return System.Text.Encoding.UTF8.GetBytes(result);
        }
        
        public async Task<byte[]> LoadAsync(byte[] input, byte[] moduleArgument, string[] args, int index)
        {
            // Implementación requerida por la interfaz
            await Task.CompletedTask;
            return moduleArgument;
        }
    }
}
