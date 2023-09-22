using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Services.UI.Controls.KCore.Editor
{
    public class Tool
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "tooltip", NullValueHandling = NullValueHandling.Ignore)]
        public string Tooltip { get; set; }

        [JsonProperty(PropertyName = "template", NullValueHandling = NullValueHandling.Ignore)]
        public string Template { get; set; }

        [JsonProperty(PropertyName = "exec", NullValueHandling = NullValueHandling.Ignore)]
        public JRaw Exec { get; set; }

    }
}
