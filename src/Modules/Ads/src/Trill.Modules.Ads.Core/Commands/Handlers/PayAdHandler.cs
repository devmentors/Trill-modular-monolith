using System.Threading.Tasks;
using Trill.Modules.Ads.Core.Clients.Users;
using Trill.Modules.Ads.Core.Domain;
using Trill.Modules.Ads.Core.Domain.Exceptions;
using Trill.Modules.Ads.Core.Events;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Ads.Core.Commands.Handlers
{
    internal sealed class PayAdHandler : ICommandHandler<PayAd>
    {
        private readonly IAdRepository _adRepository;
        private readonly IUsersApiClient _usersApiClient;
        private readonly IMessageBroker _messageBroker;

        public PayAdHandler(IAdRepository adRepository, IUsersApiClient usersApiClient, IMessageBroker messageBroker)
        {
            _adRepository = adRepository;
            _usersApiClient = usersApiClient;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(PayAd command)
        {
            var ad = await _adRepository.GetAsync(command.AdId);
            if (ad is null)
            {
                throw new AdNotFoundException(command.AdId);
            }

            ad.Pay();
            var fundsCharged = await _usersApiClient.ChargeFundsAsync(ad.UserId, ad.Amount);
            if (!fundsCharged)
            {
                throw new CannotPayAdException(ad.Id);
            }
            
            await _adRepository.UpdateAsync(ad);
            await _messageBroker.PublishAsync(new AdPaid(ad.Id));
        }
    }
}