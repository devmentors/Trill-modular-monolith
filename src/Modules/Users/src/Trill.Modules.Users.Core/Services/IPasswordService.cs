namespace Trill.Modules.Users.Core.Services
{
    internal interface IPasswordService
    {
        bool IsValid(string hash, string password);
        string Hash(string password);
    }
}