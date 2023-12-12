using System;
using AspNetFrameworkHosting.Internals;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetFrameworkHosting;

public sealed class AspNetWebApplicationBuilder
{
    private readonly HostBuilder builder;

    internal AspNetWebApplicationBuilder()
    {
        this.builder = new HostBuilder();
    }

    public AspNetWebApplication Build()
    {
        this.builder.ConfigureServices(services =>
        {
            services.AddHostedService<AspNetFrameworkHostedService>();
        });

        var app = new AspNetWebApplication(this.builder.Build());

        return app;
    }

    public AspNetWebApplicationBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
    {
        this.builder.ConfigureAppConfiguration(configureDelegate);
        return this;
    }

    public AspNetWebApplicationBuilder ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate)
    {
        this.builder.ConfigureContainer(configureDelegate);
        return this;
    }

    public AspNetWebApplicationBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
    {
        this.builder.ConfigureHostConfiguration(configureDelegate);
        return this;
    }

    public AspNetWebApplicationBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
    {
        this.builder.ConfigureServices(configureDelegate);
        return this;
    }

    public AspNetWebApplicationBuilder ConfigureServices(Action<IServiceCollection> configureDelegate)
    {
        this.builder.ConfigureServices(configureDelegate);
        return this;
    }

    public AspNetWebApplicationBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory)
        where TContainerBuilder : notnull
    {
        this.builder.UseServiceProviderFactory(factory);
        return this;
    }

    public AspNetWebApplicationBuilder UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory)
        where TContainerBuilder : notnull
    {
        this.builder.UseServiceProviderFactory(factory);
        return this;
    }
}
