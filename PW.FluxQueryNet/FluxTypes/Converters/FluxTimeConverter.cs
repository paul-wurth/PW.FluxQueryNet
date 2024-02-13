using NodaTime;
using NodaTime.Text;
using System;

namespace PW.FluxQueryNet.FluxTypes.Converters
{
    /// <summary>
    /// Extension methods to convert time to Flux notation.
    /// </summary>
    /// <seealso href="https://docs.influxdata.com/flux/latest/data-types/basic/time/">Time - InfluxDB documentation</seealso>
    public static class FluxTimeConverter
    {
        public static string ToFluxNotation(this DateTime date)
        {
            // Format: "yyyy-MM-ddTHH:mm:ss.fffffffZ"
            return date.ToUniversalTime().ToString("o");
        }

        public static string ToFluxNotation(this DateTimeOffset date)
        {
            // Format: "yyyy-MM-ddTHH:mm:ss.fffffffZ"
            return date.UtcDateTime.ToString("o");
        }

#if NET6_0_OR_GREATER
        public static string ToFluxNotation(this DateOnly date)
        {
            // Format: "yyyy-MM-dd"
            return date.ToString("o");
        }
#endif

        public static string ToFluxNotation(this Instant date)
        {
            // No UTC conversion required: Instant has no concept of a particular time zone.
            // Format: "yyyy-MM-ddTHH:mm:ss.FFFFFFFFFZ"
            return InstantPattern.ExtendedIso.Format(date);
        }

        public static string ToFluxNotation(this OffsetDateTime date) => date.ToInstant().ToFluxNotation();

        public static string ToFluxNotation(this ZonedDateTime date) => date.ToInstant().ToFluxNotation();

        public static string ToFluxNotation(this LocalDateTime date) => date.WithOffset(Offset.Zero).ToFluxNotation();
    }
}
