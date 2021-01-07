using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Trill.Modules.Users.Core.Commands;
using Trill.Modules.Users.Core.Domain.Entities;
using Trill.Modules.Users.Core.Domain.Exceptions;
using Trill.Modules.Users.Core.Domain.Repositories;
using Trill.Modules.Users.Core.Events;
using Trill.Modules.Users.Core.Services;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Generators;
using Trill.Shared.Abstractions.Messaging;
using Trill.Shared.Kernel.BuildingBlocks;

namespace Trill.Modules.Users.Core.Handlers.Commands
{
    internal sealed class SignInHandler : ICommandHandler<SignIn>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRng _rng;
        private readonly ITokenStorage _storage;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<SignInHandler> _logger;

        public SignInHandler(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository,
            IPasswordService passwordService, IJwtProvider jwtProvider, IRng rng, ITokenStorage storage,
            IMessageBroker messageBroker, ILogger<SignInHandler> logger)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _passwordService = passwordService;
            _jwtProvider = jwtProvider;
            _rng = rng;
            _storage = storage;
            _messageBroker = messageBroker;
            _logger = logger;
        }

        public async Task HandleAsync(SignIn command)
        {
            var user = await _userRepository.GetByNameAsync(command.Name);
            if (user is null || !_passwordService.IsValid(user.Password, command.Password))
            {
                _logger.LogError($"User with name: {command.Name} was not found.");
                throw new InvalidCredentialsException(command.Name);
            }

            if (user.Locked)
            {
                throw new UserLockedException(user.Id);
            }

            var claims = user.Permissions.Any()
                ? new Dictionary<string, IEnumerable<string>>
                {
                    ["permissions"] = user.Permissions
                }
                : null;
            var auth = _jwtProvider.Create(user.Id, user.Name, user.Role, claims: claims);
            auth.RefreshToken = await CreateRefreshTokenAsync(user.Id);
            _storage.Set(command.Id, auth);
            _logger.LogInformation($"User with ID: {user.Id} has been authenticated.");
            await _messageBroker.PublishAsync(new SignedIn(user.Id));
        }

        private async Task<string> CreateRefreshTokenAsync(Guid userId)
        {
            var token = _rng.Generate(30, true);
            var refreshToken = new RefreshToken(new AggregateId(), userId, token, DateTime.UtcNow);
            await _refreshTokenRepository.AddAsync(refreshToken);

            return token;
        }
    }
}