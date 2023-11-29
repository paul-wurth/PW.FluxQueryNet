namespace Flux.Net
{
    public partial class FluxQueryBuilder
    {
        /// <summary>
        /// Returns the first record with a non-<see langword="null"/> value from each input table.
        /// </summary>
        /// <param name="column">Column to operate on. Default is <c>_value</c>.</param>
        /// <remarks>This drops empty tables.</remarks>
        public FluxQueryBuilder First(string? column = null) => Aggregate("first", column);

        /// <summary>
        /// <para>Returns the last record with a non-<see langword="null"/> value from each input table.</para>
        /// <para>If the value is <see langword="null"/> in the last record, it returns the previous record with a non-<see langword="null"/> value.</para>
        /// </summary>
        /// <param name="column">Column to operate on. Default is <c>_value</c>.</param>
        /// <remarks>This drops empty tables.</remarks>
        public FluxQueryBuilder Last(string? column = null) => Aggregate("last", column);

        /// <summary>
        /// <para>Replaces all <see langword="null"/> values in input tables with a non-<see langword="null"/> value.</para>
        /// <para>Output tables are the same as the input tables with all <see langword="null"/> values replaced in the specified <paramref name="column"/>.</para>
        /// </summary>
        /// <param name="column">Column to replace <see langword="null"/> values in. Default is <c>_value</c>.</param>
        /// <param name="value">Constant value to replace <see langword="null"/> values with. Value type must match the type of the specified <paramref name="column"/>.</param>
        public FluxQueryBuilder Fill(object value, string? column = null)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> fill(value: ").Append(value.ToFlux());

            if (!string.IsNullOrWhiteSpace(column))
                _stringBuilder.Append(", column: \"").Append(column).Append("\"");

            _stringBuilder.Append(")");
            return this;
        }

        /// <summary>
        /// <para>Replaces all <see langword="null"/> values in input tables with the previous non-<see langword="null"/> value.</para>
        /// <para>Output tables are the same as the input tables with all <see langword="null"/> values replaced in the specified <paramref name="column"/>.</para>
        /// </summary>
        /// <param name="column">Column to replace <see langword="null"/> values in. Default is <c>_value</c>.</param>
        public FluxQueryBuilder FillPrevious(string? column = null)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> fill(usePrevious: true");

            if (!string.IsNullOrWhiteSpace(column))
                _stringBuilder.Append(", column: \"").Append(column).Append("\"");

            _stringBuilder.Append(")");
            return this;
        }

        /// <summary>
        /// <para>Returns all records containing unique values in a specified <paramref name="column"/>.</para>
        /// <para>Group keys, columns, and values are not modified.</para>
        /// </summary>
        /// <param name="column">Column to search for unique values. Default is <c>_value</c>.</param>
        /// <remarks>This drops empty tables.</remarks>
        public FluxQueryBuilder Unique(string? column = null) => Aggregate("unique", column);

        /// <summary>
        /// <para>Returns all unique values in a specified <paramref name="column"/>.</para>
        /// <para>
        /// The <c>_value</c> of each output record is set to a distinct value in the specified <paramref name="column"/>.<br/>
        /// <see langword="null"/> is considered its own distinct value if present.
        /// </para>
        /// </summary>
        /// <param name="column">Column to return unique values from. Default is <c>_value</c>.</param>
        public FluxQueryBuilder Distinct(string? column = null) => Aggregate("distinct", column);

        /// <summary>
        /// Regroups input data by modifying group key of input tables.
        /// </summary>
        /// <param name="columns">List of columns to use in the grouping operation. When no column is defined, it ungroups all data merges it into a single output table.</param>
        /// <remarks>Group does not guarantee sort order. To ensure data is sorted correctly, use <see cref="Sort"/> after.</remarks>
        public FluxQueryBuilder Group(params string[] columns)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> group(").AppendStringArrayParameter("columns", columns).Append(")");
            return this;
        }

        /// <summary>
        /// Regroups input data by modifying group key of input tables with all columns <b>except</b> those defined in the <paramref name="columns"/> parameter.
        /// </summary>
        /// <param name="columns">List of columns to exclude from the grouping operation.</param>
        /// <remarks>Group does not guarantee sort order. To ensure data is sorted correctly, use <see cref="Sort"/> after.</remarks>
        public FluxQueryBuilder GroupExcept(params string[] columns)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> group(mode: \"except\"").AppendStringArrayParameter("columns", columns, true).Append(")");
            return this;
        }

        /// <summary>
        /// <para>Returns a stream of tables containing only the specified <paramref name="columns"/>.</para>
        /// <para>Columns in the group key that are not specified in the <paramref name="columns"/> parameter are removed from the group key and dropped from output tables.</para>
        /// </summary>
        /// <param name="columns">Columns to keep in output tables.</param>
        /// <remarks>This is the inverse of <see cref="Drop"/>.</remarks>
        public FluxQueryBuilder Keep(params string[] columns)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> keep(").AppendStringArrayParameter("columns", columns).Append(")");
            return this;
        }

        /// <summary>
        /// <para>Removes specified <paramref name="columns"/> from a table.</para>
        /// <para>Columns in the group key that are specified in the <paramref name="columns"/> parameter are removed from the group key and dropped from output tables.</para>
        /// <para>If a specified column is not present in a table, the function returns an error.</para>
        /// </summary>
        /// <param name="columns">Columns to remove from input tables.</param>
        /// <remarks>This is the inverse of <see cref="Keep"/>.</remarks>
        public FluxQueryBuilder Drop(params string[] columns)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> drop(").AppendStringArrayParameter("columns", columns).Append(")");
            return this;
        }

        /// <summary>
        /// Orders rows in each input table based on values in specified <paramref name="columns"/>.
        /// </summary>
        /// <param name="desc">Sort results in descending order.</param>
        /// <param name="columns">Columns to sort by. Sort precedence is determined by list order. Default is <c>["_value"]</c>.</param>
        /// <returns></returns>
        public FluxQueryBuilder Sort(bool desc, params string[] columns)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> sort(desc: ").Append(desc.ToFlux()).AppendStringArrayParameter("columns", columns, true).Append(")");
            return this;
        }

        /// <summary>
        /// Sorts each input table by specified <paramref name="columns"/> and keeps the top <paramref name="n"/> records in each table.
        /// </summary>
        /// <param name="n">Number of rows to return from each input table.</param>
        /// <param name="columns">Columns to sort by. Sort precedence is determined by list order. Default is <c>["_value"]</c>.</param>
        /// <remarks>This drops empty tables.</remarks>
        public FluxQueryBuilder Top(int n, params string[] columns)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> top(n: ").Append(n.ToFlux()).AppendStringArrayParameter("columns", columns, true).Append(")");
            return this;
        }

        /// <summary>
        /// Sorts each input table by specified <paramref name="columns"/> and keeps the bottom <paramref name="n"/> records in each table.
        /// </summary>
        /// <param name="n">Number of rows to return from each input table.</param>
        /// <param name="columns">Columns to sort by. Sort precedence is determined by list order. Default is <c>["_value"]</c>.</param>
        /// <remarks>This drops empty tables.</remarks>
        public FluxQueryBuilder Bottom(int n, params string[] columns)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> bottom(n: ").Append(n.ToFlux()).AppendStringArrayParameter("columns", columns, true).Append(")");
            return this;
        }

        /// <summary>
        /// Returns the first <paramref name="n"/> rows after the specified <paramref name="offset"/> from each input table.
        /// </summary>
        /// <param name="n">Maximum number of rows to return.</param>
        /// <param name="offset">Number of rows to skip per table before limiting to <paramref name="n"/>. Default is <c>0</c>.</param>
        /// <remarks>If an input table has less than <c>offset + n</c> rows, it returns all rows after the <paramref name="offset"/>.</remarks>
        public FluxQueryBuilder Limit(int n, int? offset = null)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> limit(n: ").Append(n.ToFlux());

            if (offset.HasValue)
                _stringBuilder.Append(", offset: ").Append(offset.Value.ToFlux());

            _stringBuilder.Append(")");
            return this;
        }

        /// <summary>
        /// Collects unique values stored vertically (column-wise) and aligns them horizontally (row-wise) into logical sets.
        /// </summary>
        /// <param name="columnKey">Columns to use to uniquely identify an output row.</param>
        /// <param name="rowKey">Columns to use to identify new output columns.</param>
        /// <param name="valueColumn">Column to use to populate the value of pivoted <paramref name="columnKey"/> columns.</param>
        public FluxQueryBuilder Pivot(string[] rowKey, string[] columnKey, string valueColumn = "_value")
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> pivot(valueColumn: \"").Append(valueColumn).Append("\"")
                .AppendStringArrayParameter("rowKey", rowKey, true)
                .AppendStringArrayParameter("columnKey", columnKey, true)
                .Append(")");
            return this;
        }
    }
}
