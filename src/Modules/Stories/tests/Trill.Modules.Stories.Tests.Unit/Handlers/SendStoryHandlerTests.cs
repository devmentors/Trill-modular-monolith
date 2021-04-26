using System;
using System.Threading.Tasks;
using NSubstitute;
using Trill.Modules.Stories.Application.Commands;
using Trill.Modules.Stories.Application.Commands.Handlers;
using Trill.Modules.Stories.Application.Services;
using Trill.Modules.Stories.Core.Entities;
using Trill.Modules.Stories.Core.Factories;
using Trill.Modules.Stories.Core.Repositories;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Generators;
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
            var command = CreateCommand(1, Guid.NewGuid());
        
            await Act(command);
        
            _storyTextFactory.Received(1).Create(command.Text);
            _clock.Received(1).Current();
            _idGenerator.DidNotReceiveWithAnyArgs();
            _storyRequestStorage.Received(1).SetStoryId(command.Id, Arg.Any<long>());
            await _storyRepository.Received(1).AddAsync(Arg.Is<Story>(x => x.Id == command.StoryId));
        }

        [Fact]
        public async Task story_id_should_be_generated_when_providing_the_default_value()
        {
            const int storyId = 1;
            var command = CreateCommand(0, Guid.NewGuid());
            _idGenerator.Generate().Returns(storyId);

            await Act(command);

            _idGenerator.Received(1).Generate();
            await _storyRepository.Received(1).AddAsync(Arg.Is<Story>(x => x.Id == storyId));
        }

        #region Arrange

        private readonly IStoryRepository _storyRepository;
        private readonly IStoryTextFactory _storyTextFactory;
        private readonly IClock _clock;
        private readonly IIdGenerator _idGenerator;
        private readonly IStoryRequestStorage _storyRequestStorage;
        private readonly ICommandHandler<SendStory> _handler;

        public SendStoryHandlerTests()
        {
            _storyRepository = Substitute.For<IStoryRepository>();
            _storyTextFactory = Substitute.For<IStoryTextFactory>();
            _clock = Substitute.For<IClock>();
            _idGenerator = Substitute.For<IIdGenerator>();
            _storyRequestStorage = Substitute.For<IStoryRequestStorage>();
            _handler = new SendStoryHandler(_storyRepository, _storyTextFactory, _clock, _idGenerator,
                _storyRequestStorage);
        }

        private static SendStory CreateCommand(long storyId, Guid userId)
            => new(storyId, userId, "Test", "Lorem ipsum", new[] {"test1", "test2"});

        #endregion
    }
}