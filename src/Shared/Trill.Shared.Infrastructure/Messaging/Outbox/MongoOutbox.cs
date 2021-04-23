using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Messaging;
using Trill.Shared.Abstractions.Modules;
using Trill.Shared.Abstractions.Time;
using Trill.Shared.Infrastructure.Messaging.Dispatchers;
using Trill.Shared.Infrastructure.Modules;

namespace Trill.Shared.Infrastructure.Messaging.Outbox
{
    internal sealed class MongoOutbox : IOutbox
    {
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter()}
        };
        
        private readonly IMongoDatabase _database;
        private readonly IModuleClient _moduleClient;
        private readonly IClock _clock;
        private readonly IAsyncMessageDispatcher _asyncMessageDispatcher;
        private readonly ILogger<MongoOutbox> _logger;
        private readonly string _collectionName;
        private readonly string[] _modules;
        private readonly bool _useBackgroundDispatcher;

        public bool Enabled { get; }

        public MongoOutbox(IMongoDatabase database, IModuleRegistry moduleRegistry, OutboxOptions outboxOptions, 
            MessagingOptions messagingOptions, IModuleClient moduleClient, IClock clock,
            IAsyncMessageDispatcher asyncMessageDispatcher, ILogger<MongoOutbox> logger)
        {
            _database = database;
            _moduleClient = moduleClient;
            _clock = clock;
            _asyncMessageDispatcher = asyncMessageDispatcher;
            _logger = logger;
            Enabled = outboxOptions.Enabled;
            _modules = moduleRegistry.Modules.ToArray();
            _useBackgroundDispatcher = messagingOptions.UseBackgroundDispatcher;
            _collectionName = string.IsNullOrWhiteSpace(outboxOptions.CollectionName)
                ? "outbox"
                : outboxOptions.CollectionName;
        }

        public async Task SaveAsync(params IMessage[] messages)
        {
            if (!Enabled)
            {
                _logger.LogWarning("Outbox is disabled, outgoing messages won't be saved.");
                return;
            }

            if (messages is null || !messages.Any())
            {
                _logger.LogWarning("No messages have been provided to be saved to the outbox.");
                return;
            }

            var outboxMessages = messages.Where(x => x is {})
                .Select(x => new OutboxMessage
                {
                    Id = x.Id,
                    CorrelationId = x.CorrelationId,
                    Name = x.GetType().Name.Underscore(),
                    Payload = JsonSerializer.Serialize(x, SerializerOptions),
                    Type = x.GetType().AssemblyQualifiedName,
                    ReceivedAt = _clock.Current().ToUnixTimeMilliseconds()
                }).ToArray();

            if (!outboxMessages.Any())
            {
                _logger.LogWarning("No messages have been provided to be saved to the outbox.");
            }

            var module = messages[0].GetModuleName();
            await _database.GetCollection<OutboxMessage>($"{module}-module.{_collectionName}").InsertManyAsync(outboxMessages);
            _logger.LogInformation($"Saved {outboxMessages.Length} messages to the outbox ('{module}').");
        }

        public Task PublishUnsentAsync()
            => Task.WhenAll(_modules.Select(PublishUnsentAsync));

        private async Task PublishUnsentAsync(string module)
        {
            var collection = _database.GetCollection<OutboxMessage>($"{module}-module.{_collectionName}");
            var unsentMessages = await collection.AsQueryable()
                .Where(x => x.SentAt == null)
                .ToListAsync();

            if (!unsentMessages.Any())
            {
                _logger.LogTrace($"No unsent messages found in outbox ('{module}').");
                return;
            }

            _logger.LogInformation($"Found {unsentMessages.Count} unsent messages in outbox ('{module}'), sending...");

            foreach (var outboxMessage in unsentMessages)
            {
                var type = Type.GetType(outboxMessage.Type);
                var message = JsonSerializer.Deserialize(outboxMessage.Payload, type, SerializerOptions) as IMessage;
                if (message is null)
                {
                    _logger.LogError($"Invalid message type: {type?.Name} (cannot cast to {nameof(IMessage)}).");
                    continue;
                }

                message.Id = outboxMessage.Id;
                message.CorrelationId = outboxMessage.CorrelationId;

                var name = message.GetType().Name.Underscore();
                _logger.LogInformation($"Publishing a message: '{name}' with ID: '{message.Id}' (outbox)...");

                if (_useBackgroundDispatcher)
                {
                    await _asyncMessageDispatcher.PublishAsync(message);
                }
                else
                {
                    await _moduleClient.PublishAsync(message);
                }

                outboxMessage.SentAt = _clock.Current().ToUnixTimeMilliseconds();
                await collection.ReplaceOneAsync(x => x.Id == outboxMessage.Id, outboxMessage);
                _logger.LogInformation($"Published a message: '{name}' with ID: '{message.Id} (outbox)'.");
            }
        }

        private class OutboxMessage
        {
            public Guid Id { get; set; }
            public Guid CorrelationId { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Payload { get; set; }
            public long ReceivedAt { get; set; }
            public long? SentAt { get; set; }
        }
    }
}