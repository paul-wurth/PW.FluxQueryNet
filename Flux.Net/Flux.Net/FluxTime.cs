using Flux.Net.Extensions;
using NodaTime;
using System;

namespace Flux.Net
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
        private readonly object _value;
        private FluxTime(object value) => _value = value;

#if NET6_0_OR_GREATER
        public static implicit operator FluxTime(DateOnly value) => new(value);
#endif
        public static implicit operator FluxTime(DateTime value) => new(value);
        public static implicit operator FluxTime(DateTimeOffset value) => new(value);
        public static implicit operator FluxTime(Instant value) => new(value);
        public static implicit operator FluxTime(OffsetDateTime value) => new(value);
        public static implicit operator FluxTime(ZonedDateTime value) => new(value);
        public static implicit operator FluxTime(LocalDateTime value) => new(value);
        public static implicit operator FluxTime(TimeSpan value) => new(value);
        public static implicit operator FluxTime(Duration value) => new(value);

        public override string ToString() => _value.ToFlux();
    }
}
