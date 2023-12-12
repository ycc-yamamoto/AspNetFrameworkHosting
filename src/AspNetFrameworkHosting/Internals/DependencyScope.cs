using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetFrameworkHosting.Internals;

internal sealed class DependencyScope : IDependencyScope
{
    private readonly IServiceScope scope;

    private bool isDisposed;

    public DependencyScope(IServiceProvider serviceProvider)
    {
        this.scope = serviceProvider.CreateScope();
    }

    ~DependencyScope()
    {
        this.Dispose(false);
    }

    public object GetService(Type serviceType)
    {
        return this.scope.ServiceProvider.GetService(serviceType);
    }

    public IEnumerable<object?> GetServices(Type serviceType)
    {
        return this.scope.ServiceProvider.GetServices(serviceType);
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
            this.scope.Dispose();
        }

        this.isDisposed = true;
    }
}
