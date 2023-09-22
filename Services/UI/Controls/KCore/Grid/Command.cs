using System;
using System.Collections.Generic;
using System.Text;
using Services.Json;
using Newtonsoft.Json;

namespace Services.UI.Controls.KCore.Grid
{
    public class Command
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "click", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(JsonRawConverter))]
        public string Click { get; set; }

        [JsonProperty(PropertyName = "visible", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(JsonRawConverter))]
        public string Visible { get; set; }

        [JsonProperty(PropertyName = "template", NullValueHandling = NullValueHandling.Ignore)]
        public string Template { get; set; }
    }
}
