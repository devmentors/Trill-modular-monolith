using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Messaging;
using Trill.Shared.Infrastructure.Mongo;

namespace Trill.Shared.Infrastructure.Messaging.Inbox
{
    internal sealed class MongoInbox : IInbox
    {
        private readonly IMongoSessionFactory _sessionFactory;
        private readonly IMongoDatabase _database;
        private readonly ILogger<MongoInbox> _logger;
        private readonly bool _transactionsEnabled;
        private readonly string _collectionName;

        public bool Enabled { get; }

        public MongoInbox(IMongoSessionFactory sessionFactory, InboxOptions inboxOptions, MongoOptions mongoOptions,
            IMongoDatabase database, ILogger<MongoInbox> logger)
        {
            _sessionFactory = sessionFactory;
            _database = database;
            _logger = logger;
            _transactionsEnabled = !mongoOptions.DisableTransactions;
            Enabled = inboxOptions.Enabled;
            _collectionName = string.IsNullOrWhiteSpace(inboxOptions.CollectionName)
                ? "inbox"
                : inboxOptions.CollectionName;
        }

        public async Task HandleAsync(IMessage message, Func<Task> handler, string module)
        {
            if (!Enabled)
            {
                _logger.LogWarning("Inbox is disabled, incoming messages won't be processed.");
                return;
            }

            if (message.Id == Guid.Empty)
            {
                // A synchronous request
                await handler();
                return;
            }

            var collection = _database.GetCollection<InboxMessage>($"{module}-module.{_collectionName}");
            _logger.LogTrace($"Received a message with ID: '{message.Id}' to be processed.");
            if (await collection.AsQueryable().AnyAsync(x => x.Id == message.Id))
            {
                _logger.LogWarning($"Message with ID: '{message.Id}' was already processed.");
                return;
            }

            IClientSessionHandle session = null;
            if (_transactionsEnabled)
            {
                session = await _sessionFactory.CreateAsync();
                session.StartTransaction();
            }

            try
            {
                _logger.LogTrace($"Handling a message with ID: '{message.Id}'...");
                await handler();
                await collection.InsertOneAsync(new InboxMessage
                {
                    Id = message.Id,
                    CorrelationId = message.CorrelationId,
                    Name = message.GetType().Name.Underscore(),
                    Module = message.GetModuleName(),
                    Timestamp = DateTime.UtcNow.ToUnixTimeMilliseconds()
                });
                if (session is {})
                {
                    await session.CommitTransactionAsync();
                }

                _logger.LogTrace($"Handled a message with ID: '{message.Id}'.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"There was an error when handling a message with ID: '{message.Id}'.");
                if (session is {})
                {
                    await session.AbortTransactionAsync();
                }

                throw;
            }
            finally
            {
                session?.Dispose();
            }
        }

        private class InboxMessage
        {
            public Guid Id { get; set; }
            public Guid CorrelationId { get; set; }
            public string Name { get; set; }
            public string Module { get; set; }
            public long Timestamp { get; set; }
        }
    }
}