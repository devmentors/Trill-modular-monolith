using System;
using Trill.Modules.Stories.Core.ValueObjects;

namespace Trill.Modules.Stories.Infrastructure.Mongo.Documents
{
    internal class VisibilityDocument
    {
        public DateTime From { get; }
        public DateTime To { get; }
        public bool Highlighted { get; }

        public VisibilityDocument()
        {
        }

        public VisibilityDocument(Visibility visibility)
        {
            From = visibility.From;
            To = visibility.To;
            Highlighted = visibility.Highlighted;
        }

        public Visibility ToValueObject() => new Visibility(From, To, Highlighted);
    }
}