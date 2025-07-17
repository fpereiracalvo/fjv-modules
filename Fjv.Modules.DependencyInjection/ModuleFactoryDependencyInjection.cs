using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Fjv.Modules.Extensions;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Fjv.Modules.DependencyInjection
{
    public static class ModuleFactoryDependencyInjection
    {
        public static void AddModuleFactory(this IServiceCollection service, Assembly assembly, List<ModuleOptions> options = null)
        {
            GetModulesFromAssembly(service, assembly, options);

            AddScopedModuleFactory(service);
        }

         public static void AddModuleFactory<TIn, TOut>(this IServiceCollection service, Assembly assembly, List<ModuleOptions> options = null)
        {
            GetModulesFromAssembly(service, assembly, options);

            AddScopedModuleFactory<TIn, TOut>(service);
        }

        private static void GetModulesFromAssembly(IServiceCollection service, Assembly assembly, List<ModuleOptions> options = null)
        {
            var modules = assembly.GetModuleTypes();

            modules.ToList().ForEach(type => service.AddScoped(type));

            service.AddScoped<ModuleFactoryAssemblyConfiguration>((s) =>
            {
                return new ModuleFactoryAssemblyConfiguration
                {
                    Assembly = assembly,
                    Options = options
                };
            });
        }

        public static void AddModuleFactory(this IServiceCollection service, Assembly[] assemblies, List<ModuleOptions> options = null)
        {
            GetModulesFromAssemblies(service, assemblies, options);

            AddScopedModuleFactory(service);
        }

         public static void AddModuleFactory<TIn, TOut>(this IServiceCollection service, Assembly[] assemblies, List<ModuleOptions> options = null)
        {
            GetModulesFromAssemblies(service, assemblies, options);

            AddScopedModuleFactory<TIn, TOut>(service);
        }

        private static void GetModulesFromAssemblies(IServiceCollection service, Assembly[] assemblies, List<ModuleOptions> options = null)
        {
            foreach (var assembly in assemblies)
            {
                var modules = assembly.GetModuleTypes();

                modules.ToList().ForEach(type => service.AddScoped(type));
            }

            service.AddScoped<ModuleFactoryAssembliesConfiguration>((s) =>
            {
                return new ModuleFactoryAssembliesConfiguration
                {
                    Assemblies = assemblies,
                    Options = options
                };
            });
        }

        public static void AddModuleFactory(this IServiceCollection service, Type scopedToNamespace, List<ModuleOptions> options = null)
        {
            GetModulesFromType(service, scopedToNamespace, options);

            AddScopedModuleFactory(service);
        }

        public static void AddModuleFactory<TIn, TOut>(this IServiceCollection service, Type scopedToNamespace, List<ModuleOptions> options = null)
        {
            GetModulesFromType(service, scopedToNamespace, options);

            AddScopedModuleFactory<TIn, TOut>(service);
        }

        private static void GetModulesFromType(IServiceCollection service, Type scopedToNamespace, List<ModuleOptions> options = null)
        {
            var modules = scopedToNamespace.GetModuleTypes();

            modules.ToList().ForEach(type => service.AddScoped(type));

            service.AddScoped<ModuleFactoryNamespaceScopedConfiguration>((s) =>
            {
                return new ModuleFactoryNamespaceScopedConfiguration
                {
                    ScopedNamespaceType = scopedToNamespace,
                    Options = options
                };
            });
        }

        private static void AddScopedModuleFactory(IServiceCollection service)
        {
            service.AddScoped<IModuleFactory, DefaultModuleFactory>();
        }

        private static void AddScopedModuleFactory<TIn, TOut>(IServiceCollection service)
        {
            service.AddScoped<Generic.IModuleFactory<TIn, TOut>, GenericModuleFactory<TIn, TOut>>();
        }
    }
}
