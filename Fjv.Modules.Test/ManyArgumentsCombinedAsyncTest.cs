using System;
using System.Linq;
using System.Threading.Tasks;
using Fjv.Modules.Attributes;
using Fjv.Modules.Exceptions;
using Xunit;

namespace Fjv.Modules.Test.ManyArgumentsAsync
{
    public class ManyArgumentsCombinedAsyncTest
    {
        [Fact]
        public async void RunManyArgumnetsTestMethod()
        {
            var args = new string[]{ "-p", "--rs", "-s", "--put", "2,3", "-o", "merge", "./*.txt" };

            var moduleFactory = new ModuleFactory(typeof(ManyArgumentsCombinedAsyncTest));

            var buffer= await moduleFactory.RunAsync(args);

            Assert.True(moduleFactory.HasModule("-p"));
            Assert.True(moduleFactory.HasModule("-s"));
            Assert.True(moduleFactory.HasModule("-o"));
            Assert.True(moduleFactory.HasModule("merge"));
        }

        [Fact]
        public async void RunManyArgumnetsWithUniqueExceptionTestMethod()
        {
            var args = new string[]{ "-p", "--rs", "-s", "--put", "2,4", "-o", "merge", "./*.txt", "-p" };

            var moduleFactory = new ModuleFactory(typeof(ManyArgumentsCombinedAsyncTest));

            var exception = await Assert.ThrowsAsync<ModulesException>(async () => {
                var buffer = await moduleFactory.RunAsync(args);
            });

            Assert.Equal("The module -p doesn't allow attach one more than exist.", exception.Message);
        }
    }

    [Module("-p", Commons.ModuleRunningControl.Unique)]
    public class PClass : IDefaultModuleAsync
    {
        public async Task<byte[]> LoadAsync(byte[] input, string[] args, int index)
        {
            await Task.Run(async () => {
                Console.WriteLine(nameof(PClass));
            });

            await Task.Delay(100);

            return null;
        }

        [Option("--rs")]
        public async Task<byte[]> RSOption()
        {
            Console.WriteLine(nameof(RSOption));

            await Task.Delay(0);

            return null;
        }
    }

    [Module("-s")]
    public class SClass : IDefaultModuleAsync
    {
        public async Task<byte[]> LoadAsync(byte[] input, string[] args, int index)
        {
            Console.WriteLine(nameof(SClass));

            await Task.Delay(100);
            
            return null;
        }

        [Option("--put")]
        public async Task<byte[]> PutOption(int value, int secondValue)
        {
            await Task.Run(()=>{
                Console.WriteLine($"value: {value}");
                Console.WriteLine($"secondValue: {secondValue}");
            });

            return null;
        }
    }

    [Module("-o", Commons.ModuleRunningControl.Input)]
    public class OClass : IDefaultModuleAsync
    {
        public async Task<byte[]> LoadAsync(byte[] input, string[] args, int index)
        {
            Console.WriteLine(nameof(OClass));

            await Task.Delay(100);

            return null;
        }
    }

    [Module("merge", 
        Commons.ModuleRunningControl.Input | 
        Commons.ModuleRunningControl.ControlTaker )]
    public class MergeClass : IArgumentableModuleAsync
    {
        public async Task<byte[]> LoadAsync(byte[] input, byte[] moduleArgument, string[] args, int index)
        {
            var auxArgs = args.Take(index).ToArray().Concat(args.Skip(index+2).ToArray()).ToArray();

            var moduleFactory = new ModuleFactory(typeof(ManyArgumentsCombinedAsyncTest));

            for (var i = 0; i < 3; i++)
            {
                await moduleFactory.RunAsync(auxArgs, null);
            }

            return null;
        }
    }
}