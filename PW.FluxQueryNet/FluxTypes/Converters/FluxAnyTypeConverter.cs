using NodaTime;
using System;
using System.Globalization;

namespace PW.FluxQueryNet.FluxTypes.Converters
{
    /// <summary>
    /// Extension methods to convert any <see cref="object"/> to Flux notation.
    /// </summary>
    public static class FluxAnyTypeConverter
    {
        internal const string NullValueMessage = "Cannot represent a null value in Flux.";

        public static string ToFluxNotation(this object value) => value switch
        {
            null => throw new ArgumentNullException(nameof(value), NullValueMessage),

            IFluxType x => x.ToFluxNotation(),

            // Boolean
            bool b => b.ToFluxNotation(),

            // Integers
            sbyte i => i.ToFluxNotation(),
            short i => i.ToFluxNotation(),
            int i => i.ToFluxNotation(),
            long i => i.ToFluxNotation(),

            // Unsigned integers
            byte u => u.ToFluxNotation(),
            ushort u => u.ToFluxNotation(),
            uint u => u.ToFluxNotation(),
            ulong u => u.ToFluxNotation(),

            // Floating-point numbers
            float f => f.ToFluxNotation(),
            double f => f.ToFluxNotation(),
            decimal f => f.ToFluxNotation(),

            // Dates and times
            DateTime d => d.ToFluxNotation(),
            DateTimeOffset d => d.ToFluxNotation(),
#if NET6_0_OR_GREATER
            DateOnly d => d.ToFluxNotation(),
#endif
            Instant d => d.ToFluxNotation(),
            ZonedDateTime d => d.ToFluxNotation(),
            OffsetDateTime d => d.ToFluxNotation(),
            OffsetDate d => d.ToFluxNotation(),
            LocalDateTime d => d.ToFluxNotation(),
            LocalDate d => d.ToFluxNotation(),

            // Durations
            TimeSpan d => d.ToFluxNotation(),
            Duration d => d.ToFluxNotation(),
            Period d => d.ToFluxNotation(),

            // String
            string s => s.ToFluxNotation(),

            _ => Convert.ToString(value, CultureInfo.InvariantCulture)!
        };
    }
}
