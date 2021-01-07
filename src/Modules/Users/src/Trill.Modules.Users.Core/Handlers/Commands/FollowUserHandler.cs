using System;
using System.Threading.Tasks;
using Trill.Modules.Users.Core.Commands;
using Trill.Modules.Users.Core.Domain.Entities;
using Trill.Modules.Users.Core.Domain.Repositories;
using Trill.Modules.Users.Core.Events;
using Trill.Modules.Users.Core.Exceptions;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Messaging;
using Trill.Shared.Kernel.BuildingBlocks;

namespace Trill.Modules.Users.Core.Handlers.Commands
{
    internal sealed class FollowUserHandler : ICommandHandler<FollowUser>
    {
        private readonly IFollowerRepository _followerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMessageBroker _messageBroker;

        public FollowUserHandler(IFollowerRepository  followerRepository, IUserRepository userRepository,
            IMessageBroker messageBroker)
        {
            _followerRepository = followerRepository;
            _userRepository = userRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(FollowUser command)
        {
            if (command.UserId == command.FolloweeId)
            {
                throw new FollowerAndFolloweeIdAreTheSameException(command.UserId);
            }
            
            var follower = await _followerRepository.GetAsync(command.UserId, command.FolloweeId);
            if (follower is {})
            {
                return;
            }

            var followee = await _userRepository.GetAsync(command.FolloweeId);
            if (followee is null)
            {
                throw new UserNotFoundException(command.FolloweeId);
            }

            await _followerRepository.AddAsync(new Follower(new AggregateId(), command.UserId, command.FolloweeId,
                DateTime.UtcNow));
            await _messageBroker.PublishAsync(new UserFollowed(command.UserId, command.FolloweeId));
        }
    }
}