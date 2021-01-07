namespace Trill.Web.Core.Users.Requests
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}