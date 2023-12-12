using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using AspNetFrameworkHosting.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Owin.Hosting;
using Owin;

namespace AspNetFrameworkHosting;

public static class ServiceCollectionExtension
{
    public static void AddAspNetFrameworkHosting(this IServiceCollection services, StartOptions startOptions, Action<IAppBuilder> startup)
    {
        var assembly = Assembly.GetCallingAssembly();
        var exportedTypes = assembly.GetExportedTypes();
        var controllerTypes = exportedTypes.Where(type => !type.IsAbstract && !type.IsGenericTypeDefinition)
            .Where(type => typeof(ApiController).IsAssignableFrom(type) ||
                           type.Name.EndsWith("Controller", StringComparison.InvariantCulture));

        foreach (var controllerType in controllerTypes)
        {
            services.AddTransient(controllerType);
        }

        services.AddSingleton<IHttpControllerActivator, HttpControllerActivator>();
        services.AddSingleton(startOptions);
        services.AddSingleton(startup);
    }
}
