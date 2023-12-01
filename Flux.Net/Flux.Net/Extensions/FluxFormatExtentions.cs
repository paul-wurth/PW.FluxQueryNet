using NodaTime;
using NodaTime.Text;
using System;
using System.Globalization;

namespace Flux.Net.Extensions
{
    public static class FluxFormatExtentions
    {
        public static string ToFlux(this object value) => value switch
        {
            DateTime d => d.ToFlux(),
            DateTimeOffset d => d.ToFlux(),
            Instant d => d.ToFlux(),
            OffsetDateTime d => d.ToFlux(),
            ZonedDateTime d => d.ToFlux(),
            LocalDateTime d => d.ToFlux(),
            bool b => b.ToFlux(),
            string s => s.ToFlux(),
            _ => Convert.ToString(value, CultureInfo.InvariantCulture)
        };

        public static string ToFlux(this bool value) => value.ToString().ToLowerInvariant();

        public static string ToFlux(this string value) => $"\"{value}\"";

        #region Date and time

        public static string ToFlux(this DateTime date)
        {
            // Format: "yyyy-MM-ddTHH:mm:ss.fffffffZ"
            return date.ToUniversalTime().ToString("o");
        }

        public static string ToFlux(this DateTimeOffset date)
        {
            // Format: "yyyy-MM-ddTHH:mm:ss.fffffffZ"
            return date.UtcDateTime.ToString("o");
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

        #endregion
    }
}
