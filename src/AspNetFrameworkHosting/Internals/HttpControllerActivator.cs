using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace AspNetFrameworkHosting.Internals;

internal sealed class HttpControllerActivator : IHttpControllerActivator
{
    private readonly IServiceProvider serviceProvider;

    public HttpControllerActivator(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
    {
        return (IHttpController)this.serviceProvider.GetService(controllerType);
    }
}
