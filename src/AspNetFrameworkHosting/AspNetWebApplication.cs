using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Owin.Hosting;
using Owin;

namespace AspNetFrameworkHosting;

public sealed class AspNetWebApplication : IHost, IAsyncDisposable
{
    private readonly IHost host;

    internal AspNetWebApplication(IHost host)
    {
        this.host = host;
    }

    public IServiceProvider Services => this.host.Services;

    public IConfiguration Configuration => this.host.Services.GetRequiredService<IConfiguration>();

    public IHostEnvironment Environment => this.host.Services.GetRequiredService<IHostEnvironment>();

    public IHostApplicationLifetime ApplicationLifetime => this.host.Services.GetRequiredService<IHostApplicationLifetime>();

    public static AspNetWebApplicationBuilder CreateBuilder()
    {
        var builder = new AspNetWebApplicationBuilder();

        return builder;
    }

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        return this.host.StartAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        return this.host.StopAsync(cancellationToken);
    }

    public void Dispose()
    {
        this.host.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return ((IAsyncDisposable)this.host).DisposeAsync();
    }
}
