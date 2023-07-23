using Fjv.Modules.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Fjv.Modules.DependencyInjection.Test;

public class DependencyInjectionPreparationTest
{
    IServiceProvider _service;

    public DependencyInjectionPreparationTest()
    {
        var services = new ServiceCollection();

        services.AddModuleFactory(typeof(DependencyInjectionPreparationTest));

        _service = services.BuildServiceProvider();
    }

    [Fact]
    public void Test1()
    {
        var args = new string[]{ "-m", "--sa", "message", "color", "--nsa", "message,color" };

        var moduleFactory = _service.GetService<Fjv.Modules.DependencyInjection.ModuleFactory>();

        var buffer = moduleFactory.Run(args);

        Assert.True(moduleFactory.HasModule("-m"));
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

            Assert.Equal("message", message);
            Assert.Equal("color", color);

            return new byte[]{};
        }

        [Option("--nsa")]
        public byte[] NoSeparatedArguments(string message, string color)
        {
            Console.WriteLine($"from default: {message}, {color}");

            Assert.Equal("message", message);
            Assert.Equal("color", color);

            return new byte[]{};
        }
    }
}