namespace Trill.Shared.Abstractions.Generators
{
    public interface IRng
    {
        string Generate(int length = 50, bool removeSpecialChars = false);
    }
}