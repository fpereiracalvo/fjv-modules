using System;
using System.Linq;
using Fjv.Modules.Attributes;
using Xunit;

namespace Fjv.Modules.Test.ManyArguments
{
    public class ManyArgumentsCombinedTest
    {
        [Fact]
        public void RunManyArgumnetsTestMethod()
        {
            var args = new string[]{ "-p", "--rs", "-s", "--put", "2,3", "-o", "merge", "./*.txt" };

            var moduleFactory = new ModuleFactory(typeof(ManyArgumentsCombinedTest));

            var buffer= moduleFactory.Run(args);

            Assert.True(moduleFactory.HasModule("-p"));
            Assert.True(moduleFactory.HasModule("-s"));
            Assert.True(moduleFactory.HasModule("-o"));
            Assert.True(moduleFactory.HasModule("merge"));
        }

        [Fact]
        public void RunManyArgumnetsWithUniqueExceptionTestMethod()
        {
            var args = new string[]{ "-p", "--rs", "-s", "--put", "2,4", "-o", "merge", "./*.txt", "-p" };

            var moduleFactory = new ModuleFactory(typeof(ManyArgumentsCombinedTest));

            Assert.Throws<Exception>(new Action(()=>{
                var buffer = moduleFactory.Run(args);
            }));
        }
    }

    [Module("-p", Commons.ModuleRunningControl.Unique)]
    public class PClass : IDefaultModule
    {
        public byte[] Load(byte[] input, string[] args, int index)
        {
            Console.WriteLine(nameof(PClass));

            return null;
        }

        [Option("--rs")]
        public byte[] RSOption()
        {
            return null;
        }
    }

    [Module("-s")]
    public class SClass : IDefaultModule
    {
        public byte[] Load(byte[] input, string[] args, int index)
        {
            Console.WriteLine(nameof(SClass));

            return null;
        }

        [Option("--put")]
        public byte[] PutOption(int value, int secondValue)
        {
            Console.WriteLine($"value: {value}");

            return null;
        }
    }

    [Module("-o", Commons.ModuleRunningControl.Input)]
    public class OClass : IDefaultModule
    {
        public byte[] Load(byte[] input, string[] args, int index)
        {
            Console.WriteLine(nameof(OClass));

            return null;
        }
    }

    [Module("merge", 
        Commons.ModuleRunningControl.Input | 
        Commons.ModuleRunningControl.ControlTaker )]
    public class MergeClass : IArgumentableModule
    {
        public byte[] Load(byte[] input, byte[] moduleArgument, string[] args, int index)
        {
            Console.WriteLine(nameof(MergeClass));
            
            var auxArgs = args.Take(index).ToArray().Concat(args.Skip(index+2).ToArray()).ToArray();

            var moduleFactory = new ModuleFactory(typeof(ManyArgumentsCombinedTest));

            for (var i = 0; i < 3; i++)
            {
                moduleFactory.Run(auxArgs, null);
            }

            return null;
        }
    }
}