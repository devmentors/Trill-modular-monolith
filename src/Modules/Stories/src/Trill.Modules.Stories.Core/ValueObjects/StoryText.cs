using System;

namespace Trill.Modules.Stories.Core.ValueObjects
{
    internal class StoryText : IEquatable<StoryText>
    {
        public string Value { get; }

        public StoryText(string value)
        {
            Value = value?.Trim();
        }

        public static implicit operator string(StoryText storyText) => storyText.Value;

        public static implicit operator StoryText(string storyText) => new StoryText(storyText);

        public bool Equals(StoryText other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((StoryText) obj);
        }

        public override int GetHashCode() => Value != null ? Value.GetHashCode() : 0;
    }
}