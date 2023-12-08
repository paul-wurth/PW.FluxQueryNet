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

        /// <summary>
        /// Add conditions to the filter predicate with raw Flux.
        /// </summary>
        /// <remarks>Records representing each row are passed as <c>r</c>.</remarks>
        /// <param name="filters">A raw Flux predicate (eg. <c>"r._value &gt; 0 and r._value &lt; 50"</c>).</param>
        public FluxFilter Where(string filters)
        {
            AppendAndIfNeeded();
            _stringBuilder.Append(filters);

            return this;
        }

        /// <summary>
        /// Add a condition to the filter predicate to keep only records with the specified <paramref name="measurement"/>.
        /// </summary>
        /// <param name="measurement">Name of the measurement to filter records.</param>
        public FluxFilter Measurement(string measurement)
        {
            AppendAndIfNeeded();
            _stringBuilder.Append("r._measurement == \"").Append(measurement).Append('"');

            return this;
        }

        /// <summary>
        /// Add a condition to the filter predicate to keep only records with the specified <paramref name="tagKey"/> and <paramref name="tagValue"/>.
        /// </summary>
        /// <param name="tagKey">Key of the tag to filter.</param>
        /// <param name="tagValue">Value of the tag to filter.</param>
        public FluxFilter Tag(string tagKey, string tagValue)
        {
            AppendAndIfNeeded();
            _stringBuilder.Append("r[\"").Append(tagKey).Append("\"] == \"").Append(tagValue).Append('"');

            return this;
        }

        /// <summary>
        /// Add conditions to the filter predicate to keep only records with the specified <paramref name="tags"/>.
        /// </summary>
        /// <param name="tags">A dictionary of tag keys and values to filter records.</param>
        public FluxFilter Tags(IDictionary<string, string> tags)
        {
            if (tags == null)
                return this;

            foreach (var tag in tags)
                Tag(tag.Key, tag.Value);

            return this;
        }

        /// <summary>
        /// Add conditions to the filter predicate to keep only records with any specified <paramref name="fields"/>.
        /// </summary>
        /// <param name="fields">An array of field keys to filter records.</param>
        public FluxFilter Fields(params string[] fields)
        {
            if (fields == null || fields.Length == 0)
                return this;

            AppendAndIfNeeded();

            _stringBuilder.Append('(');
            for (int i = 0; i < fields.Length; i++)
            {
                if (i > 0)
                    _stringBuilder.Append(" or ");

                _stringBuilder.Append("r._field == \"").Append(fields[i]).Append('"');
            }
            _stringBuilder.Append(')');

            return this;
        }
    }
}
