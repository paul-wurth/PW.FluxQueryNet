using InfluxDB.Client.Api.Domain;

namespace PW.FluxQueryNet.FluxTypes.Converters
{
    /// <summary>
    /// Extension methods to convert boolean to Flux notation and Flux AST representation.
    /// </summary>
    /// <seealso href="https://docs.influxdata.com/flux/latest/data-types/basic/bool/">Boolean - InfluxDB documentation</seealso>
    public static class FluxBooleanConverter
    {
        public static string ToFluxNotation(this bool value) => value.ToString().ToLowerInvariant();

        public static BooleanLiteral ToFluxAstNode(this bool value) => new(nameof(BooleanLiteral), value);
    }
}
