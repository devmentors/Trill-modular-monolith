using System.Threading.Tasks;
using Trill.Modules.Users.Core.Commands;
using Trill.Modules.Users.Core.Domain.Repositories;
using Trill.Modules.Users.Core.Events;
using Trill.Modules.Users.Core.Exceptions;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Users.Core.Handlers.Commands
{
    internal sealed class UnfollowUserHandler : ICommandHandler<UnfollowUser>
    {
        private readonly IFollowerRepository _followerRepository;
        private readonly IMessageBroker _messageBroker;

        public UnfollowUserHandler(IFollowerRepository  followerRepository, IMessageBroker messageBroker)
        {
            _followerRepository = followerRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(UnfollowUser command)
        {
            if (command.UserId == command.FolloweeId)
            {
                throw new FollowerAndFolloweeIdAreTheSameException(command.UserId);
            }
            
            var follower = await _followerRepository.GetAsync(command.UserId, command.FolloweeId);
            if (follower is null)
            {
                return;
            }

            await _followerRepository.DeleteAsync(follower.Id);
            await _messageBroker.PublishAsync(new UserUnfollowed(command.UserId, command.FolloweeId));
        }
    }
}