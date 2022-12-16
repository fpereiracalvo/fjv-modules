using Fjv.Modules;
using Fjv.Modules.Attributes;

namespace CopyTool.Modules 
{
    [Module("-file")]
    public class FileModule : IArgumentableModule
    {
        string _filePath = string.Empty;

        byte[] _moduleArgument = new byte[]{};

        public byte[] Load(byte[] input, byte[] moduleArgument, string[] args, int index)
        {
            _moduleArgument = moduleArgument;

            _filePath = System.Text.Encoding.UTF8.GetString(moduleArgument);
            
            return moduleArgument;
        }

        [Option("--copy")]
        public byte[] CopyTo(string destinyPath)
        {
            Console.WriteLine($"Copy {_filePath} to {destinyPath}...");

            try
            {
                File.Copy(_filePath, destinyPath);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return _moduleArgument;
        }

        [Option("--copy-partial")]
        public byte[] CopyPartial(int length, string destinyPath)
        {
            if(length<=0)
            {
                Console.WriteLine($"The partial length must by more than 0.");

                return _moduleArgument;
            }

            try
            {
                using (var file = new System.IO.StreamReader(_filePath))
                {
                    var buffer = new char[length];

                    file.ReadBlock(buffer, 0, (int)length);

                    File.WriteAllBytes(destinyPath, System.Text.Encoding.UTF8.GetBytes(buffer.Where(s=>s!=(char)0).ToArray()));
                }

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return _moduleArgument;
        }
    }
}