using NodaTime;
using System;
using System.Text;

namespace PW.FluxQueryNet.FluxTypes.Converters
{
    /// <summary>
    /// Extension methods to convert duration to Flux notation.
    /// </summary>
    /// <seealso href="https://docs.influxdata.com/flux/latest/data-types/basic/duration/">Duration - InfluxDB documentation</seealso>
    public static class FluxDurationConverter
    {
        public static string ToFluxNotation(this TimeSpan timeSpan) => timeSpan == TimeSpan.Zero ? "0ns" :
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

        public static string ToFluxNotation(this Duration duration) => duration == Duration.Zero ? "0ns" :
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
    }
}
