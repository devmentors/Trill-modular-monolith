using Microsoft.Extensions.DependencyInjection;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Events;
using Trill.Shared.Abstractions.Messaging;
using Trill.Shared.Infrastructure.Messaging.Brokers;
using Trill.Shared.Infrastructure.Messaging.Dispatchers;
using Trill.Shared.Infrastructure.Messaging.Inbox;
using Trill.Shared.Infrastructure.Messaging.Outbox;

namespace Trill.Shared.Infrastructure.Messaging
{
    internal static class Extensions
    {
        private const string SectionName = "messaging";

        public static IServiceCollection AddMessaging(this IServiceCollection services,
            string sectionName = SectionName)
        {
            if (string.IsNullOrWhiteSpace(sectionName))
            {
                sectionName = SectionName;
            }

            var messagingOptions = services.GetOptions<MessagingOptions>(sectionName);
            var inboxOptions = services.GetOptions<InboxOptions>($"{sectionName}:inbox");
            var outboxOptions = services.GetOptions<OutboxOptions>($"{sectionName}:outbox");
            services
                .AddSingleton(messagingOptions)
                .AddSingleton(inboxOptions)
                .AddSingleton(outboxOptions)
                .AddSingleton<IAsyncMessageDispatcher, AsyncMessageDispatcher>()
                .AddTransient<IInbox, MongoInbox>()
                .AddTransient<IOutbox, MongoOutbox>()
                .AddScoped<IMessageBroker, InMemoryMessageBroker>();

            if (inboxOptions.Enabled)
            {
                services.TryDecorate(typeof(ICommandHandler<>), typeof(InboxCommandHandlerDecorator<>));
                services.TryDecorate(typeof(IEventHandler<>), typeof(InboxEventHandlerDecorator<>));
            }

            if (outboxOptions.Enabled)
            {
                services.AddHostedService<OutboxProcessor>();
            }

            if (messagingOptions.UseBackgroundDispatcher)
            {
                services.AddHostedService<BackgroundDispatcher>();
            }

            return services;
        }
    }
}