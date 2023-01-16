using System;
using Xunit;
using Fjv.Modules.Attributes;

namespace Fjv.Modules.Test.SeparatedArguments
{
    public class OptionSeparatedArgumentTest
    {
        [Fact]
        public void TestName()
        {
            var args = new string[]{ "-m", "--sa", "message", "color", "--nsa", "message,color" };

            var moduleFactory = new ModuleFactory(typeof(OptionSeparatedArgumentTest));

            var buffer= moduleFactory.Run(args);
        }
    }

    [Module("-m")]
    public class SeparatedArgumentModule : IDefaultModule
    {
        public byte[] Load(byte[] input, string[] args, int index)
        {
            return new byte[]{};
        }

        [Option("--sa", true)]
        public byte[] SeparatedArguments(string message, string color)
        {
            Console.WriteLine($"from separated: {message}, {color}");

            return new byte[]{};
        }

        [Option("--nsa")]
        public byte[] NoSeparatedArguments(string message, string color)
        {
            Console.WriteLine($"from default: {message}, {color}");

            return new byte[]{};
        }
    }
}