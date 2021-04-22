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
using Trill.Modules.Stories.Core.Factories;
using Trill.Modules.Stories.Core.Policies;
using Trill.Modules.Stories.Core.Repositories;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Generators;
using Trill.Shared.Abstractions.Kernel;
using Trill.Shared.Abstractions.Messaging;
using Trill.Shared.Abstractions.Time;
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
            var command = CreateCommand(0, user.Id);
            _storyAuthorPolicy.CanCreate(user).Returns(true);
            
            await Act(command);
            
            _storyTextFactory.Received(1).Create(command.Text);
            _idGenerator.Received(1).Generate();
            _clock.Received(1).Current();
            _storyRequestStorage.Received(1).SetStoryId(command.Id, Arg.Any<long>());
            await _storyRepository.Received(1).AddAsync(Arg.Is<Story>(x => x.Title == command.Title));
        }
        
        [Fact]
        public async Task story_id_should_be_generated_when_providing_the_default_value()
        {
            var storyId = 1;
            var user = SetupUser();
            var command = CreateCommand(0, user.Id);
            _storyAuthorPolicy.CanCreate(user).Returns(true);
            _idGenerator.Generate().Returns(storyId);
            
            await Act(command);

            _idGenerator.Received(1);
            await _storyRepository.Received(1).AddAsync(Arg.Is<Story>(x => x.Id == storyId));
        }

        [Fact]
        public async Task story_sent_event_should_be_published()
        {
            var user = SetupUser();
            var command = CreateCommand(1, user.Id);
            _storyAuthorPolicy.CanCreate(user).Returns(true);
            _eventMapper.Map(Arg.Any<IDomainEvent>()).Returns(new[]
            {
                new StorySent(1,
                    default, default, default, default, default)
            });
            
            await Act(command);
            
            await _messageBroker.Received(1).PublishAsync(Arg.Is<StorySent>(x => x.StoryId == command.StoryId));
        }

        [Fact]
        public async Task handler_should_fail_given_invalid_user()
        {
            var command = CreateCommand(1, Guid.NewGuid());
            
            var exception = await Record.ExceptionAsync(() => Act(command));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<UserNotFoundException>();
        }
        
        [Fact]
        public async Task handler_should_fail_given_locked_user()
        {
            var user = SetupUser(0, true);
            var command = CreateCommand(1, user.Id);
            
            var exception = await Record.ExceptionAsync(() => Act(command));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<CannotCreateStoryException>();
        }

        #region Arrange

        private readonly IUserRepository _userRepository;
        private readonly IStoryRepository _storyRepository;
        private readonly IStoryTextFactory _storyTextFactory;
        private readonly IStoryAuthorPolicy _storyAuthorPolicy;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly IClock _clock;
        private readonly IIdGenerator _idGenerator;
        private readonly IStoryRequestStorage _storyRequestStorage;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;
        private readonly ICommandHandler<SendStory> _handler;

        public SendStoryHandlerTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _storyRepository = Substitute.For<IStoryRepository>();
            _storyTextFactory = Substitute.For<IStoryTextFactory>();
            _storyAuthorPolicy = Substitute.For<IStoryAuthorPolicy>();
            _domainEventDispatcher = Substitute.For<IDomainEventDispatcher>();
            _clock = Substitute.For<IClock>();
            _idGenerator = Substitute.For<IIdGenerator>();
            _storyRequestStorage = Substitute.For<IStoryRequestStorage>();
            _eventMapper = Substitute.For<IEventMapper>();
            _messageBroker = Substitute.For<IMessageBroker>();
            _handler = new SendStoryHandler(_userRepository, _storyRepository, _storyTextFactory, _storyAuthorPolicy,
                _domainEventDispatcher, _clock, _idGenerator, _storyRequestStorage, _eventMapper, _messageBroker);
        }

        private User SetupUser(int rating = 0, bool locked = false)
        {
            var user = new User(Guid.NewGuid(), "test", DateTime.UtcNow, rating, locked);
            _userRepository.GetAsync(user.Id).Returns(user);
            return user;
        }

        private static SendStory CreateCommand(long storyId, Guid userId)
            => new(storyId, userId, "Test", "Lorem ipsum", new[] {"test1", "test2"});

        #endregion
    }
}