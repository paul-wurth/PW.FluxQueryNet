using NodaTime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Flux.Net
{
    public class FluxQueryBuilder
    {
        private string limitRecords = string.Empty;
        private string sortRecords = string.Empty;
        private string window = string.Empty;
        private string group = string.Empty;

        private Aggregates Aggregate = new();
        private Functions Function = new();
        private readonly StringBuilder _stringBuilder;

        #region From

        public static FluxQueryBuilder From(string bucket, string? retentionPolicy = null) => new(bucket, retentionPolicy);

        private FluxQueryBuilder(string bucket, string? retentionPolicy)
        {
            _stringBuilder = new StringBuilder("from(bucket: \"").Append(bucket);

            if (!string.IsNullOrWhiteSpace(retentionPolicy))
                _stringBuilder.Append("/").Append(retentionPolicy);

            _stringBuilder.Append("\")");
        }

        #endregion

        #region Time range

        public FluxQueryBuilder RelativeTimeRange(KeyValuePair<TimeUnit, double> start, KeyValuePair<TimeUnit, double>? end = null)
            => Range(start.ToFlux(), end?.ToFlux());

        public FluxQueryBuilder AbsoluteTimeRange(Instant start, Instant? end = null)
            => Range(start.ToInfluxDateTime(), end?.ToInfluxDateTime());

        public FluxQueryBuilder AbsoluteTimeRange(OffsetDateTime start, OffsetDateTime? end = null)
            => Range(start.ToInfluxDateTime(), end?.ToInfluxDateTime());

        public FluxQueryBuilder AbsoluteTimeRange(ZonedDateTime start, ZonedDateTime? end = null)
            => Range(start.ToInfluxDateTime(), end?.ToInfluxDateTime());

        public FluxQueryBuilder AbsoluteTimeRange(DateTime start, DateTime? end = null)
            => Range(start.ToInfluxDateTime(), end?.ToInfluxDateTime());

        private FluxQueryBuilder Range(string start, string? end)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> range(start: ").Append(start);

            if (!string.IsNullOrEmpty(end))
                _stringBuilder.Append(", stop: ").Append(end);

            _stringBuilder.Append(")");
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

            _stringBuilder.Append(")");
            return this;
        }

        #endregion

        public FluxQueryBuilder Aggregates(Action<Aggregates> filterAction)
        {
            Aggregate = new Aggregates();
            filterAction.Invoke(Aggregate);
            return this;
        }

        public FluxQueryBuilder Window(string interval, Action<Aggregates>? filterAction = null)
        {
            window = $@"window(every: {interval})";
            if (filterAction != null)
            {
                Aggregate = new Aggregates();
                filterAction.Invoke(Aggregate);
            }
            return this;
        }

        public FluxQueryBuilder Functions(Action<Functions> filterAction)
        {
            Function = new Functions();
            filterAction.Invoke(Function);
            return this;
        }

        public FluxQueryBuilder Count()
        {
            limitRecords = "\n|> count() ";
            return this;
        }

        public FluxQueryBuilder Sort(bool desc, params string[] columns)
        {
            var orderString = Convert.ToString(desc, CultureInfo.InvariantCulture).ToLowerInvariant();
            sortRecords = $@"
|> sort(columns: [{string.Join(@" ,", columns.Select(s => { return $@"""{s}"""; }))} ], desc: {orderString}) ";
            return this;
        }

        public FluxQueryBuilder Limit(int limit, int offset = 0)
        {
            limitRecords = $@"
|> limit(n: {limit}, offset: {offset}) ";
            return this;
        }

        public FluxQueryBuilder Group()
        {
            group = "\n|> group() ";
            return this;
        }

        public string ToQuery()
        {
            var aggr = Aggregate?._Aggregates;
            var fun = Function?._Functions;

            if (!string.IsNullOrEmpty(group))
            {
                // Insert group to merge all tables
                _stringBuilder.Append(group);
                _stringBuilder.Append("\n");
            }

            if (!string.IsNullOrEmpty(fun))
            {
                _stringBuilder.Append(fun);
                _stringBuilder.Append("\n");
                //queryString = $@"{queryString} 
                //                 {fun} ";
            }

            if (!string.IsNullOrEmpty(window))
            {
                _stringBuilder.Append(window);
                _stringBuilder.Append("\n");
                //queryString = $@"{queryString} 
                //                 {window} ";
            }

            if (!string.IsNullOrEmpty(aggr))
            {
                _stringBuilder.Append(aggr);
                _stringBuilder.Append("\n");
                //queryString = $@"{queryString} 
                //                 {aggr} ";
            }

            if (!string.IsNullOrEmpty(sortRecords))
            {
                _stringBuilder.Append(sortRecords);
                _stringBuilder.Append("\n");
                //queryString = $@"{queryString} 
                //                 {sortRecords} ";
            }

            if (!string.IsNullOrEmpty(limitRecords))
            {
                _stringBuilder.Append(limitRecords);
                _stringBuilder.Append("\n");
                //queryString = $@"{queryString} 
                //                 {limitRecords} ";
            }

            return _stringBuilder.ToString();
        }
    }
}
