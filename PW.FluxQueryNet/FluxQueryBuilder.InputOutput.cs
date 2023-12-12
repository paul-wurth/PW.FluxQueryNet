namespace PW.FluxQueryNet
{
    public partial class FluxQueryBuilder
    {
        /// <inheritdoc/>
        public IFluxStream From(string bucket, string? retentionPolicy = null)
        {
            _stringBuilder.Append("from(bucket: \"").Append(bucket);

            if (!string.IsNullOrWhiteSpace(retentionPolicy))
                _stringBuilder.Append('/').Append(retentionPolicy);

            _stringBuilder.Append("\")");
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream Yield(string? name = null)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> yield(");

            if (!string.IsNullOrWhiteSpace(name))
                _stringBuilder.Append("name: \"").Append(name).Append('"');

            _stringBuilder.Append(')');
            return this;
        }
    }
}
