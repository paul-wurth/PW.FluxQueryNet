using InfluxDB.Client.Api.Domain;
using PW.FluxQueryNet.FluxTypes.Converters;
using System;

namespace PW.FluxQueryNet
{
    /// <summary>
    /// Represents a package to import, with an optional alias.
    /// </summary>
    /// <seealso href="https://docs.influxdata.com/flux/latest/spec/packages/#import-declaration">Package import declaration - InfluxDB documentation</seealso>
    public sealed class FluxPackageImport : IEquatable<FluxPackageImport>
    {
        public string Package { get; }
        public string? Alias { get; }

        public FluxPackageImport(string package, string? alias = null)
        {
            Package = package;
            Alias = alias;
        }

        public static implicit operator FluxPackageImport(string value) => new(value);

        public override string ToString() => ToFluxNotation();

        public string ToFluxNotation() => $"import {Alias} {Package.ToFluxNotation()}";

        public ImportDeclaration ToFluxAstNode() => new(nameof(ImportDeclaration),
            _as: Alias == null ? null : new(nameof(Identifier), Alias),
            path: Package.ToFluxAstNode());


        public bool Equals(FluxPackageImport? other)
        {
            if (other == null)
                return false;

            return Package == other.Package && Alias == other.Alias;
        }

        public override bool Equals(object? obj) => Equals(obj as FluxPackageImport);

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 17;

                if (Package != null)
                    hashCode = hashCode * 59 + Package.GetHashCode();

                if (Alias != null)
                    hashCode = hashCode * 59 + Alias.GetHashCode();

                return hashCode;
            }
        }
    }
}
