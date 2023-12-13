using PW.FluxQueryNet.Extensions;
using System.Collections.Generic;
using System.Text;

namespace PW.FluxQueryNet
{
    public partial class FluxQueryBuilder : IFluxSource, IFluxStream
    {
        private readonly HashSet<string> _packages = new();
        private readonly StringBuilder _stringBuilder = new();

        private FluxQueryBuilder() { }

        /// <summary>
        /// Creates a <see cref="FluxQueryBuilder"/> to generate a Flux query.
        /// </summary>
        public static IFluxSource Create() => new FluxQueryBuilder();

        /// <inheritdoc/>
        public IFluxStream PipeCustomFlux(string rawFlux)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append(rawFlux);
            return this;
        }

        /// <inheritdoc/>
        public string ToQuery()
        {
            if (_packages.Count < 1)
                return _stringBuilder.ToString();

            var packagesBuilder = new StringBuilder();
            foreach (var package in _packages)
            {
                packagesBuilder.Append("import \"").Append(package).Append('"').AppendLine();
            }
            packagesBuilder.AppendLine();

            return packagesBuilder.ToString() + _stringBuilder.ToString();
        }
    }
}
