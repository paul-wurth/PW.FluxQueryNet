using System.Collections.Generic;
using System.Text;

namespace PW.FluxQueryNet
{
    public partial class FluxQueryBuilder
    {
        private readonly HashSet<string> _packages = new();
        private readonly StringBuilder _stringBuilder;

        private FluxQueryBuilder(StringBuilder stringBuilder) => _stringBuilder = stringBuilder;


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
