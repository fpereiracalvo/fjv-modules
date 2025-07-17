using Fjv.Modules.Attributes;
using Fjv.Modules.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Fjv.Modules.DependencyInjection.Test;

public class DependencyInjectionPreparationGenericTest
{
    IServiceProvider _service;

    public DependencyInjectionPreparationGenericTest()
    {
        var services = new ServiceCollection();

        services.AddModuleFactory<string, string>(typeof(DependencyInjectionPreparationGenericTest));

        _service = services.BuildServiceProvider();
    }

    [Fact]
    public void Test1()
    {
        var args = new string[]{ "-gm", "--sa", "message", "color", "--nsa", "message,color" };

        var moduleFactory = _service.GetService<IModuleFactory<string, string>>();

        var buffer = moduleFactory.Run(args);

        Assert.True(moduleFactory.HasModule("-m"));
    }

    [Module("-gm")]
    public class SeparatedArgumentModule : IDefaultModule<string, string>
    {
        public string Load(string input, string[] args, int index)
        {
            return string.Empty;
        }

        [Option("--sa", true)]
        public string SeparatedArguments(string message, string color)
        {
            Console.WriteLine($"from separated: {message}, {color}");

            Assert.Equal("message", message);
            Assert.Equal("color", color);

            return string.Empty;
        }

        [Option("--nsa")]
        public string NoSeparatedArguments(string message, string color)
        {
            Console.WriteLine($"from default: {message}, {color}");

            Assert.Equal("message", message);
            Assert.Equal("color", color);

            return string.Empty;
        }
    }
}