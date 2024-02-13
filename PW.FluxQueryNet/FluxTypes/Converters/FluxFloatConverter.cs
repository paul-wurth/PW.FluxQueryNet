using System.Globalization;

namespace PW.FluxQueryNet.FluxTypes.Converters
{
    /// <summary>
    /// Extension methods to convert float to Flux notation.
    /// </summary>
    /// <seealso href="https://docs.influxdata.com/flux/latest/data-types/basic/float/">Float - InfluxDB documentation</seealso>
    public static class FluxFloatConverter
    {
        public static string ToFluxNotation(this float value) => WithFloatConversion(value switch
        {
            float.PositiveInfinity => "+Inf",
            float.NegativeInfinity => "-Inf",
            _ => value.ToString(CultureInfo.InvariantCulture)
        });

        public static string ToFluxNotation(this double value) => WithFloatConversion(value switch
        {
            double.PositiveInfinity => "+Inf",
            double.NegativeInfinity => "-Inf",
            _ => value.ToString(CultureInfo.InvariantCulture)
        });

        public static string ToFluxNotation(this decimal value) => WithFloatConversion(value.ToString(CultureInfo.InvariantCulture));

        /// <summary>
        /// Ensures support for floating numbers without decimal point (eg. <c>123</c>),
        /// scientific notation (eg. <c>1.23e+4</c>), <c>+Inf</c>, <c>-Inf</c> and <c>NaN</c>.
        /// </summary>
        private static string WithFloatConversion(string value) => $"float(v: \"{value}\")";
    }
}
