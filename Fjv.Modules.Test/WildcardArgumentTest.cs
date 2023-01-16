using System;
using Xunit;
using Fjv.Modules;
using Fjv.Modules.Attributes;
using System.Text;
using System.IO;

namespace Fjv.Modules.Test.Wildcard
{
    public class WildcardArgumentTest
    {
        [Fact]
        public void EchoFileTextTest()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Remove(path.IndexOf("Fjv.Modules.Test"));

            var args = new string[]{ Path.Combine(path, "Files", "helloWorld.txt"), "--echo" };

            var moduleFactory = new ModuleFactory(typeof(WildcardArgumentTest));

            var buffer= moduleFactory.Run(args);

            //buffer has byte array of file text.
            Assert.NotEmpty(buffer);
            Assert.True(moduleFactory.HasModule("*"));
        }
    }

    [Module("*")]
    public class TextFileSample : IDefaultModule
    {
        string _text;

        public byte[] Load(byte[] input, string[] args, int index)
        {
            var filepath = Encoding.UTF8.GetString(input);

            if(!File.Exists(filepath))
            {
                //todo: show error message before terminate with exit 1.

                Environment.Exit(1);
            }

            _text = File.ReadAllText(filepath);

            return Encoding.UTF8.GetBytes(_text);
        }

        [Option("--echo")]
        public byte[] Print()
        {
            Console.WriteLine(_text);

            return Encoding.UTF8.GetBytes(_text);
        }
    }
}
