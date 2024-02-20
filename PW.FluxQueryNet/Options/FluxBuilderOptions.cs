using InfluxDB.Client.Api.Domain;
using PW.FluxQueryNet.Extensions;
using PW.FluxQueryNet.FluxTypes.Converters;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace PW.FluxQueryNet.Options
{
    public class FluxBuilderOptions
    {
        public ParameterizedTypes ParameterizedTypes { get; }

        public DateTime? Now { get; private set; }

        private readonly HashSet<FluxPackageImport> _packageImports = new();
        public ImmutableHashSet<FluxPackageImport> PackageImports => _packageImports.ToImmutableHashSet();


        public FluxBuilderOptions(ParameterizedTypes parameterizedTypes = ParameterizedTypes.All)
        {
            ParameterizedTypes = parameterizedTypes;
        }

        public FluxBuilderOptions SetNow(DateTime now)
        {
            Now = now;
            return this;
        }

        public FluxBuilderOptions ImportPackage(FluxPackageImport? packageImport)
        {
            _packageImports.AddIfNotNull(packageImport);
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
            if (_packageImports.Count < 1)
                return null;

            var stringBuilder = new StringBuilder();
            foreach (var import in _packageImports)
            {
                stringBuilder.AppendLine(import.ToFluxNotation());
            }
            stringBuilder.AppendLine();

            return stringBuilder.ToString();
        }

        internal List<ImportDeclaration> GetImportsAsFluxAst() => _packageImports
            .Select(import => import.ToFluxAstNode())
            .ToList();
    }
}
