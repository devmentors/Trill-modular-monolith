using System;
using System.Collections.Generic;
using Trill.Modules.Ads.Core.Domain.Exceptions;

namespace Trill.Modules.Ads.Core.Domain
{
    internal class Ad
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string Header { get; private set; }
        public string Content { get; private set; }
        public IEnumerable<string> Tags { get; private set; }
        public AdState State { get; private set; }
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ApprovedAt { get; private set; }
        public DateTime? RejectedAt { get; private set; }
        public DateTime? PaidAt { get; private set; }
        public DateTime? PublishedAt { get; private set; }
        public long? StoryId { get; private set; }

        private Ad()
        {
        }

        public Ad(Guid id, Guid userId, string header, string content, IEnumerable<string> tags, AdState state,
            DateTime from, DateTime to, DateTime createdAt)
        {
            if (string.IsNullOrWhiteSpace(header))
            {
                throw new InvalidHeaderException();
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new InvalidContentException();
            }

            if (from >= to)
            {
                throw new InvalidPeriodException(from, to);
            }

            Id = id;
            UserId = userId;
            Header = header;
            Content = content;
            Tags = tags;
            State = state;
            From = from;
            To = to;
            CreatedAt = createdAt;
            Amount = (int) Math.Floor((To - From).TotalDays) * 100;
        }

        public void Approve()
        {
            if (State != AdState.New)
            {
                throw new CannotChangeAdStateException(Id);
            }
            
            State = AdState.Approved;
            ApprovedAt = DateTime.UtcNow;
        }

        public void Reject()
        {
            if (State != AdState.New)
            {
                throw new CannotChangeAdStateException(Id);
            }
            
            State = AdState.Rejected;
            RejectedAt = DateTime.UtcNow;
        }

        public void Pay()
        {
            if (State != AdState.Approved)
            {
                throw new CannotChangeAdStateException(Id);
            }
            
            PaidAt = DateTime.UtcNow;
        }
        
        
        public void Publish()
        {
            if (State != AdState.Approved)
            {
                throw new CannotChangeAdStateException(Id);
            }

            if (PublishedAt.HasValue)
            {
                throw new CannotPublishAdAException(Id);
            }
            
            PublishedAt = DateTime.UtcNow;
        }

        public void SetStoryId(long storyId)
        {
            StoryId = storyId;
        }
    }
}