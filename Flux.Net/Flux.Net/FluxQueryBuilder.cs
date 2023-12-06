using System;
using System.Text;

namespace Flux.Net
{
    public partial class FluxQueryBuilder
    {
        private readonly StringBuilder _stringBuilder;

        #region From

        public static FluxQueryBuilder From(string bucket, string? retentionPolicy = null) => new(bucket, retentionPolicy);

        private FluxQueryBuilder(string bucket, string? retentionPolicy)
        {
            _stringBuilder = new StringBuilder("from(bucket: \"").Append(bucket);

            if (!string.IsNullOrWhiteSpace(retentionPolicy))
                _stringBuilder.Append('/').Append(retentionPolicy);

            _stringBuilder.Append("\")");
        }

        #endregion

        #region Time range

        public FluxQueryBuilder Range(FluxTime start, FluxTime? end = null)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> range(start: ").Append(start);

            if (end != null)
                _stringBuilder.Append(", stop: ").Append(end);

            _stringBuilder.Append(')');
            return this;
        }

        #endregion

        #region Filter

        public FluxQueryBuilder Filter(Action<FluxFilter> filterAction)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> filter(fn: (r) => ");

            var filter = new FluxFilter(_stringBuilder);
            filterAction.Invoke(filter);

            _stringBuilder.Append(')');
            return this;
        }

        #endregion

        public string ToQuery()
        {
            return _stringBuilder.ToString();
        }
    }
}
