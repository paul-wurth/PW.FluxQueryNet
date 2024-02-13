namespace PW.FluxQueryNet.FluxTypes.Converters
{
    /// <summary>
    /// Extension methods to convert boolean to Flux notation.
    /// </summary>
    /// <seealso href="https://docs.influxdata.com/flux/latest/data-types/basic/bool/">Boolean - InfluxDB documentation</seealso>
    public static class FluxBooleanConverter
    {
        public static string ToFluxNotation(this bool value) => value.ToString().ToLowerInvariant();
    }
}
