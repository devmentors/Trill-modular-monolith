using System.Threading.Tasks;
using Trill.Modules.Ads.Core.Domain;
using Trill.Modules.Ads.Core.Domain.Exceptions;
using Trill.Modules.Ads.Core.Events;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Ads.Core.Commands.Handlers
{
    internal sealed class ApproveAdHandler : ICommandHandler<ApproveAd>
    {
        private readonly IAdRepository _adRepository;
        private readonly IMessageBroker _messageBroker;

        public ApproveAdHandler(IAdRepository adRepository, IMessageBroker messageBroker)
        {
            _adRepository = adRepository;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(ApproveAd command)
        {
            var ad = await _adRepository.GetAsync(command.AdId);
            if (ad is null)
            {
                throw new AdNotFoundException(command.AdId);
            }

            ad.Approve();
            await _adRepository.UpdateAsync(ad);
            await _messageBroker.PublishAsync(new AdApproved(ad.Id));
        }
    }
}