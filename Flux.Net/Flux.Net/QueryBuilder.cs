using NodaTime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Flux.Net
{
    public class QueryBuilder
    {
        public static FluxQuery From(string dataSource, string retentionPolicy = "autogen")
        {
            return new FluxQuery(dataSource, retentionPolicy);
        }
    }

    public class FluxQuery
    {
        private string limitRecords = string.Empty;
        private string sortRecords = string.Empty;
        private string window = string.Empty;
        private string group = string.Empty;

        private Aggregates Aggregate = new();
        private Functions Function = new();
        private readonly StringBuilder _stringBuilder = new();

        public FluxQuery(string dataSource, string retentionPolicy = "autogen")
        {
            if (string.IsNullOrEmpty(retentionPolicy))
            {
                _stringBuilder.Append($@"from(bucket:""{dataSource}"") ");
            }
            else
            {
                _stringBuilder.Append($@"from(bucket:""{dataSource}/{retentionPolicy}"") ");
            }
        }

        #region Time range

        public FluxQuery RelativeTimeRange(KeyValuePair<TimeUnit, double> start, KeyValuePair<TimeUnit, double>? end = null)
            => Range(start.ToFlux(), end?.ToFlux());

        public FluxQuery AbsoluteTimeRange(Instant start, Instant? end = null)
            => Range(start.ToInfluxDateTime(), end?.ToInfluxDateTime());

        public FluxQuery AbsoluteTimeRange(OffsetDateTime start, OffsetDateTime? end = null)
            => Range(start.ToInfluxDateTime(), end?.ToInfluxDateTime());

        public FluxQuery AbsoluteTimeRange(ZonedDateTime start, ZonedDateTime? end = null)
            => Range(start.ToInfluxDateTime(), end?.ToInfluxDateTime());

        public FluxQuery AbsoluteTimeRange(DateTime start, DateTime? end = null)
            => Range(start.ToInfluxDateTime(), end?.ToInfluxDateTime());

        private FluxQuery Range(string start, string? end)
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

        public FluxQuery Filter(Action<FluxFilter> filterAction)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> filter(fn: (r) => ");

            var filter = new FluxFilter(_stringBuilder);
            filterAction.Invoke(filter);

            _stringBuilder.Append(")");
            return this;
        }

        #endregion

        public FluxQuery Aggregates(Action<Aggregates> filterAction)
        {
            Aggregate = new Aggregates();
            filterAction.Invoke(Aggregate);
            return this;
        }

        public FluxQuery Window(string interval, Action<Aggregates>? filterAction = null)
        {
            window = $@"window(every: {interval})";
            if (filterAction != null)
            {
                Aggregate = new Aggregates();
                filterAction.Invoke(Aggregate);
            }
            return this;
        }

        public FluxQuery Functions(Action<Functions> filterAction)
        {
            Function = new Functions();
            filterAction.Invoke(Function);
            return this;
        }

        public FluxQuery Count()
        {
            limitRecords = "\n|> count() ";
            return this;
        }

        public FluxQuery Sort(bool desc, params string[] columns)
        {
            var orderString = Convert.ToString(desc, CultureInfo.InvariantCulture).ToLowerInvariant();
            sortRecords = $@"
|> sort(columns: [{string.Join(@" ,", columns.Select(s => { return $@"""{s}"""; }))} ], desc: {orderString}) ";
            return this;
        }

        public FluxQuery Limit(int limit, int offset = 0)
        {
            limitRecords = $@"
|> limit(n: {limit}, offset: {offset}) ";
            return this;
        }

        public FluxQuery Group()
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
