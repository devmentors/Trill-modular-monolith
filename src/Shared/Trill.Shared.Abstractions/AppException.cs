using System;

namespace Trill.Shared.Abstractions
{
    public abstract class AppException : Exception
    {
        protected AppException(string message) : base(message)
        {
        }
    }
}