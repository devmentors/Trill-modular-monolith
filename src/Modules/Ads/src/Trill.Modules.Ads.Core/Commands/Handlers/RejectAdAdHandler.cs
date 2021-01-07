using System.Threading.Tasks;
using Trill.Modules.Ads.Core.Domain;
using Trill.Modules.Ads.Core.Domain.Exceptions;
using Trill.Modules.Ads.Core.Events;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Ads.Core.Commands.Handlers
{
    internal sealed class RejectAdAdHandler : ICommandHandler<RejectAd>
    {
        private readonly IAdRepository _adRepository;
        private readonly IMessageBroker _messageBroker;

        public RejectAdAdHandler(IAdRepository adRepository, IMessageBroker messageBroker)
        {
            _adRepository = adRepository;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(RejectAd command)
        {
            var ad = await _adRepository.GetAsync(command.AdId);
            if (ad is null)
            {
                throw new AdNotFoundException(command.AdId);
            }

            ad.Reject();
            await _adRepository.UpdateAsync(ad);
            await _messageBroker.PublishAsync(new AdRejected(ad.Id));
        }
    }
}