using PW.FluxQueryNet.Extensions;
using PW.FluxQueryNet.FluxTypes.Converters;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace PW.FluxQueryNet.Options
{
    public class FluxBuilderOptions
    {
        public ParameterizedTypes ParameterizedTypes { get; }

        public DateTime? Now { get; private set; }

        private readonly HashSet<string> _packages = new();
        public ImmutableHashSet<string> Packages => _packages.ToImmutableHashSet();


        public FluxBuilderOptions(ParameterizedTypes parameterizedTypes = ParameterizedTypes.All)
        {
            ParameterizedTypes = parameterizedTypes;
        }

        public FluxBuilderOptions SetNow(DateTime now)
        {
            Now = now;
            return this;
        }

        public FluxBuilderOptions ImportPackage(string? package)
        {
            _packages.AddIfNotNull(package);
            return this;
        }


        internal string? GetNowAsFluxNotation()
        {
            if (Now == null)
                return null;

            return $"option now = () => {Now.Value.ToFluxNotation()}{Environment.NewLine}{Environment.NewLine}";
        }

        internal string? GetImportsAsFluxNotation()
        {
            if (_packages.Count < 1)
                return null;

            var stringBuilder = new StringBuilder();
            foreach (var package in _packages)
            {
                stringBuilder.Append("import \"").Append(package).Append('"').AppendLine();
            }
            stringBuilder.AppendLine();

            return stringBuilder.ToString();
        }
    }
}
