using PW.FluxQueryNet.Extensions;
using PW.FluxQueryNet.Parameterization;
using System;

namespace PW.FluxQueryNet
{
    public partial class FluxQueryBuilder
    {
        /// <inheritdoc/>
        public IFluxStream From(string bucket)
        {
            _stringBuilder.Append("from(bucket: \"").Append(bucket).Append("\")");
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream FromCustomFlux(FormattableString rawFlux)
        {
            _stringBuilder.Append(_parameters.Parameterize(rawFlux, "fromCustomFlux"));
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream FromCustomFluxUnsafe(Func<ParametersManager, string> rawFluxBuilder)
        {
            _stringBuilder.Append(rawFluxBuilder(_parameters));
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream Yield(string? name = null)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("yield(");

            if (!string.IsNullOrWhiteSpace(name))
                _stringBuilder.Append("name: \"").Append(name).Append('"');

            _stringBuilder.Append(')');
            return this;
        }
    }
}
