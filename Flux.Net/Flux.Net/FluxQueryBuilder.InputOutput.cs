using System.Text;

namespace Flux.Net
{
    public partial class FluxQueryBuilder
    {
        /// <summary>
        /// Queries data from an InfluxDB data source.
        /// </summary>
        /// <param name="bucket">Name of the bucket to query.</param>
        /// <param name="retentionPolicy">Name of the retention policy.</param>
        public static FluxQueryBuilder From(string bucket, string? retentionPolicy = null)
        {
            var stringBuilder = new StringBuilder("from(bucket: \"").Append(bucket);

            if (!string.IsNullOrWhiteSpace(retentionPolicy))
                stringBuilder.Append('/').Append(retentionPolicy);

            stringBuilder.Append("\")");

            return new FluxQueryBuilder(stringBuilder);
        }

        /// <summary>
        /// <para>Delivers input data as a result of the query.</para>
        /// <para>A query may have multiple yields, each identified by unique name specified in the name parameter.</para>
        /// </summary>
        /// <remarks>
        /// It is implicit for queries that output a single stream of tables and is only necessary when yielding multiple results from a query.
        /// </remarks>
        /// <param name="name">Unique name for the yielded results. Default is <c>_results</c>.</param>
        public FluxQueryBuilder Yield(string? name = null)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> ").Append("yield").Append('(');

            if (!string.IsNullOrWhiteSpace(name))
                _stringBuilder.Append("name: \"").Append(name).Append('"');

            _stringBuilder.Append(')');
            return this;
        }
    }
}
