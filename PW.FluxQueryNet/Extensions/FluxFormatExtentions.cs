using NodaTime;
using NodaTime.Text;
using System;
using System.Globalization;
using System.Text;

namespace PW.FluxQueryNet.Extensions
{
    public static class FluxFormatExtentions
    {
        private const string NullValueMessage = "Cannot represent a null value in Flux.";

        public static string ToFlux(this object value) => value switch
        {
            null => throw new ArgumentNullException(nameof(value), NullValueMessage),
            DateTime d => d.ToFlux(),
            DateTimeOffset d => d.ToFlux(),
#if NET6_0_OR_GREATER
            DateOnly d => d.ToFlux(),
#endif
            Instant d => d.ToFlux(),
            OffsetDateTime d => d.ToFlux(),
            ZonedDateTime d => d.ToFlux(),
            LocalDateTime d => d.ToFlux(),
            TimeSpan t => t.ToFlux(),
            Duration d => d.ToFlux(),
            bool b => b.ToFlux(),
            byte b => b.ToFlux(),
            ushort u => u.ToFlux(),
            uint u => u.ToFlux(),
            ulong u => u.ToFlux(),
            float f => f.ToFlux(),
            double d => d.ToFlux(),
            decimal d => d.ToFlux(),
            string s => s.ToFlux(),
            _ => Convert.ToString(value, CultureInfo.InvariantCulture)!
        };

        public static string ToFlux(this bool value) => value.ToString().ToLowerInvariant();

        public static string ToFlux(this string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), NullValueMessage);

            return $"\"{value}\"";
        }

        #region Floating-point number

        // See https://docs.influxdata.com/flux/latest/data-types/basic/float/

        public static string ToFlux(this float value) => WithFloatConversion(value switch
        {
            float.PositiveInfinity => "+Inf",
            float.NegativeInfinity => "-Inf",
            _ => value.ToString(CultureInfo.InvariantCulture)
        });

        public static string ToFlux(this double value) => WithFloatConversion(value switch
        {
            double.PositiveInfinity => "+Inf",
            double.NegativeInfinity => "-Inf",
            _ => value.ToString(CultureInfo.InvariantCulture)
        });

        public static string ToFlux(this decimal value) => WithFloatConversion(value.ToString(CultureInfo.InvariantCulture));

        /// <summary>
        /// Ensures support for floating numbers without decimal point (eg. <c>123</c>),
        /// scientific notation (eg. <c>1.23e+4</c>), <c>+Inf</c>, <c>-Inf</c> and <c>NaN</c>.
        /// </summary>
        private static string WithFloatConversion(string value) => $"float(v: \"{value}\")";

        #endregion

        #region Unsigned integer

        // See https://docs.influxdata.com/flux/latest/data-types/basic/uint/

        public static string ToFlux(this byte value) => WithUintConversion(value.ToString(CultureInfo.InvariantCulture));

        public static string ToFlux(this ushort value) => WithUintConversion(value.ToString(CultureInfo.InvariantCulture));

        public static string ToFlux(this uint value) => WithUintConversion(value.ToString(CultureInfo.InvariantCulture));

        public static string ToFlux(this ulong value) => WithUintConversion(value.ToString(CultureInfo.InvariantCulture));

        /// <summary>
        /// Ensures support for unsigned integers.
        /// </summary>
        private static string WithUintConversion(string value) => $"uint(v: \"{value}\")";

        #endregion

        #region Date and time

        // See https://docs.influxdata.com/flux/latest/data-types/basic/time/

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

#if NET6_0_OR_GREATER
        public static string ToFlux(this DateOnly date)
        {
            // Format: "yyyy-MM-dd"
            return date.ToString("o");
        }
#endif

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

        #region Duration

        // See https://docs.influxdata.com/flux/latest/data-types/basic/duration/

        public static string ToFlux(this TimeSpan timeSpan) => timeSpan == TimeSpan.Zero ? "0ns" :
            CreateFluxDuration(
                timeSpan < TimeSpan.Zero,
                timeSpan.Days,
                timeSpan.Hours,
                timeSpan.Minutes,
                timeSpan.Seconds,
                timeSpan.Milliseconds,
                timeSpan.Microseconds(),
                timeSpan.Nanoseconds()
            );

        public static string ToFlux(this Duration duration) => duration == Duration.Zero ? "0ns" :
            CreateFluxDuration(
                duration < Duration.Zero,
                duration.Days,
                duration.Hours,
                duration.Minutes,
                duration.Seconds,
                duration.Milliseconds,
                duration.Microseconds(),
                duration.Nanoseconds()
            );

        private static string CreateFluxDuration(bool isNegative, int days, int hours, int minutes, int seconds, int milliseconds, int microseconds, int nanoseconds)
        {
            var stringBuilder = new StringBuilder();

            if (isNegative)
                stringBuilder.Append('-');
            if (days != 0)
                stringBuilder.Append(Math.Abs(days)).Append('d');
            if (hours != 0)
                stringBuilder.Append(Math.Abs(hours)).Append('h');
            if (minutes != 0)
                stringBuilder.Append(Math.Abs(minutes)).Append('m');
            if (seconds != 0)
                stringBuilder.Append(Math.Abs(seconds)).Append('s');
            if (milliseconds != 0)
                stringBuilder.Append(Math.Abs(milliseconds)).Append("ms");
            if (microseconds != 0)
                stringBuilder.Append(Math.Abs(microseconds)).Append("us");
            if (nanoseconds != 0)
                stringBuilder.Append(Math.Abs(nanoseconds)).Append("ns");

            return stringBuilder.ToString();
        }

        private const long TicksPerMicrosecond = 10;
        private const long MicrosecondsPerMillisecond = 1000;

        public static int Microseconds(this TimeSpan timeSpan) => (int)(timeSpan.Ticks / TicksPerMicrosecond % MicrosecondsPerMillisecond);
        public static int Nanoseconds(this TimeSpan timeSpan) => (int)(timeSpan.Ticks % TicksPerMicrosecond * NodaConstants.NanosecondsPerTick);

        public static int Microseconds(this Duration duration) => (int)(duration.NanosecondOfDay / NodaConstants.NanosecondsPerMicrosecond % MicrosecondsPerMillisecond);
        public static int Nanoseconds(this Duration duration) => (int)(duration.NanosecondOfDay % NodaConstants.NanosecondsPerMicrosecond);

        #endregion
    }
}
