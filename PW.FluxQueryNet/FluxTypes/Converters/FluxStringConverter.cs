using System;

namespace PW.FluxQueryNet.FluxTypes.Converters
{
    /// <summary>
    /// Extension methods to convert string to Flux notation.
    /// </summary>
    /// <seealso href="https://docs.influxdata.com/flux/latest/data-types/basic/string/">String - InfluxDB documentation</seealso>
    public static class FluxStringConverter
    {
        public static string ToFluxNotation(this string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), FluxAnyTypeConverter.NullValueMessage);

            return $"\"{value}\"";
        }
    }
}
