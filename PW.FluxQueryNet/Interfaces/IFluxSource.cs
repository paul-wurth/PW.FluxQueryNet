namespace PW.FluxQueryNet
{
    public interface IFluxSource
    {
        /// <summary>
        /// Queries data from an InfluxDB data source.
        /// </summary>
        /// <param name="bucket">Name of the bucket to query.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/from/">from() function - InfluxDB documentation</seealso>
        IFluxStream From(string bucket);

        /// <summary>
        /// Queries data from any supported data source specified in <paramref name="rawFlux"/>.
        /// </summary>
        /// <param name="rawFlux">Raw Flux that returns a data stream from a data source.</param>
        IFluxStream FromCustomFlux(string rawFlux);
    }
}
