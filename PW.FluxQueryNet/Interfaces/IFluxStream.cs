namespace PW.FluxQueryNet
{
    public partial interface IFluxStream
    {
        /// <summary>
        /// Performs an operation specified in <paramref name="rawFlux"/> on the piped data stream.
        /// </summary>
        /// <param name="rawFlux">Raw Flux that operates on and returns a data stream.</param>
        IFluxStream PipeCustomFlux(string rawFlux);

        /// <summary>
        /// <para>Delivers input data as a result of the query.</para>
        /// <para>A query may have multiple yields, each identified by unique name specified in the name parameter.</para>
        /// </summary>
        /// <remarks>
        /// This function is implicit for queries that output a single stream of tables and is only necessary when yielding multiple results from a query.
        /// </remarks>
        /// <param name="name">Unique name for the yielded results. Default is <c>_results</c>.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/yield/">yield() function - InfluxDB documentation</seealso>
        IFluxStream Yield(string? name = null);

        /// <summary>
        /// Returns a string representation of the generated Flux query.
        /// </summary>
        string ToQuery();
    }
}
