using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using AspNetFrameworkHosting.Internals;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetFrameworkHosting;

public sealed class DependencyInjectionResolver : IDependencyResolver
{
    private readonly IServiceProvider serviceProvider;

    private bool isDisposed;

    public DependencyInjectionResolver(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    ~DependencyInjectionResolver()
    {
        this.Dispose(false);
    }

    public object GetService(Type serviceType)
    {
        return this.serviceProvider.GetService(serviceType);
    }

    public IEnumerable<object?> GetServices(Type serviceType)
    {
        return this.serviceProvider.GetServices(serviceType);
    }

    public IDependencyScope BeginScope()
    {
        return new DependencyScope(this.serviceProvider);
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (this.isDisposed)
        {
            return;
        }

        if (disposing)
        {
            /*if (this.serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }*/
        }

        this.isDisposed = true;
    }
}
