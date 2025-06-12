using System.Reflection;
using AspLib.RequestPipeline.Factory;
using AspLib.RequestPipeline.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AspLib.RequestPipeline;

public static class Extensions
{
    public static IServiceCollection RegisterPipeline(
        this IServiceCollection services,
        Assembly? assembly = null
    )
    {
        services.AddTransient<IRequestHandlerFactory, RequestHandlerFactory>();
        services.AddTransient(typeof(IRequestPipeline<,>), typeof(PrioritizedPipeline<,>));

        Assembly[] assemblies = assembly is null
            ? AppDomain.CurrentDomain.GetAssemblies()
            : [assembly];

        FromAssembly(services, assemblies);

        return services;
    }

    private static IServiceCollection FromAssembly(
        IServiceCollection services,
        Assembly[] assemblies
    )
    {
        Type prioritizedHandlerInterfaceType = typeof(IPrioritizedRequestHandler<,>);
        Type handlerInterfaceType = typeof(IRequestHandler<,>);
        var handlers = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t is { IsAbstract: false, IsInterface: false })
            .SelectMany(t =>
                t.GetInterfaces()
                    .Where(i =>
                        i.IsGenericType
                        && (
                            i.GetGenericTypeDefinition() == handlerInterfaceType
                            || i.GetGenericTypeDefinition() == prioritizedHandlerInterfaceType
                        )
                    )
                    .Select(i => new { Interface = i, Implementation = t })
            )
            .ToList();

        foreach (var handler in handlers)
        {
            services.AddTransient(handler.Interface, handler.Implementation);
        }
        return services;
    }
}
