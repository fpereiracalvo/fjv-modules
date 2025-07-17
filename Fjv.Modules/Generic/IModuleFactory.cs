using System;
using System.Collections.Generic;
using System.Threading;
using Fjv.Modules.Commons;

namespace Fjv.Modules.Generic
{
    /// <summary>
    /// Generic interface for a module factory that supports strongly-typed input and output
    /// </summary>
    /// <typeparam name="TInput">Type of the input data for modules</typeparam>
    /// <typeparam name="TOutput">Type of the output data from modules</typeparam>
    public interface IModuleFactory<TInput, TOutput> : IModuleFactoryEvents, IModuleFactoryCommons
    {
        TOutput Run(string[] args, TInput initialInput = default);

        System.Threading.Tasks.Task<TOutput> RunAsync(string[] args, TInput initialInput = default, CancellationToken cancellationToken = default);
    }
}
