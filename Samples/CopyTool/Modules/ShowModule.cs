using Fjv.Modules;
using Fjv.Modules.Attributes;

namespace CopyTool.Modules 
{
    [Module("-show")]
    public class ShowModule : IDefaultModule
    {
        string _filePath = string.Empty;
        byte[] _input = new byte[]{};

        public byte[] Load(byte[] input, string[] args, int index)
        {
            _input = input;
            _filePath = System.Text.Encoding.UTF8.GetString(input);

            return input;
        }

        [Option("--all")]
        public byte[] ShowAll()
        {
            try
            {
                Console.WriteLine(File.ReadAllText(_filePath));
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return _input;
        }

        [Option("--partial")]
        public byte[] ShowPartial(int length)
        {
            if(length<=0)
            {
                Console.WriteLine($"The partial length must by more than 0.");

                return _input;
            }

            try
            {
                using (var file = new System.IO.StreamReader(_filePath))
                {
                    var buffer = new char[length];

                    file.ReadBlock(buffer, 0, (int)length);

                    Console.WriteLine(new String(buffer.Where(s=>s!=(char)0).ToArray()));
                }

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return _input;
        }
    }
}