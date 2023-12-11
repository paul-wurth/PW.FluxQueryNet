using NodaTime;
using PW.FluxQueryNet.Extensions;
using System;

namespace PW.FluxQueryNet
{
    /// <summary>
    /// Represents a length of time in Flux with nanosecond precision.
    /// </summary>
    /// <remarks>
    /// This can only be obtained by the implicit conversion of a <see cref="TimeSpan"/> or <see cref="Duration"/>.
    /// </remarks>
    /// <seealso href="https://docs.influxdata.com/flux/latest/data-types/basic/duration/">Duration - InfluxDB documentation</seealso>
    public sealed class FluxDuration
    {
        private readonly object _value;
        private FluxDuration(object value) => _value = value;

        public static implicit operator FluxDuration(TimeSpan value) => new(value);
        public static implicit operator FluxDuration(Duration value) => new(value);

        public override string ToString() => _value.ToFlux();
    }
}
