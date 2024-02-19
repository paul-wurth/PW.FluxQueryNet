using System;

namespace PW.FluxQueryNet.Options
{
    [Flags]
    public enum ParameterizedTypes : uint
    {
        None     = 0b00000000,  // 0
        String   = 0b00000001,  // 1
        Boolean  = 0b00000010,  // 2
        Integer  = 0b00000100,  // 4
        UInteger = 0b00001000,  // 8
        Float    = 0b00010000,  // 16
        DateTime = 0b00100000,  // 32
        Duration = 0b01000000,  // 64

        Numeric = Integer | UInteger | Float,
        All = String | Boolean | Numeric | DateTime | Duration
    }

    public static class ParameterizedTypesExtensions
    {
        /// <summary>
        /// Determines whether one or more bit fields are set in the current instance.
        /// </summary>
        /// <remarks>
        /// Similar to <see cref="Enum.HasFlag(Enum)"/> but avoids .NET Framework and .NET Core &lt; 2.1 performance issues.
        /// See <see href="https://github.com/dotnet/runtime/issues/6080">GitHub issue</see> and
        /// <see href="https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-core-2-1/#gist89028118">blog post</see>.
        /// </remarks>
        public static bool IsSet(this ParameterizedTypes self, ParameterizedTypes flag) => (self & flag) == flag;
    }
}
