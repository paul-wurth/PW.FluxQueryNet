using System.Text;

namespace PW.FluxQueryNet.Extensions
{
    internal static class StringBuilderExtensions
    {
        public static StringBuilder AppendPipe(this StringBuilder stringBuilder) => stringBuilder.Append("|> ");

        public static StringBuilder AppendStringArrayParameter(this StringBuilder stringBuilder,
            string parameterName, string[] values, bool prefixComma = false) // TODO: parameterize string arrays
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
