using NodaTime;
using PW.FluxQueryNet.FluxTypes.Converters;
using System;

namespace PW.FluxQueryNet.FluxTypes
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
        private readonly string _value;

        private FluxDuration(string value) => _value = value;

        public override string ToString() => _value;


        public static implicit operator FluxDuration(TimeSpan value) => new(value.ToFluxNotation());
        public static implicit operator FluxDuration(Duration value) => new(value.ToFluxNotation());
    }
}
