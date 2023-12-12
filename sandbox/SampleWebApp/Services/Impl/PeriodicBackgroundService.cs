using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SampleWebApp.Services.Impl;

public sealed class PeriodicBackgroundService : BackgroundService
{
    private readonly ILogger<PeriodicBackgroundService> logger;

    private Timer? timer;

    public PeriodicBackgroundService(ILogger<PeriodicBackgroundService> logger)
    {
        this.logger = logger;
    }

    public override void Dispose()
    {
        base.Dispose();
        this.timer?.Dispose();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this.timer = new Timer((_) => this.PeriodicAction(), default, TimeSpan.Zero, TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    private void PeriodicAction()
    {
        this.logger.LogInformation($"Do periodic action. ({DateTime.Now:yyyy-MM-dd HH:mm:ss})");
    }
}
