using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;

namespace Berg {
    // https://stackoverflow.com/questions/40334515/automatically-generate-lowercase-dashed-routes-in-asp-net-core
    public class SlugifyParameterTransformer : IOutboundParameterTransformer {
        public string TransformOutbound(object value) {
            // Slugify value
            return value == null ? null : Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }
}