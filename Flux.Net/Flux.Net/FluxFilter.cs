using System.Collections.Generic;
using System.Text;

namespace Flux.Net
{
    public class FluxFilter
    {
        private readonly StringBuilder _stringBuilder;
        private bool _firstCondition = true;

        internal FluxFilter(StringBuilder stringBuilder)
        {
            _stringBuilder = stringBuilder;
        }

        private void AppendAndIfNeeded()
        {
            if (_firstCondition)
                _firstCondition = false;
            else
                _stringBuilder.Append(" and ");
        }

        public FluxFilter Where(string filters)
        {
            AppendAndIfNeeded();
            _stringBuilder.Append(filters);

            return this;
        }

        public FluxFilter Measurement(string measurement)
        {
            AppendAndIfNeeded();
            _stringBuilder.Append("r._measurement == \"").Append(measurement).Append("\"");

            return this;
        }

        public FluxFilter Tag(string tagKey, string tagValue)
        {
            AppendAndIfNeeded();
            _stringBuilder.Append("r[\"").Append(tagKey).Append("\"] == \"").Append(tagValue).Append("\"");

            return this;
        }

        public FluxFilter Tags(IDictionary<string, string> tags)
        {
            if (tags == null)
                return this;

            foreach (var tag in tags)
                Tag(tag.Key, tag.Value);

            return this;
        }

        public FluxFilter Fields(params string[] fields)
        {
            if (fields == null || fields.Length == 0)
                return this;

            AppendAndIfNeeded();

            _stringBuilder.Append("(");
            for (int i = 0; i < fields.Length; i++)
            {
                if (i > 0)
                    _stringBuilder.Append(" or ");

                _stringBuilder.Append("r._field == \"").Append(fields[i]).Append("\"");
            }
            _stringBuilder.Append(")");

            return this;
        }
    }
}
