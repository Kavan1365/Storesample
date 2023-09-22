using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Services.UI.Controls.KCore.Grid
{
    public class ColumnValue
    {
        [JsonProperty(PropertyName = "value")]
        public int Value { get; set; }
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
    }
}
