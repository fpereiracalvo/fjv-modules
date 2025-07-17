using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fjv.Modules.Commons;

namespace Fjv.Modules
{
    /// <summary>
    /// Generic interface for a module factory that supports strongly-typed input and output.
    /// </summary>
    public interface IModuleFactory : IModuleFactoryEvents, IModuleFactoryCommons
    {
        byte[] Run(string[] args, byte[] buffer = null);
        byte[] Run(ModuleItem module, ModuleFactory moduleFactory, byte[] input);
        Task<byte[]> RunAsync(string[] args, byte[] buffer = null);
        Task<byte[]> RunAsync(ModuleItem module, ModuleFactory moduleFactory, byte[] input);
    }
}