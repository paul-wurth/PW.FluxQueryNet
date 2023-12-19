using PW.FluxQueryNet.Extensions;

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
        public IFluxStream FromCustomFlux(string rawFlux)
        {
            _stringBuilder.Append(rawFlux);
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
