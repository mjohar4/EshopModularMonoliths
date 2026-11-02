using Carter;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Extensions;

public static class CarterExtensions
{
    public static IServiceCollection AddCarterWithAssemblies
        (this IServiceCollection services, params Assembly[] assemblies)
    {


        services.AddCarter(configurator: configurationBinder =>
        {
            foreach (var assembly in assemblies)
            {
                var moduleTypes = assembly.GetTypes()
                    .Where(t => t.IsAssignableTo(typeof(ICarterModule))).ToArray();

                configurationBinder.WithModules(moduleTypes);
            }
        });


        return services;
    }
}
