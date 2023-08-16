using System;
using Fjv.Modules.Attributes;
using Xunit;

namespace Fjv.Modules.Test.HelpMessage
{
    public class HelpMessageTest
    {
        [Fact]
        public void ShowArgumentedHelpMessageTest()
        {
            //given
            var args = new string[]{ "-p", "--rs", "-s", "--put", "2,3" };
            IModuleFactory moduleFactory = new ModuleFactory(typeof(HelpMessageTest));

            //when
            var message = moduleFactory.GetHelp(args);
            Console.WriteLine(message);

            //then
            Assert.Equal($"-p\tHelp message for PClass.\n\t--rs\tHelp message for RSOption.\n-s\tHelp message for SClass.\n\t--put\tHelp message for PutOption. Input: integer value, second integer value.\n", message);
        }

        [Fact]
        public void ShowAllHelpMessageTest()
        {
            //given
            IModuleFactory moduleFactory = new ModuleFactory(typeof(HelpMessageTest));

            //when
            var message = moduleFactory.GetHelp();
            Console.WriteLine(message);

            //then
            Assert.Equal($"-p\tHelp message for PClass.\n\t--ff\tHelp message for FfOption.\n\t--rs\tHelp message for RSOption.\n-s\tHelp message for SClass.\n\t--pop\tHelp message for PopOption.\n\t--put\tHelp message for PutOption. Input: integer value, second integer value.\n", message);
        }
    }

    [Module("-p", Commons.ModuleRunningControl.Unique)]
    [ModuleHelp("Help message for PClass.")]
    public class PClass : IDefaultModule
    {
        public byte[] Load(byte[] input, string[] args, int index)
        {
            Console.WriteLine(nameof(PClass));

            return null;
        }

        [Option("--rs")]
        [OptionHelp("Help message for RSOption.")]
        public byte[] RSOption()
        {
            return null;
        }

        [Option("--ff")]
        [OptionHelp("Help message for FfOption.")]
        public byte[] FfOption()
        {
            return null;
        }
    }

    [Module("-s")]
    [ModuleHelp("Help message for SClass.")]
    public class SClass : IDefaultModule
    {
        public byte[] Load(byte[] input, string[] args, int index)
        {
            Console.WriteLine(nameof(SClass));

            return null;
        }

        [Option("--put")]
        [OptionHelp("Help message for PutOption. Input: integer value, second integer value.")]
        public byte[] PutOption(int value, int secondValue)
        {
            Console.WriteLine($"value: {value}");
            Console.WriteLine($"secondValue: {secondValue}");

            return null;
        }

        [Option("--pop")]
        [OptionHelp("Help message for PopOption.")]
        public byte[] PopOption()
        {
            return null;
        }
    }
}