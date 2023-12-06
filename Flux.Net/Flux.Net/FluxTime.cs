using Flux.Net.Extensions;
using NodaTime;
using System;

namespace Flux.Net
{
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
