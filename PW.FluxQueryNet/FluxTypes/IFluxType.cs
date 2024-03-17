using InfluxDB.Client.Api.Domain;

namespace PW.FluxQueryNet.FluxTypes
{
    public interface IFluxType
    {
        string ToFluxNotation();

        Expression ToFluxAstNode();

        bool CanConvertToFluxAstNode { get; }
    }
}
