using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Trill.Modules.Stories.Application.Commands;
using Trill.Modules.Stories.Application.Services;
using Trill.Shared.Abstractions.Dispatchers;

namespace Trill.Modules.Stories.Api.Controllers
{
    [ApiController]
    [Route("api/stories-module/[controller]")]
    internal class StoriesController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;
        private readonly IStoryRequestStorage _storyRequestStorage;

        public StoriesController(IDispatcher dispatcher, IStoryRequestStorage storyRequestStorage)
        {
            _dispatcher = dispatcher;
            _storyRequestStorage = storyRequestStorage;
        }

        [HttpPost]
        public async Task<ActionResult> Post(SendStory command)
        {
            await _dispatcher.SendAsync(command);
            var storyId = _storyRequestStorage.GetStoryId(command.Id);
            return Created($"api/stories-module/stories/{storyId}", null);
        }
    }
}