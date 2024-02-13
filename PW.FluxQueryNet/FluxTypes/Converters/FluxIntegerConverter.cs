using System.Globalization;

namespace PW.FluxQueryNet.FluxTypes.Converters
{
    /// <summary>
    /// Extension methods to convert integer to Flux notation.
    /// </summary>
    /// <seealso href="https://docs.influxdata.com/flux/latest/data-types/basic/int/">Integer - InfluxDB documentation</seealso>
    public static class FluxIntegerConverter
    {
        public static string ToFluxNotation(this sbyte value) => value.ToString(CultureInfo.InvariantCulture);

        public static string ToFluxNotation(this short value) => value.ToString(CultureInfo.InvariantCulture);

        public static string ToFluxNotation(this int value) => value.ToString(CultureInfo.InvariantCulture);

        public static string ToFluxNotation(this long value) => value.ToString(CultureInfo.InvariantCulture);
    }
}
