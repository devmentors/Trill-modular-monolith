using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Trill.Shared.Infrastructure.Logging.Options;
using Trill.Shared.Infrastructure.Web;

namespace Trill.Shared.Infrastructure.Logging
{
    internal static class Extensions
    {
        private const string AppSectionName = "app";
        private const string LoggerSectionName = "logger";
        
        public static IHostBuilder UseLogging(this IHostBuilder builder,
            Action<LoggerConfiguration> configure = null, string loggerSectionName = LoggerSectionName,
            string appSectionName = AppSectionName)
            => builder.UseSerilog((context, loggerConfiguration) =>
            {
                if (string.IsNullOrWhiteSpace(loggerSectionName))
                {
                    loggerSectionName = LoggerSectionName;
                }

                if (string.IsNullOrWhiteSpace(appSectionName))
                {
                    appSectionName = AppSectionName;
                }

                var appOptions = context.Configuration.GetOptions<AppOptions>(appSectionName);
                var loggerOptions = context.Configuration.GetOptions<LoggerOptions>(loggerSectionName);

                MapOptions(loggerOptions, appOptions, loggerConfiguration, context.HostingEnvironment.EnvironmentName);
                configure?.Invoke(loggerConfiguration);
            });
        
                private static void MapOptions(LoggerOptions loggerOptions, AppOptions appOptions,
            LoggerConfiguration loggerConfiguration, string environmentName)
        {
            var level = GetLogEventLevel(loggerOptions.Level);

            loggerConfiguration.Enrich.FromLogContext()
                .MinimumLevel.Is(level)
                .Enrich.WithProperty("Environment", environmentName)
                .Enrich.WithProperty("Application", appOptions.Name)
                .Enrich.WithProperty("Instance", appOptions.Instance)
                .Enrich.WithProperty("Version", appOptions.Version);

            foreach (var (key, value) in loggerOptions.Tags ?? new Dictionary<string, object>())
            {
                loggerConfiguration.Enrich.WithProperty(key, value);
            }

            foreach (var (key, value) in loggerOptions.MinimumLevelOverrides ?? new Dictionary<string, string>())
            {
                var logLevel = GetLogEventLevel(value);
                loggerConfiguration.MinimumLevel.Override(key, logLevel);
            }

            loggerOptions.ExcludePaths?.ToList().ForEach(p => loggerConfiguration.Filter
                .ByExcluding(Matching.WithProperty<string>("RequestPath", n => n.EndsWith(p))));

            loggerOptions.ExcludeProperties?.ToList().ForEach(p => loggerConfiguration.Filter
                .ByExcluding(Matching.WithProperty(p)));

            Configure(loggerConfiguration, loggerOptions);
        }

        private static void Configure(LoggerConfiguration loggerConfiguration, LoggerOptions options)
        {
            var consoleOptions = options.Console ?? new ConsoleOptions();
            var fileOptions = options.File ?? new FileOptions();
            var seqOptions = options.Seq ?? new SeqOptions();

            if (consoleOptions.Enabled)
            {
                loggerConfiguration.WriteTo.Console();
            }

            if (fileOptions.Enabled)
            {
                var path = string.IsNullOrWhiteSpace(fileOptions.Path) ? "logs/logs.txt" : fileOptions.Path;
                if (!Enum.TryParse<RollingInterval>(fileOptions.Interval, true, out var interval))
                {
                    interval = RollingInterval.Day;
                }

                loggerConfiguration.WriteTo.File(path, rollingInterval: interval);
            }
            if (seqOptions.Enabled)
            {
                loggerConfiguration.WriteTo.Seq(seqOptions.Url, apiKey: seqOptions.ApiKey);
            }
        }

        private static LogEventLevel GetLogEventLevel(string level)
            => Enum.TryParse<LogEventLevel>(level, true, out var logLevel) 
                ? logLevel
                : LogEventLevel.Information;
    }
}