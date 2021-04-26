using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Trill.Modules.Stories.Application.Commands;
using Trill.Shared.Abstractions.Dispatchers;

namespace Trill.Modules.Stories.Api.Controllers
{
    [ApiController]
    [Route("api/stories-module/stories/{storyId}")]
    internal class StoryRatingsController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public StoryRatingsController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpPost("rate")]
        public async Task<ActionResult> Post(RateStory command)
        {
            await _dispatcher.SendAsync(command);
            return NoContent();
        }
    }
}