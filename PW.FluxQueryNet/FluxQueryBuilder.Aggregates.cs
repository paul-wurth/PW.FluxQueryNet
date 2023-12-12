﻿using PW.FluxQueryNet.Extensions;

namespace PW.FluxQueryNet
{
    public partial class FluxQueryBuilder
    {
        /// <inheritdoc/>
        public IFluxStream Aggregate(string methodName, string? column = null)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> ").Append(methodName).Append('(');

            if (!string.IsNullOrWhiteSpace(column))
                _stringBuilder.Append("column: \"").Append(column).Append('"');

            _stringBuilder.Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream Sum(string? column = null) => Aggregate("sum", column);

        /// <inheritdoc/>
        public IFluxStream Count(string? column = null) => Aggregate("count", column);

        /// <inheritdoc/>
        public IFluxStream Mean(string? column = null) => Aggregate("mean", column);

        /// <inheritdoc/>
        public IFluxStream MovingAverage(int n)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> movingAverage(n: ").Append(n.ToFlux()).Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream TimedMovingAverage(FluxDuration every, FluxDuration period, string? column = null)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> timedMovingAverage(every: ").Append(every).Append(", period: ").Append(period);

            if (!string.IsNullOrWhiteSpace(column))
                _stringBuilder.Append(", column: \"").Append(column).Append('"');

            _stringBuilder.Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream Mode(string? column = null) => Aggregate("mode", column);

        /// <inheritdoc/>
        public IFluxStream Spread(string? column = null) => Aggregate("spread", column);


        /// <inheritdoc/>
        public IFluxStream Window(FluxDuration every, FluxDuration? period = null, FluxDuration? offset = null,
            string? location = null, string? timeColumn = null, string? startColumn = null, string? stopColumn = null, bool createEmpty = false)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> window(every: ").Append(every);

            if (period != null)
                _stringBuilder.Append(", period: ").Append(period);

            if (offset != null)
                _stringBuilder.Append(", offset: ").Append(offset);

            if (!string.IsNullOrWhiteSpace(location))
                _stringBuilder.Append(", location: ").Append(location);

            if (!string.IsNullOrWhiteSpace(timeColumn))
                _stringBuilder.Append(", timeColumn: \"").Append(timeColumn).Append('"');

            if (!string.IsNullOrWhiteSpace(startColumn))
                _stringBuilder.Append(", startColumn: \"").Append(startColumn).Append('"');

            if (!string.IsNullOrWhiteSpace(stopColumn))
                _stringBuilder.Append(", stopColumn: \"").Append(stopColumn).Append('"');

            _stringBuilder.Append(", createEmpty: ").Append(createEmpty.ToFlux()).Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream AggregateWindow(string aggregateFunction, FluxDuration every, FluxDuration? period = null, FluxDuration? offset = null,
            string? location = null, string? column = null, string? timeSrcColumn = null, string? timeDstColumn = null, bool createEmpty = true)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> aggregateWindow(fn: ").Append(aggregateFunction).Append(", every: ").Append(every);

            if (period != null)
                _stringBuilder.Append(", period: ").Append(period);

            if (offset != null)
                _stringBuilder.Append(", offset: ").Append(offset);

            if (!string.IsNullOrWhiteSpace(location))
                _stringBuilder.Append(", location: ").Append(location);

            if (!string.IsNullOrWhiteSpace(column))
                _stringBuilder.Append(", column: \"").Append(column).Append('"');

            if (!string.IsNullOrWhiteSpace(timeSrcColumn))
                _stringBuilder.Append(", timeSrc: \"").Append(timeSrcColumn).Append('"');

            if (!string.IsNullOrWhiteSpace(timeDstColumn))
                _stringBuilder.Append(", timeDst: \"").Append(timeDstColumn).Append('"');

            _stringBuilder.Append(", createEmpty: ").Append(createEmpty.ToFlux()).Append(')');
            return this;
        }
    }
}
