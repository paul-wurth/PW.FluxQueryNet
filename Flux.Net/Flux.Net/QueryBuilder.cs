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
        private FluxFilter filter = new FluxFilter();
        private Aggregates Aggregate = new Aggregates();
        private Functions Function = new Functions();
        StringBuilder queryString = new StringBuilder();

        public FluxQuery(string dataSource, string retentionPolicy = "autogen")
        {
            if (string.IsNullOrEmpty(retentionPolicy))
            {
                queryString.Append($@"from(bucket:""{dataSource}"") ");
            }
            else
            {
                queryString.Append($@"from(bucket:""{dataSource}/{retentionPolicy}"") ");
            }
        }

        #region time range
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

        private FluxQuery Range(string start, string end)
        {
            queryString.AppendLine();
            queryString.Append("|> range(start: ").Append(start);

            if (!string.IsNullOrEmpty(end))
                queryString.Append(", stop: ").Append(end);

            queryString.Append(")");
            return this;
        }
        #endregion


        #region filter

        public FluxQuery Filter(Action<FluxFilter> filterAction)
        {
            filter = new FluxFilter();
            filterAction.Invoke(filter);

            return this;
        }

        #endregion

        public FluxQuery Aggregates(Action<Aggregates> filterAction)
        {
            Aggregate = new Aggregates();
            filterAction.Invoke(Aggregate);
            return this;
        }

        public FluxQuery Window(string interval, Action<Aggregates> filterAction = null)
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
            var select = filter?.SelectQuery;
            var filt = filter?.FilterQuery;
            var m = filter?.MeasurementName;
            var aggr = Aggregate?._Aggregates;
            var fun = Function?._Functions;
            string filterQuery = string.Empty;

            if (!string.IsNullOrEmpty(m))
            {
                filterQuery = $@"{filterQuery} r._measurement == ""{m}"" ";
            }

            if (!string.IsNullOrEmpty(select))
            {
                filterQuery = $@"{filterQuery} {select}";
            }

            if (!string.IsNullOrEmpty(filt))
            {
                filterQuery = $@"{filterQuery} {filt}";
            }
            if (!string.IsNullOrEmpty(filterQuery))
            {
                queryString.Append("\n");
                queryString.Append($@"|> filter(fn: (r) => {filterQuery})");
            }

            if (!string.IsNullOrEmpty(group))
            {
                // Insert group to merge all tables
                queryString.Append(group);
                queryString.Append("\n");
            }

            if (!string.IsNullOrEmpty(fun))
            {
                queryString.Append(fun);
                queryString.Append("\n");
                //queryString = $@"{queryString} 
                //                 {fun} ";
            }

            if (!string.IsNullOrEmpty(window))
            {
                queryString.Append(window);
                queryString.Append("\n");
                //queryString = $@"{queryString} 
                //                 {window} ";
            }

            if (!string.IsNullOrEmpty(aggr))
            {
                queryString.Append(aggr);
                queryString.Append("\n");
                //queryString = $@"{queryString} 
                //                 {aggr} ";
            }

            if (!string.IsNullOrEmpty(sortRecords))
            {
                queryString.Append(sortRecords);
                queryString.Append("\n");
                //queryString = $@"{queryString} 
                //                 {sortRecords} ";
            }

            if (!string.IsNullOrEmpty(limitRecords))
            {
                queryString.Append(limitRecords);
                queryString.Append("\n");
                //queryString = $@"{queryString} 
                //                 {limitRecords} ";
            }

            return queryString.ToString();
        }
    }
}
