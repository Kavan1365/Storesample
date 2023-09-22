using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Services.UI.Controls.KCore.Grid
{
    public class ToolBarButton
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }
    }
}
