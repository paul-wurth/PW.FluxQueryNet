using System.Text;

namespace Flux.Net
{
    public partial class FluxQueryBuilder
    {
        private readonly StringBuilder _stringBuilder;

        private FluxQueryBuilder(StringBuilder stringBuilder) => _stringBuilder = stringBuilder;


        public string ToQuery()
        {
            return _stringBuilder.ToString();
        }
    }
}
