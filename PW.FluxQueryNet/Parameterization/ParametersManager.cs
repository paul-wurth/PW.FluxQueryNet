using System;
using System.Collections.Generic;
using System.Linq;

namespace PW.FluxQueryNet.Parameterization
{
    public class ParametersManager
    {
        private const string Prefix = "params";

        private readonly Dictionary<string, object?> _parameters = new();

        public string Parameterize(string paramName, object? value)
        {
            paramName = $"{paramName}_{_parameters.Count}";
            _parameters.Add(paramName, value);

            return $"{Prefix}.{paramName}";
        }

        internal string Parameterize(FormattableString formattableString, string paramName)
        {
            if (formattableString.ArgumentCount < 1)
                return formattableString.Format;

            var paramNames = formattableString.GetArguments()
                .Select(arg => Parameterize(paramName, arg))
                .ToArray();

            return string.Format(formattableString.Format, paramNames);
        }
    }
}
