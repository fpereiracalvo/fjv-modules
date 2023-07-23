using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Fjv.Modules.Extensions;

namespace Fjv.Modules.DependencyInjection;
public static class ModuleFactoryDependencyInjection
{
    public static void AddModuleFactory(this IServiceCollection service, Assembly assembly, List<ModuleOptions> options = null)
    {
        var modules = assembly.GetModuleTypes();

        modules.ToList().ForEach(type=>service.AddScoped(type));

        service.AddScoped<ModuleFactoryAssemblyConfiguration>((s)=>{
            return new ModuleFactoryAssemblyConfiguration {
                Assembly = assembly,
                Options = options
            };
        });
        service.AddScoped<ModuleFactory>();
    }

    public static void AddModuleFactory(this IServiceCollection service, Assembly[] assemblies, List<ModuleOptions> options = null)
    {
        foreach (var assembly in assemblies)
        {
            var modules = assembly.GetModuleTypes();

            modules.ToList().ForEach(type=>service.AddScoped(type));
        }

        service.AddScoped<ModuleFactoryAssembliesConfiguration>((s)=>{
            return new ModuleFactoryAssembliesConfiguration{
                 Assemblies = assemblies,
                Options = options
            };
        });
        service.AddScoped<ModuleFactory>();
    }

    public static void AddModuleFactory(this IServiceCollection service, Type scopedToNamespace, List<ModuleOptions> options = null)
    {
        var modules = scopedToNamespace.GetModuleTypes();

        modules.ToList().ForEach(type=>service.AddScoped(type));

        service.AddScoped<ModuleFactoryNamespaceScopedConfiguration>((s)=>{
            return new ModuleFactoryNamespaceScopedConfiguration{
                 ScopedNamespaceType = scopedToNamespace,
                Options = options
            };
        });
        service.AddScoped<ModuleFactory>();
    }
}
