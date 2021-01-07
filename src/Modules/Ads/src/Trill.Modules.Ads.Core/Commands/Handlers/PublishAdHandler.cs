using System.Threading.Tasks;
using Trill.Modules.Ads.Core.Clients.Stories;
using Trill.Modules.Ads.Core.Clients.Stories.Requests;
using Trill.Modules.Ads.Core.Domain;
using Trill.Modules.Ads.Core.Domain.Exceptions;
using Trill.Modules.Ads.Core.Events;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Ads.Core.Commands.Handlers
{
    internal sealed class PublishAdHandler : ICommandHandler<PublishAd>
    {
        private readonly IAdRepository _adRepository;
        private readonly IStoryApiClient _storyApiClient;
        private readonly IMessageBroker _messageBroker;

        public PublishAdHandler(IAdRepository adRepository, IStoryApiClient storyApiClient,
            IMessageBroker messageBroker)
        {
            _adRepository = adRepository;
            _storyApiClient = storyApiClient;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(PublishAd command)
        {
            var ad = await _adRepository.GetAsync(command.AdId);
            if (ad is null)
            {
                throw new AdNotFoundException(command.AdId);
            }

            ad.Publish();
            var storyId = await _storyApiClient.SendStoryAsync(new SendStory(default, ad.UserId,
                ad.Header, ad.Content, ad.Tags, ad.From, ad.To, true));
            if (storyId is null)
            {
                throw new CannotPublishAdAException(command.AdId);
            }

            ad.SetStoryId(storyId.Value);
            await _adRepository.UpdateAsync(ad);
            await _messageBroker.PublishAsync(new AdPublished(ad.Id));
        }
    }
}