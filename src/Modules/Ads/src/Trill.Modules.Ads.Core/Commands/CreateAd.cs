using System;
using System.Collections.Generic;
using System.Linq;
using Trill.Shared.Abstractions.Commands;

namespace Trill.Modules.Ads.Core.Commands
{
    internal class CreateAd : ICommand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; }
        public Guid AdId { get; }
        public Guid UserId { get; }
        public string Header { get; }
        public string Content { get; }
        public IEnumerable<string> Tags { get; }
        public DateTime From { get; }
        public DateTime To { get; }

        public CreateAd(Guid adId, Guid userId, string header, string content, IEnumerable<string> tags,
            DateTime from, DateTime to)
        {
            AdId = adId == Guid.Empty ? Guid.NewGuid() : adId;
            UserId = userId;
            Header = header;
            Content = content;
            Tags = tags ?? Enumerable.Empty<string>();
            From = from;
            To = to;
        }
    }
}