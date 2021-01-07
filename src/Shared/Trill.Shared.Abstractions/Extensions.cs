using System;
using System.Linq;

namespace Trill.Shared.Abstractions
{
    public static class Extensions
    {
        public static long ToUnixTimeMilliseconds(this DateTime dateTime)
            => new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();

        public static string Underscore(this string value)
            => string.IsNullOrWhiteSpace(value)
                ? string.Empty
                : string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString()))
                    .ToLowerInvariant();

        public static string GetModuleName(this object value)
            => value?.GetType().GetModuleName() ?? string.Empty;

        public static string GetModuleName(this Type type)
        {
            if (type?.Namespace is null)
            {
                return string.Empty;
            }

            return type.Namespace.StartsWith("Trill.Modules.")
                ? type.Namespace.Split(".")[2].ToLowerInvariant()
                : string.Empty;
        }
        
        public static string GetExceptionCode(this Exception exception)
            => exception.GetType().Name.Underscore().Replace("_exception", string.Empty);
    }
}