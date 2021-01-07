using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Trill.Shared.Infrastructure.Messaging.Outbox
{
    internal class OutboxProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<OutboxProcessor> _logger;
        private readonly TimeSpan _interval;
        private readonly bool _enabled;

        public OutboxProcessor(IServiceScopeFactory serviceScopeFactory, OutboxOptions outboxOptions,
            ILogger<OutboxProcessor> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _interval = outboxOptions.Interval ?? TimeSpan.FromSeconds(1);
            _enabled = outboxOptions.Enabled;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_enabled)
            {
                _logger.LogInformation("Outbox is disabled");
                return;
            }

            _logger.LogInformation("Outbox is enabled");
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogTrace("Started processing outbox messages...");
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    try
                    {
                        var outbox = scope.ServiceProvider.GetRequiredService<IOutbox>();
                        await outbox.PublishUnsentAsync();
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError("There was an error when processing outbox.");
                        _logger.LogError(exception, exception.Message);
                    }
                }

                stopwatch.Stop();
                _logger.LogTrace($"Finished processing outbox messages in {stopwatch.ElapsedMilliseconds} ms.");
                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}