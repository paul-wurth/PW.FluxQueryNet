using Flux.Net.Extensions;
using System.Collections.Generic;

namespace Flux.Net
{
    public enum TimeUnit
    {
        Seconds = 1,
        Minutes = 2,
        Hours = 3,
        Days = 4,
        Months = 5,
        Years = 6
    }

    public static class TimeUnitExtensions
    {
        public static string ToFlux(this TimeUnit unit)
        {
            return unit switch
            {
                TimeUnit.Seconds => "s",
                TimeUnit.Minutes => "m",
                TimeUnit.Hours => "h",
                TimeUnit.Days => "d",
                TimeUnit.Months => "mo",
                TimeUnit.Years => "y",
                _ => "d"
            };
        }

        public static string ToFlux(this KeyValuePair<TimeUnit, double> duration)
        {
            return duration.Value.ToFlux() + duration.Key.ToFlux();
        }
    }
}
