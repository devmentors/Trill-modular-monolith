using System;
using System.Threading.Tasks;
using Trill.Modules.Ads.Core.Domain;
using Trill.Modules.Ads.Core.Events;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Ads.Core.Commands.Handlers
{
    internal sealed class CreateAdHandler : ICommandHandler<CreateAd>
    {
        private readonly IAdRepository _adRepository;
        private readonly IMessageBroker _messageBroker;

        public CreateAdHandler(IAdRepository adRepository, IMessageBroker messageBroker)
        {
            _adRepository = adRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(CreateAd command)
        {
            var ad = new Ad(command.AdId, command.UserId, command.Header, command.Content, command.Tags,
                AdState.New, command.From, command.To, DateTime.UtcNow);
            await _adRepository.AddAsync(ad);
            await _messageBroker.PublishAsync(new AdCreated(command.AdId));
        }
    }
}