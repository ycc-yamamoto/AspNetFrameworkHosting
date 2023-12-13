using System;
using Microsoft.Extensions.Logging;

namespace SampleWebApp.Extensions;

public static partial class LoggerExtension
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "{Method} {Path}")]
    public static partial void ReceivedHttpRequest(this ILogger logger, string method, string path);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Do periodic action. ({DateTime:yyyy-MM-dd HH:mm:ss})")]
    public static partial void DoActionPeriodicBackgroundProcess(this ILogger logger, DateTime dateTime);
}
