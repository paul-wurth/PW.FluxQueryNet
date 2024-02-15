namespace PW.FluxQueryNet.FluxTypes
{
    public interface IFluxType
    {
        string ToFluxNotation();

        string? GetPackage();
    }
}
