using Flux.Net.Extensions;
using NodaTime;
using System;

namespace Flux.Net
{
    public sealed class FluxDuration
    {
        private readonly object _value;
        private FluxDuration(object value) => _value = value;

        public static implicit operator FluxDuration(TimeSpan value) => new(value);
        public static implicit operator FluxDuration(Duration value) => new(value);

        public override string ToString() => _value.ToFlux();
    }
}
