using PW.FluxQueryNet.Extensions;
using System;

namespace PW.FluxQueryNet
{
    public partial class FluxQueryBuilder
    {
        /// <inheritdoc/>
        public IFluxStream Range(FluxTime start, FluxTime? end = null)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> range(start: ").Append(start);
            _packages.AddIfNotNull(start.GetPackage());

            if (end != null)
            {
                _stringBuilder.Append(", stop: ").Append(end);
                _packages.AddIfNotNull(end.GetPackage());
            }

            _stringBuilder.Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream Filter(Action<FluxFilter> filterAction)
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
