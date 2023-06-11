using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fjv.Modules.Attributes;
using Xunit;

namespace Fjv.Modules.Test.ThirdPartyModule
{
    public class ThirdPartyModuleTest
    {
        [Fact]
        public void RenameThirdPartyModuleTest()
        {
            // Given
            var args = new string[]{ "-my-option-module" };

            var moduleFactory = new ModuleFactory(typeof(ThirdPartyModuleTest), new List<ModuleOptions>{
                new ModuleOptions {
                    ModuleType = typeof(TestModule),
                    Name = "-my-option-module"
                }
            });

            // When
            var buffer= moduleFactory.Run(args);
        
            // Then
            Assert.True(moduleFactory.HasModule("-my-option-module"));
        }

        [Fact]
        public void NormalThirdPartyModuleTest()
        {
            // Given
            var args = new string[]{ "-thirdparty-module" };

            var moduleFactory = new ModuleFactory(typeof(ThirdPartyModuleTest));

            // When
            var buffer= moduleFactory.Run(args);
        
            // Then
            Assert.True(moduleFactory.HasModule("-thirdparty-module"));
        }

        [Module("-thirdparty-module")]
        public class TestModule : IDefaultModule
        {
            public byte[] Load(byte[] input, string[] args, int index)
            {
                Debug.WriteLine("Third party module!");
                
                Assert.True(true);

                return input;
            }
        }
    }
}