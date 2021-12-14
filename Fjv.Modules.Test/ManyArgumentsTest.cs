using System;
using System.Linq;
using Fjv.Modules.Attributes;
using Xunit;

namespace Fjv.Modules.Test
{
    public class ManyArgumentsTest
    {
        [Fact]
        public void RunManyArgumnetsTestMethod()
        {
            var args = new string[]{ "-p", "--rs", "-s", "--put", "2", "-o", "merge", "./*.txt" };

            var moduleFactory = new ModuleFactory(typeof(ManyArgumentsTest).Assembly);

            var buffer= moduleFactory.Run(args);

            Assert.True(moduleFactory.HasModule("-p"));
            Assert.True(moduleFactory.HasModule("-s"));
            Assert.True(moduleFactory.HasModule("-o"));
            Assert.True(moduleFactory.HasModule("merge"));
        }
    }

    [Module("-p")]
    public class PClass : IModule
    {
        public bool IsOutput => false;
        public bool IsInput => false;
        public bool IsControlTaker => false;
        public bool NeedArgument => false;

        public byte[] Load(byte[] input, string[] args, int index)
        {
            Console.WriteLine(nameof(PClass));

            return null;
        }

        public byte[] Load(byte[] input, byte[] moduleArgument, string[] args, int index)
        {
            throw new System.NotImplementedException();
        }

        [Option("--rs")]
        public byte[] RSOption()
        {
            return null;
        }
    }

    [Module("-s")]
    public class SClass : IModule
    {
        public bool IsOutput => false;
        public bool IsInput => false;
        public bool IsControlTaker => false;
        public bool NeedArgument => false;

        public byte[] Load(byte[] input, string[] args, int index)
        {
            Console.WriteLine(nameof(SClass));

            return null;
        }

        public byte[] Load(byte[] input, byte[] moduleArgument, string[] args, int index)
        {
            throw new System.NotImplementedException();
        }

        [Option("--put")]
        public byte[] PutOption(int value)
        {
            Console.WriteLine($"value: {value}");

            return null;
        }
    }

    [Module("-o")]
    public class OClass : IModule
    {
        public bool IsOutput => true;
        public bool IsInput => false;
        public bool IsControlTaker => false;
        public bool NeedArgument => false;

        public byte[] Load(byte[] input, string[] args, int index)
        {
            Console.WriteLine(nameof(OClass));

            return null;
        }

        public byte[] Load(byte[] input, byte[] moduleArgument, string[] args, int index)
        {
            throw new System.NotImplementedException();
        }
    }

    [Module("merge")]
    public class MergeClass : IModule
    {
        public bool IsOutput => false;
        public bool IsInput => true;
        public bool IsControlTaker => true;
        public bool NeedArgument => true;

        public byte[] Load(byte[] input, string[] args, int index)
        {
            throw new System.NotImplementedException();
        }

        public byte[] Load(byte[] input, byte[] moduleArgument, string[] args, int index)
        {
            Console.WriteLine(nameof(MergeClass));
            
            var auxArgs = args.Take(index).ToArray().Concat(args.Skip(index+2).ToArray()).ToArray();

            var moduleFactory = new ModuleFactory(typeof(ManyArgumentsTest).Assembly);

            for (var i = 0; i < 3; i++)
            {
                moduleFactory.Run(auxArgs, null);
            }

            return null;
        }
    }
}