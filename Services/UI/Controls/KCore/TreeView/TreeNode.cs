using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Services.UI.Controls.KCore.TreeView
{
    public class TreeNode
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "parentId")]
        public int? ParentId { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "checked")]
        public bool Checked { get; set; }

        [JsonProperty(PropertyName = "showCheckBox")]
        public bool ShowCheckBox { get; set; } = true;

        [JsonProperty(PropertyName = "items", NullValueHandling = NullValueHandling.Ignore)]
        public List<TreeNode> Items { get; set; }

        [JsonIgnore]
        public bool IsRemoteData { get; set; } = true;

        [JsonProperty(PropertyName = "hasChildren")]
        public bool HasChildren { get { return IsRemoteData || Items?.Count > 0; } }

        public object ExtraInfo { get; set; }
    }
}
