using System;
using System.Threading.Tasks;
using NSubstitute;
using Shouldly;
using Trill.Modules.Stories.Application.Commands;
using Trill.Modules.Stories.Application.Commands.Handlers;
using Trill.Modules.Stories.Application.Events;
using Trill.Modules.Stories.Application.Exceptions;
using Trill.Modules.Stories.Application.Services;
using Trill.Modules.Stories.Core.Entities;
using Trill.Modules.Stories.Core.Policies;
using Trill.Modules.Stories.Core.Repositories;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Generators;
using Trill.Shared.Abstractions.Messaging;
using Xunit;

namespace Trill.Modules.Stories.Tests.Unit.Handlers
{
    public class SendStoryHandlerTests
    {
        private Task Act(SendStory command) => _handler.HandleAsync(command);

        [Fact]
        public async Task story_should_be_added_given_valid_data()
        {
            var user = SetupUser();
            var command = CreateCommand(user.Id);
            
            await Act(command);
            
            _storyTextPolicy.Received(1).Verify(command.Text);
            _idGenerator.Received(1).Generate();
            _dateTimeProvider.Received(1).Get();
            _storyRequestStorage.Received(1).SetStoryId(command.Id, Arg.Any<long>());
            await _storyRepository.Received(1).AddAsync(Arg.Is<Story>(x => x.Title == command.Title));
        }

        [Fact]
        public async Task story_sent_event_should_be_published()
        {
            var user = SetupUser();
            var command = CreateCommand(user.Id);
            
            await Act(command);
            
            await _messageBroker.Received(1).PublishAsync(Arg.Is<StorySent>(x => x.StoryId == command.StoryId));
        }

        [Fact]
        public async Task handler_should_fail_given_invalid_user()
        {
            var command = CreateCommand(Guid.NewGuid());
            
            var exception = await Record.ExceptionAsync(() => Act(command));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<UserNotFoundException>();
        }
        
        [Fact]
        public async Task handler_should_fail_given_locked_user()
        {
            var user = SetupUser(true);
            var command = CreateCommand(user.Id);
            
            var exception = await Record.ExceptionAsync(() => Act(command));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<UserLockedException>();
        }

        #region Arrange

        private readonly IUserRepository _userRepository;
        private readonly IStoryRepository _storyRepository;
        private readonly IStoryTextPolicy _storyTextPolicy;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IIdGenerator _idGenerator;
        private readonly IStoryRequestStorage _storyRequestStorage;
        private readonly IMessageBroker _messageBroker;
        private readonly ICommandHandler<SendStory> _handler;

        public SendStoryHandlerTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _storyRepository = Substitute.For<IStoryRepository>();
            _storyTextPolicy = Substitute.For<IStoryTextPolicy>();
            _dateTimeProvider = Substitute.For<IDateTimeProvider>();
            _idGenerator = Substitute.For<IIdGenerator>();
            _storyRequestStorage = Substitute.For<IStoryRequestStorage>();
            _messageBroker = Substitute.For<IMessageBroker>();
            _handler = new SendStoryHandler(_userRepository, _storyRepository, _storyTextPolicy, _dateTimeProvider,
                _idGenerator, _storyRequestStorage, _messageBroker);
        }

        private User SetupUser(bool locked = false)
        {
            var user = new User(Guid.NewGuid(), "test", DateTime.UtcNow, locked);
            _userRepository.GetAsync(user.Id).Returns(user);
            return user;
        }

        private static SendStory CreateCommand(Guid userId)
            => new SendStory(default, userId, "Test", "Lorem ipsum", new[] {"test1", "test2"});

        #endregion
    }
}