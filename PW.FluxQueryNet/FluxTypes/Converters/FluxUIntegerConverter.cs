using System.Globalization;

namespace PW.FluxQueryNet.FluxTypes.Converters
{
    /// <summary>
    /// Extension methods to convert unsigned integer to Flux notation.
    /// </summary>
    /// <seealso href="https://docs.influxdata.com/flux/latest/data-types/basic/uint/">Unsigned integer - InfluxDB documentation</seealso>
    public static class FluxUIntegerConverter
    {
        public static string ToFluxNotation(this byte value) => WithUIntConversion(value.ToString(CultureInfo.InvariantCulture));

        public static string ToFluxNotation(this ushort value) => WithUIntConversion(value.ToString(CultureInfo.InvariantCulture));

        public static string ToFluxNotation(this uint value) => WithUIntConversion(value.ToString(CultureInfo.InvariantCulture));

        public static string ToFluxNotation(this ulong value) => WithUIntConversion(value.ToString(CultureInfo.InvariantCulture));

        /// <summary>
        /// Ensures support for unsigned integers.
        /// </summary>
        private static string WithUIntConversion(string value) => $"uint(v: \"{value}\")";
    }
}
