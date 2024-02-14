using NodaTime;
using System;

namespace PW.FluxQueryNet.FluxTypes.Converters
{
    /// <summary>
    /// Extension methods to convert duration to Flux notation.
    /// </summary>
    /// <seealso href="https://docs.influxdata.com/flux/latest/data-types/basic/duration/">Duration - InfluxDB documentation</seealso>
    public static class FluxDurationConverter
    {
        public static FluxDuration AsFluxDuration(this TimeSpan value) => value;
        public static FluxDuration AsFluxDuration(this Duration value) => value;
        public static FluxDuration AsFluxDuration(this Period value) => value;

        public static string ToFluxNotation(this TimeSpan value) => value.AsFluxDuration().ToFluxNotation();
        public static string ToFluxNotation(this Duration value) => value.AsFluxDuration().ToFluxNotation();
        public static string ToFluxNotation(this Period value) => value.AsFluxDuration().ToFluxNotation();
    }
}
