using System;
using Trill.Modules.Stories.Core.Entities;
using Trill.Modules.Stories.Core.Exceptions;

namespace Trill.Modules.Stories.Core.ValueObjects
{
    internal class Author : IEquatable<Author>
    {
        public Guid Id { get; }
        public string Name { get; }

        public Author(Guid id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new MissingAuthorNameException();
            }

            Id = id;
            Name = name;
        }

        public bool Equals(Author other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) || Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Author) obj);
        }

        public override int GetHashCode() => Id.GetHashCode();

        public static Author Create(User user) => Create(user.Id, user.Name);

        public static Author Create(Guid id, string name) => new Author(id, name);
    }
}