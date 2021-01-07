using System;
using Trill.Shared.Abstractions.Commands;

namespace Trill.Modules.Users.Core.Commands
{
    internal class SignIn : ICommand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public SignIn(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}