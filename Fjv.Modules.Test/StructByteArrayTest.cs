
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Fjv.Modules.Attributes;
using Fjv.Modules.Extensions;
using Xunit;

namespace Fjv.Modules.Test
{
    public class StructByteArrayTest
    {
        [Fact]
        public void PassingStructToModuleTest()
        {
            var args = new string[]{ "-transmit", "Hello world!", "-receive", "--print-screen" };

            var moduleFactory = new ModuleFactory(typeof(StructByteArrayTest).Assembly);

            var buffer= moduleFactory.Run(args);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MessageStruct
    {
        [MarshalAs(UnmanagedType.U4)]
        public Int32 Id;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string Text;
    }

    [Module("-transmit")]
    public class TrasmitterModule : IArgumentableModule
    {
        public byte[] Load(byte[] input, byte[] moduleArgument, string[] args, int index)
        {
            var message = new MessageStruct {
                Id = 1000,
                Text = Encoding.UTF8.GetString(moduleArgument)
            };

            return message.ToBytes();
        }
    }

    [Module("-receive")]
    public class ReceiverModule : IDefaultModule
    {
        MessageStruct _content;

        public byte[] Load(byte[] input, string[] args, int index)
        {
            _content = input.ToObject<MessageStruct>();

            Assert.True(_content.Id == 1000);
            Assert.True(_content.Text.Equals("Hello world!"));

            return input;
        }

        [Option("--print-screen")]
        public byte[] PrintScreen()
        {
            Console.WriteLine($"{_content.Id}: {_content.Text}");

            return _content.ToBytes();
        }
    }
}