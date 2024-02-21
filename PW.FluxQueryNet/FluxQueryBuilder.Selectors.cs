using PW.FluxQueryNet.Extensions;

namespace PW.FluxQueryNet
{
    public partial class FluxQueryBuilder
    {
        /// <inheritdoc/>
        public IFluxStream First(string? column = null) => Aggregate("first", column);

        /// <inheritdoc/>
        public IFluxStream Last(string? column = null) => Aggregate("last", column);

        /// <inheritdoc/>
        public IFluxStream Unique(string? column = null) => Aggregate("unique", column);

        /// <inheritdoc/>
        public IFluxStream Distinct(string? column = null) => Aggregate("distinct", column);

        /// <inheritdoc/>
        public IFluxStream Min(string? column = null) => Aggregate("min", column);

        /// <inheritdoc/>
        public IFluxStream Max(string? column = null) => Aggregate("max", column);

        /// <inheritdoc/>
        public IFluxStream Top(int n, params string[] columns)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("top(n: ").Append(_parameters.Parameterize("top_n", n))
                .AppendStringArrayParameter("columns", columns, true).Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream Bottom(int n, params string[] columns)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("bottom(n: ").Append(_parameters.Parameterize("bottom_n", n))
                .AppendStringArrayParameter("columns", columns, true).Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream Sort(bool desc, params string[] columns)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("sort(desc: ").Append(_parameters.Parameterize("sort_desc", desc))
                .AppendStringArrayParameter("columns", columns, true).Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream Limit(int n, int? offset = null)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("limit(n: ").Append(_parameters.Parameterize("limit_n", n));

            if (offset.HasValue)
                _stringBuilder.Append(", offset: ").Append(_parameters.Parameterize("limit_offset", offset.Value));

            _stringBuilder.Append(')');
            return this;
        }


        /// <inheritdoc/>
        public IFluxStream Fill(object value, string? column = null)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("fill(value: ").Append(_parameters.Parameterize("fill_value", value));

            if (!string.IsNullOrWhiteSpace(column))
                _stringBuilder.Append(", column: ").Append(_parameters.Parameterize("fill_column", column));

            _stringBuilder.Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream FillPrevious(string? column = null)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("fill(usePrevious: true");

            if (!string.IsNullOrWhiteSpace(column))
                _stringBuilder.Append(", column: ").Append(_parameters.Parameterize("fill_column", column));

            _stringBuilder.Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream Group(params string[] columns)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("group(").AppendStringArrayParameter("columns", columns).Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream GroupExcept(params string[] columns)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("group(mode: \"except\"").AppendStringArrayParameter("columns", columns, true).Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream Keep(params string[] columns)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("keep(").AppendStringArrayParameter("columns", columns).Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream Drop(params string[] columns)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("drop(").AppendStringArrayParameter("columns", columns).Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream Pivot(string[] rowKey, string[] columnKey, string valueColumn = "_value")
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("pivot(valueColumn: ").Append(_parameters.Parameterize("pivot_valueColumn", valueColumn))
                .AppendStringArrayParameter("rowKey", rowKey, true)
                .AppendStringArrayParameter("columnKey", columnKey, true)
                .Append(')');
            return this;
        }
    }
}
