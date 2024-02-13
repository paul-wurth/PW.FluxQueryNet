using NodaTime;
using PW.FluxQueryNet.FluxTypes.Converters;
using System;

namespace PW.FluxQueryNet.FluxTypes
{
    /// <summary>
    /// Represents a single point in time in Flux with nanosecond precision.
    /// </summary>
    /// <remarks>
    /// This can only be obtained by the implicit conversion of a <see cref="DateTime"/>, <see cref="DateTimeOffset"/>,
    /// <see cref="Instant"/>, <see cref="OffsetDateTime"/>, <see cref="ZonedDateTime"/>, <see cref="LocalDateTime"/>,
    /// or a duration relative to <c>now()</c>, such as <see cref="TimeSpan"/> or <see cref="Duration"/>.
    /// </remarks>
    /// <seealso href="https://docs.influxdata.com/flux/latest/data-types/basic/time/">Time - InfluxDB documentation</seealso>
    public sealed class FluxTime
    {
        private readonly string _value;
        private readonly string? _package;

        private FluxTime(string value, string? package = null)
        {
            _value = value;
            _package = package;
        }

        public override string ToString() => _value;
        internal string? GetPackage() => _package;


#if NET6_0_OR_GREATER
        public static implicit operator FluxTime(DateOnly value) => new(value.ToFluxNotation());
#endif
        public static implicit operator FluxTime(DateTime value) => new(value.ToFluxNotation());
        public static implicit operator FluxTime(DateTimeOffset value) => new(value.ToFluxNotation());
        public static implicit operator FluxTime(Instant value) => new(value.ToFluxNotation());
        public static implicit operator FluxTime(OffsetDateTime value) => new(value.ToFluxNotation());
        public static implicit operator FluxTime(ZonedDateTime value) => new(value.ToFluxNotation());
        public static implicit operator FluxTime(LocalDateTime value) => new(value.ToFluxNotation());
        public static implicit operator FluxTime(TimeSpan value) => new(value.ToFluxNotation());
        public static implicit operator FluxTime(Duration value) => new(value.ToFluxNotation());

        /// <summary>
        /// Returns the current system time at which the Flux script is executed.
        /// </summary>
        /// <remarks>It is cached at runtime, so all executions of this function in a Flux script return the same time value.</remarks>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/now/">now() function - InfluxDB documentation</seealso>
        public static readonly FluxTime Now = new("now()");

        /// <summary>
        /// Returns the <see cref="Now"/> timestamp truncated to the day unit.
        /// </summary>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/today/">today() function - InfluxDB documentation</seealso>
        public static readonly FluxTime Today = new("today()");

        /// <summary>
        /// Returns the current system time at which this function is executed.
        /// </summary>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/system/time/">system.time() function - InfluxDB documentation</seealso>
        public static readonly FluxTime SystemTime = new("system.time()", FluxPackages.System);

        /// <summary>
        /// Returns the earliest time InfluxDB can represent.
        /// </summary>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/internal/influxql/#constants">Constants - InfluxDB documentation</seealso>
        public static readonly FluxTime MinValue = new("influxql.minTime", FluxPackages.Internal_InfluxQL);

        /// <summary>
        /// Returns the latest time InfluxDB can represent.
        /// </summary>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/internal/influxql/#constants">Constants - InfluxDB documentation</seealso>
        public static readonly FluxTime MaxValue = new("influxql.maxTime", FluxPackages.Internal_InfluxQL);
    }
}
