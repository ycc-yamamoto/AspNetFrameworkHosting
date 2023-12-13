﻿using System;
using System.Globalization;
using System.IO;
using System.Net.Http.Formatting;
using System.Reflection;
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
using Serilog.Events;

var startOptions = new StartOptions("http://localhost:5000");
var builder = AspNetWebApplication.CreateBuilder();

builder.ConfigureServices(services =>
{
    services.AddHostedService<PeriodicBackgroundService>();
    services.AddSingleton<IUserStoreService, UserStoreService>();

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
        appBuilder.UseSwaggerUi3(typeof(Program).Assembly, setting =>
        {
            // setting.SwaggerRoutes.Add(new SwaggerUi3Route("v1", "/swagger/v1/swagger.json"));
            setting.GeneratorSettings.Title = "SampleWebApp";
            setting.GeneratorSettings.Description = "SampleWebApp";
            setting.GeneratorSettings.Version = "v1";
            setting.TransformToExternalPath = (internalUiRoute, request) =>
            {
                var path = internalUiRoute.EndsWith("swagger.json", StringComparison.InvariantCulture) &&
                           !string.IsNullOrEmpty(request.PathBase.Value)
                    ? $"{request.PathBase.Value}{internalUiRoute}"
                    : internalUiRoute;

                return path;
            };
        });
        appBuilder.UseSwaggerReDoc(typeof(Program).Assembly, setting =>
        {
            setting.Path = "/redoc";
        });
        config.MapHttpAttributeRoutes();
        config.EnsureInitialized();
        appBuilder.UseWebApi(config);
    });
});
builder.ConfigureLogging((_, logging) =>
{
    logging.ClearProviders();

    var baseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
    var logger = new LoggerConfiguration()
        .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
        .WriteTo.Async(w => w.File(
            $"{Path.Combine(baseDirectory, "logs", "log-.log")}",
            restrictedToMinimumLevel: LogEventLevel.Information,
            outputTemplate:
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{ThreadId}] {Message:lj}{NewLine}{Exception}",
            rollingInterval: RollingInterval.Day,
            formatProvider: CultureInfo.InvariantCulture))
        .Enrich.WithThreadId()
        .CreateLogger();

    logging.AddSerilog(logger: logger, dispose: true);
});

var app = builder.Build();

app.Run();
