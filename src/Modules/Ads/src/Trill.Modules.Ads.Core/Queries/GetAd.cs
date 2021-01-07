using System;
using Trill.Modules.Ads.Core.DTO;
using Trill.Shared.Abstractions.Queries;

namespace Trill.Modules.Ads.Core.Queries
{
    internal class GetAd : IQuery<AdDetailsDto>
    {
        public Guid AdId { get; set; }
    }
}