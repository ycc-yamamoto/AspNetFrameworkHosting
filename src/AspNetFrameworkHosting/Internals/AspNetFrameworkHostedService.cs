#pragma warning disable CA1812 // Avoid uninstantiated internal classes
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Owin.Hosting;
using Owin;

namespace AspNetFrameworkHosting.Internals;

internal sealed class AspNetFrameworkHostedService : IHostedService, IDisposable
{
    private readonly IHostApplicationLifetime applicationLifetime;

    private readonly StartOptions startOptions;

    private readonly Action<IAppBuilder> startup;

    private IDisposable? disposable;

    private bool isDisposed;

    public AspNetFrameworkHostedService(
        IHostApplicationLifetime applicationLifetime,
        StartOptions startOptions,
        Action<IAppBuilder> startup)
    {
        this.applicationLifetime = applicationLifetime;
        this.startOptions = startOptions;
        this.startup = startup;
    }

    ~AspNetFrameworkHostedService()
    {
        this.Dispose(false);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.disposable = WebApp.Start(this.startOptions, this.startup);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        this.applicationLifetime.StopApplication();
        return Task.CompletedTask;
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
            this.disposable?.Dispose();
        }

        this.isDisposed = true;
    }
}
