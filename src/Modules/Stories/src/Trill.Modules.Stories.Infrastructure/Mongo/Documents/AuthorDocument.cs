using System;
using Trill.Modules.Stories.Core.ValueObjects;

namespace Trill.Modules.Stories.Infrastructure.Mongo.Documents
{
    internal class AuthorDocument
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public AuthorDocument()
        {
        }
        
        public AuthorDocument(Author author)
        {
            Id = author.Id;
            Name = author.Name;
        }

        public Author ToValueObject() => new Author(Id, Name);
    }
}