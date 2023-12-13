using System;
using System.Net.Http.Formatting;
using System.Web.Http;
using AspNetFrameworkHosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NSwag.AspNet.Owin;
using Owin;
using SampleWebApp.Services;
using SampleWebApp.Services.Impl;
using Serilog;

var builder = AspNetWebApplication.CreateBuilder();

builder.ConfigureServices((context, services) =>
{
    services.AddHostedService<PeriodicBackgroundService>();
    services.AddSingleton<IUserStoreService, UserStoreService>();

    var startOptions = new StartOptions(context.Configuration["Environment:SiteUrl"]);

    // 必ず最後に呼ぶ
    services.AddAspNetFrameworkHosting(startOptions, appBuilder =>
    {
        using var config = new HttpConfiguration();

        config.DependencyResolver = new DependencyInjectionResolver(services.BuildServiceProvider());
        config.Formatters.Clear();
        config.Formatters.Add(new JsonMediaTypeFormatter());
        config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        if (context.HostingEnvironment.IsDevelopment())
        {
            appBuilder.UseSwaggerUi3(typeof(Program).Assembly, setting =>
            {
                var swaggerSettings = context.Configuration.GetSection("Swagger:Version1.0");

                setting.GeneratorSettings.Title = swaggerSettings["Title"];
                setting.GeneratorSettings.Description = swaggerSettings["Description"];
                setting.GeneratorSettings.Version = swaggerSettings["Version"];
                setting.TransformToExternalPath = (internalUiRoute, request) =>
                {
                    var path = internalUiRoute.EndsWith("swagger.json", StringComparison.InvariantCulture) &&
                               !string.IsNullOrEmpty(request.PathBase.Value)
                        ? $"{request.PathBase.Value}{internalUiRoute}"
                        : internalUiRoute;

                    return path;
                };
            });
            appBuilder.UseSwaggerReDoc(typeof(Program).Assembly, setting => { setting.Path = "/redoc"; });
        }

        config.MapHttpAttributeRoutes();
        config.EnsureInitialized();
        appBuilder.UseWebApi(config);
    });
});
builder.ConfigureLogging((context, logging) =>
{
    logging.ClearProviders();

    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(context.Configuration.GetSection("Logging"))
        .CreateLogger();

    logging.AddSerilog(logger: logger, dispose: true);
});

var app = builder.Build();

app.Run();
