using NodaTime;
using NodaTime.Text;
using System;

namespace Flux.Net
{
    public static class QueryExtentions
    {
        public static string ToInfluxDateTime(this DateTime date)
        {
            // Format: "yyyy-MM-ddTHH:mm:ss.fffffffZ"
            return date.ToUniversalTime().ToString("o");
        }

        public static string ToInfluxDateTime(this Instant date)
        {
            // No UTC conversion required: Instant has no concept of a particular time zone.
            // Format: "yyyy-MM-ddTHH:mm:ss.FFFFFFFFFZ"
            return InstantPattern.ExtendedIso.Format(date);
        }

        public static string ToInfluxDateTime(this OffsetDateTime date)
        {
            return date.ToInstant().ToInfluxDateTime();
        }

        public static string ToInfluxDateTime(this ZonedDateTime date)
        {
            return date.ToInstant().ToInfluxDateTime();
        }

        public static string ToInfluxDateTime(this LocalDateTime date)
        {
            return date.WithOffset(Offset.Zero).ToInfluxDateTime();
        }
    }
}
