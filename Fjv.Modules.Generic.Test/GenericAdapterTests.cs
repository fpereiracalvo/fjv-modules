using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Fjv.Modules.Extensions;
using Fjv.Modules.Generic.Test.TestModules;
using Xunit;

namespace Fjv.Modules.Generic.Test
{
    public class GenericAdapterTests
    {
        // Test para verificar que podemos usar módulos genéricos directamente
        [Fact]
        public void GenericModule_CanBeUsedDirectly()
        {
            // Arrange
            var stringModule = new StringModule();
            
            // Act
            var result = stringModule.Echo("test direct usage");
            
            // Assert
            Assert.Equal("test direct usage", result);
            
            // También probamos otras operaciones
            Assert.Equal("TEST UPPERCASE", stringModule.ToUpper("test uppercase"));
        }
        
        // Test para verificar que los módulos legacy también funcionan
        [Fact]
        public void LegacyModule_CanBeUsedDirectly()
        {
            // Arrange
            var legacyModule = new LegacyByteArrayModule();
            
            // Act
            var input = Encoding.UTF8.GetBytes("legacy test");
            var output = legacyModule.Process(input);
            
            // Assert - Verificamos que los primeros 3 bytes sean el prefijo esperado
            Assert.Equal(0x01, output[0]);
            Assert.Equal(0x02, output[1]);
            Assert.Equal(0x03, output[2]);
            
            // Y que el resto sean los bytes de entrada
            var originalInput = new byte[output.Length - 3];
            Array.Copy(output, 3, originalInput, 0, originalInput.Length);
            Assert.Equal("legacy test", Encoding.UTF8.GetString(originalInput));
        }
        
        // Test para verificar que podemos obtener un módulo por su nombre desde la fábrica
        [Fact]
        public void ModuleFactory_CanGetModuleByName()
        {
            // Arrange
            var factory = new ModuleFactory<string, string>(Assembly.GetExecutingAssembly());
            
            // Act - Obtenemos un módulo por su nombre
            var stringModule = factory.GetModule("stringmodule");
            
            // Assert
            Assert.NotNull(stringModule);
            Assert.IsType<StringModule>(stringModule);
        }
    }
}
