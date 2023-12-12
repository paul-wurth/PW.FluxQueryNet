namespace PW.FluxQueryNet
{
    public interface IFluxSource
    {
        /// <summary>
        /// Queries data from an InfluxDB data source.
        /// </summary>
        /// <param name="bucket">Name of the bucket to query.</param>
        /// <param name="retentionPolicy">Name of the retention policy.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/from/">from() function - InfluxDB documentation</seealso>
        IFluxStream From(string bucket, string? retentionPolicy = null);
    }
}
