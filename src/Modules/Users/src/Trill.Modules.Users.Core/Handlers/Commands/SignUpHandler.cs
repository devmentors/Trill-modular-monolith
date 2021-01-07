using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Trill.Modules.Users.Core.Commands;
using Trill.Modules.Users.Core.Domain.Entities;
using Trill.Modules.Users.Core.Domain.Exceptions;
using Trill.Modules.Users.Core.Domain.Repositories;
using Trill.Modules.Users.Core.Events;
using Trill.Modules.Users.Core.Services;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Users.Core.Handlers.Commands
{
    internal sealed class SignUpHandler : ICommandHandler<SignUp>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<SignUpHandler> _logger;

        private static readonly Regex EmailRegex = new Regex(
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public SignUpHandler(IUserRepository userRepository, IPasswordService passwordService,
            IMessageBroker messageBroker, ILogger<SignUpHandler> logger)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _messageBroker = messageBroker;
            _logger = logger;
        }

        public async Task HandleAsync(SignUp command)
        {
            const decimal bonusFunds = 10000;
            if (!EmailRegex.IsMatch(command.Email))
            {
                _logger.LogError($"Invalid email: {command.Email}");
                throw new InvalidEmailException(command.Email);
            }

            var user = await _userRepository.GetByEmailAsync(command.Email);
            if (user is {})
            {
                _logger.LogError($"Email already in use: {command.Email}");
                throw new EmailInUseException(command.Email);
            }

            user = await _userRepository.GetByNameAsync(command.Name);
            if (user is {})
            {
                _logger.LogError($"Name already in use: {command.Name}");
                throw new NameInUseException(command.Name);
            }

            var role = string.IsNullOrWhiteSpace(command.Role) ? "user" : command.Role.ToLowerInvariant();
            var password = _passwordService.Hash(command.Password);
            user = new User(command.UserId, command.Email, command.Name, password, role, DateTime.UtcNow,
                command.Permissions, bonusFunds);
            await _userRepository.AddAsync(user);
            _logger.LogInformation($"Created an account for the user with ID: '{user.Id}'.");
            await _messageBroker.PublishAsync(new UserCreated(user.Id, user.Name, user.Role));
        }
    }
}