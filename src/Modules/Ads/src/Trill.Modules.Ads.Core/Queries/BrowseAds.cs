using System;
using Trill.Modules.Ads.Core.DTO;
using Trill.Shared.Abstractions.Queries;

namespace Trill.Modules.Ads.Core.Queries
{
    internal class BrowseAds : PagedQueryBase, IQuery<Paged<AdDto>>
    {
        public Guid? UserId { get; set; }
    }
}