using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flux.Net
{
    public class FluxFilter
    {
        private readonly List<string> _filters = new();
        private readonly List<FluxSelect> _selections = new();

        public FluxFilter Where(string filters)
        {
            _filters.Add(filters);
            return this;
        }

        public FluxFilter Where(IDictionary<string, string> tagFilters)
        {
            if (tagFilters == null || tagFilters.Count == 0)
                return this;

            var conditions = tagFilters.Select(kvp => $"r[\"{kvp.Key}\"] == \"{kvp.Value}\"");
            _filters.AddRange(conditions);

            return this;
        }

        public FluxFilter Measurement(string measurement)
        {
            _filters.Add($"r._measurement == \"{measurement}\"");
            return this;
        }

        public FluxFilter Select(Action<FluxSelect> filter)
        {
            var select = new FluxSelect();
            filter.Invoke(select);
            _selections.Add(select);

            return this;
        }

        internal void Build(StringBuilder stringBuilder)
        {
            if (_filters.Count == 0 && _selections.Count == 0)
                return;

            stringBuilder.Append("|> filter(fn: (r) => ");

            for (int i = 0; i < _filters.Count; i++)
            {
                if (i > 0)
                    stringBuilder.Append(" and ");

                stringBuilder.Append(_filters[i]);
            }

            for (int i = 0; i < _selections.Count; i++)
            {
                if (i > 0 || _filters.Count > 0)
                    stringBuilder.Append(" and ");

                stringBuilder.Append("(");
                _selections[i].Build(stringBuilder);
                stringBuilder.Append(")");
            }

            stringBuilder.Append(")");
        }
    }

    public class FluxSelect
    {
        private readonly List<string> _selections = new();

        public FluxSelect Fields(params string[] fields)
        {
            if (fields == null || fields.Length == 0)
                return this;

            var selections = string.Join(" or ", fields.Select(field => $"r._field == \"{field}\""));
            _selections.Add(selections);

            return this;
        }

        [Obsolete("InfluxDB tags cannot be selected this way because \"r.tag\" doesn't exist by default. Please use Where() instead.")]
        public FluxSelect Tags(params string[] tags)
        {
            if (tags == null || tags.Length == 0)
                return this;

            var selections = string.Join(" or ", tags.Select(tag => $"r.tag == \"{tag}\""));
            _selections.Add(selections);

            return this;
        }

        internal void Build(StringBuilder stringBuilder)
        {
            for (int i = 0; i < _selections.Count; i++)
            {
                if (i > 0)
                    stringBuilder.Append(" and ");

                stringBuilder.Append(_selections[i]);
            }
        }
    }
}
