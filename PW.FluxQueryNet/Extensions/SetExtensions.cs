using System.Collections.Generic;

namespace PW.FluxQueryNet.Extensions
{
    internal static class SetExtensions
    {
        public static bool AddIfNotNull<T>(this ISet<T> set, T? value)
        {
            if (value == null)
                return false;

            return set.Add(value);
        }
    }
}
