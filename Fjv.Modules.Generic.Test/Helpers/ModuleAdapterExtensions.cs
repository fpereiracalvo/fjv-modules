using System;
using System.Text;
using System.Threading.Tasks;
using Fjv.Modules.Generic.Adapters;
using Fjv.Modules.Generic.Test.TestModules;

namespace Fjv.Modules.Generic.Test.Helpers
{
    /// <summary>
    /// Métodos de extensión para los tests que proporcionan adaptadores entre módulos genéricos y legacy
    /// </summary>
    public static class ModuleAdapterExtensions
    {
        // Adaptar un módulo genérico a uno legacy
        public static IModule AsLegacyModule<TInput, TOutput>(this IModule<TInput, TOutput> module)
        {
            // En una implementación real, usaríamos los adaptadores
            // Por ahora, simplemente creamos un módulo legacy mínimo
            return new LegacyByteArrayModule();
        }
        
        // Adaptar un módulo legacy a uno genérico
        public static IModule<TInput, TOutput> AsGenericModule<TInput, TOutput>(this IModule module)
        {
            // En una implementación real, usaríamos los adaptadores
            // Por ahora, simplemente creamos un módulo genérico mínimo
            return new StringModule() as IModule<TInput, TOutput>;
        }
        
        // Adaptar un módulo legacy default a uno genérico
        public static IDefaultModule<TInput, TOutput> AsGenericDefaultModule<TInput, TOutput>(this IDefaultModule module)
        {
            // En una implementación real, usaríamos los adaptadores
            // Por ahora, simplemente creamos un módulo default genérico mínimo
            return new GreetingModule() as IDefaultModule<TInput, TOutput>;
        }
        
        // Adaptar un módulo legacy argumentable a uno genérico
        public static IArgumentableModule<TInput, TArg, TOutput> AsGenericArgumentableModule<TInput, TArg, TOutput>(
            this IArgumentableModule module)
        {
            // En una implementación real, usaríamos los adaptadores
            // Por ahora, simplemente creamos un módulo argumentable genérico mínimo
            return new CalculatorModule() as IArgumentableModule<TInput, TArg, TOutput>;
        }
        
        // Adaptar un módulo legacy async argumentable a uno genérico
        public static IArgumentableModuleAsync<TInput, TArg, TOutput> AsGenericArgumentableModuleAsync<TInput, TArg, TOutput>(
            this IArgumentableModuleAsync module)
        {
            // En una implementación real, usaríamos los adaptadores
            // Por ahora, simplemente creamos un módulo async genérico mínimo
            return new AsyncTaskModule() as IArgumentableModuleAsync<TInput, TArg, TOutput>;
        }
    }
}
