using System.Collections.Generic;

namespace Trill.Modules.Users.Core.DTO
{
    internal class UserDetailsDto : UserDto
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public IEnumerable<string> Permissions { get; set; }
        public decimal Funds { get; set; }
    }
}