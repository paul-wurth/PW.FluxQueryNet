using NodaTime;
using NodaTime.Text;
using System;
using System.Globalization;

namespace Flux.Net
{
    public static class QueryExtentions
    {
        public static string ToFlux(this DateTime date)
        {
            // Format: "yyyy-MM-ddTHH:mm:ss.fffffffZ"
            return date.ToUniversalTime().ToString("o");
        }
        public static string ToFlux(this Instant date)
        {
            // No UTC conversion required: Instant has no concept of a particular time zone.
            // Format: "yyyy-MM-ddTHH:mm:ss.FFFFFFFFFZ"
            return InstantPattern.ExtendedIso.Format(date);
        }
        public static string ToFlux(this OffsetDateTime date) => date.ToInstant().ToFlux();
        public static string ToFlux(this ZonedDateTime date) => date.ToInstant().ToFlux();
        public static string ToFlux(this LocalDateTime date) => date.WithOffset(Offset.Zero).ToFlux();

        public static string ToFlux(this bool value) => value.ToString().ToLowerInvariant();
        public static string ToFlux(this object value) => Convert.ToString(value, CultureInfo.InvariantCulture);
    }
}
