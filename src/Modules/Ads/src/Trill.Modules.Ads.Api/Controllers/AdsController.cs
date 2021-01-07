using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Trill.Modules.Ads.Core.Commands;
using Trill.Modules.Ads.Core.DTO;
using Trill.Modules.Ads.Core.Queries;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Queries;

namespace Trill.Modules.Ads.Api.Controllers
{
    [ApiController]
    [Route("ads-module/[controller]")]
    internal class AdsController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public AdsController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<ActionResult<Paged<AdDto>>> Get([FromQuery] BrowseAds query)
        {
            var result = await _dispatcher.QueryAsync(query);
            return Ok(result);
        }

        [HttpGet("{adId}")]
        public async Task<ActionResult<AdDetailsDto>> Get([FromRoute] GetAd query)
        {
            var result = await _dispatcher.QueryAsync(query);
            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Post(CreateAd command)
        {
            await _dispatcher.SendAsync(command);
            return Created($"ads-module/ads/{command.AdId}", null);
        }

        [HttpPut("{adId}/approve")]
        public async Task<ActionResult> Approve(ApproveAd command)
        {
            await _dispatcher.SendAsync(command);
            return NoContent();
        }

        [HttpPut("{adId}/reject")]
        public async Task<ActionResult> Reject(RejectAd command)
        {
            await _dispatcher.SendAsync(command);
            return NoContent();
        }
        
        [HttpPut("{adId}/pay")]
        public async Task<ActionResult> Pay(PayAd command)
        {
            await _dispatcher.SendAsync(command);
            return NoContent();
        }
        
        [HttpPut("{adId}/publish")]
        public async Task<ActionResult> Publish(PublishAd command)
        {
            await _dispatcher.SendAsync(command);
            return NoContent();
        }
    }
}