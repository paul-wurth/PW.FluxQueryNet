using System.Text;

namespace Flux.Net.Extensions
{
    internal static class StringBuilderExtensions
    {
        public static StringBuilder AppendStringArrayParameter(this StringBuilder stringBuilder,
            string parameterName, string[] values, bool prefixComma = false)
        {
            if (values == null || values.Length == 0)
                return stringBuilder;

            if (prefixComma)
                stringBuilder.Append(", ");

            stringBuilder.Append(parameterName).Append(": [");
            for (int i = 0; i < values.Length; i++)
            {
                if (i > 0)
                    stringBuilder.Append(", ");

                stringBuilder.Append('"').Append(values[i]).Append('"');
            }
            return stringBuilder.Append(']');
        }
    }
}
