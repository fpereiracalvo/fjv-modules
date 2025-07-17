using System;
using System.Reflection;
using System.Text;
using Fjv.Modules.Generic.Test.TestModules;

namespace Fjv.Modules.Generic.Test
{
    public class GenericAdapterTests
    {
        // Test to verify that we can use generic modules directly
        [Fact]
        public void GenericModule_CanBeUsedDirectly()
        {
            // Arrange
            var stringModule = new StringModule();
            
            // Act
            var result = stringModule.Echo("test direct usage");
            
            // Assert
            Assert.Equal("test direct usage", result);
            
            // Also, we test other operations
            Assert.Equal("TEST UPPERCASE", stringModule.ToUpper("test uppercase"));
        }
        
        // Test to verify that legacy modules also work
        [Fact]
        public void LegacyModule_CanBeUsedDirectly()
        {
            // Arrange
            var legacyModule = new LegacyByteArrayModule();

            // Act
            var input = "legacy test";
            var output = legacyModule.Process(input);
            
            // Assert - Verify that the first 3 bytes are the expected prefix
            Assert.Equal(0x01, output[0]);
            Assert.Equal(0x02, output[1]);
            Assert.Equal(0x03, output[2]);
            
            // And that the rest are the input bytes
            var originalInput = new byte[output.Length - 3];
            Array.Copy(output, 3, originalInput, 0, originalInput.Length);
            Assert.Equal("legacy test", Encoding.UTF8.GetString(originalInput));
        }
        
        // Test to verify that we can get a module by its name from the factory
        [Fact]
        public void ModuleFactory_CanGetModuleByName()
        {
            // Arrange
            var factory = new ModuleFactory<string, string>(Assembly.GetExecutingAssembly());
            
            // Act - We get a module by its name
            var stringModule = factory.GetModule("stringmodule");
            
            // Assert
            Assert.NotNull(stringModule);
            Assert.IsType<StringModule>(stringModule);
        }
    }
}
