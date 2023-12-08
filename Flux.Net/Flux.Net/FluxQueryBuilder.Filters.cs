using NodaTime;
using System;

namespace Flux.Net
{
    public partial class FluxQueryBuilder
    {
        /// <summary>
        /// Filters rows based on time bounds.
        /// </summary>
        /// <remarks>
        /// <para>Input data must have a <c>_time</c> column of type time. Rows with a <see langword="null"/> value in the <c>_time</c> are filtered.</para>
        /// <para>It adds a <c>_start</c> column with the value of start and a <c>_stop</c> column with the value of stop. These columns are added to the group key.</para>
        /// <para>Each input table's group key value is modified to fit within the time bounds. Tables with all rows outside the time bounds are filtered entirely.</para>
        /// </remarks>
        /// <param name="start">
        /// <para>Earliest time to include in results. Results include rows with <c>_time</c> values that match the specified start time.</para>
        /// <para>Use a duration relative to <c>now()</c> (eg. <see cref="TimeSpan"/> or <see cref="Duration"/>) or an absolute time
        /// (eg. <see cref="DateTime"/>, <see cref="DateTimeOffset"/>, <see cref="Instant"/>, <see cref="OffsetDateTime"/>, etc.).</para>
        /// </param>
        /// <param name="end">
        /// <para>Latest time to include in results. Results exclude rows with <c>_time</c> values that match the specified stop time. Default is <c>now()</c>.</para>
        /// <para>Use a duration relative to <c>now()</c> (eg. <see cref="TimeSpan"/> or <see cref="Duration"/>) or an absolute time
        /// (eg. <see cref="DateTime"/>, <see cref="DateTimeOffset"/>, <see cref="Instant"/>, <see cref="OffsetDateTime"/>, etc.).</para>
        /// </param>
        public FluxQueryBuilder Range(FluxTime start, FluxTime? end = null)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> range(start: ").Append(start);

            if (end != null)
                _stringBuilder.Append(", stop: ").Append(end);

            _stringBuilder.Append(')');
            return this;
        }

        /// <summary>
        /// Filters data based on conditions defined in a predicate function.
        /// </summary>
        /// <param name="filterAction">An action to specify the predicate function that evaluates <see langword="true"/> or <see langword="false"/>.</param>
        public FluxQueryBuilder Filter(Action<FluxFilter> filterAction)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> filter(fn: (r) => ");

            var filter = new FluxFilter(_stringBuilder);
            filterAction.Invoke(filter);

            _stringBuilder.Append(')');
            return this;
        }
    }
}
