using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Services.UI.Controls.KCore.TreeView
{
    public class TreeViewOptions : MvcControlAttributes
    {

        public TreeViewOptions()
        {
            Events = new Dictionary<string, string>();
        }
        internal override string Role { get { return "treeview"; } }
        public bool Checkboxes { get; set; }
        internal bool AllowDelete { get; set; }
        internal bool AllowEdite { get; set; }
        internal bool AllowCreate { get; set; }
        public bool CheckChildren { get; set; } = false;
        public string DataImageUrlField { get; set; }
        public string DataTextField { get; set; } = "text";
        public string OnCheck { get; set; }
        public string OnDataBound { get; set; }
        public bool loadOnDemand { get; set; } = true;
        internal string DataSourceUrl { get; set; }
        internal List<TreeNode> LocalDataSource { get; set; }
        internal string CheckBoxesInputName { get; set; }

        internal Dictionary<string, string> Events { get; set; }

        internal override void GetAttributes(Dictionary<string, string> attributes)
        {
            attributes["role"] = Role;

            attributes["source"] = JsonConvert.SerializeObject(GetDataSource(),
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            if (!string.IsNullOrEmpty(OnCheck))
                attributes["check"] = JsonConvert.SerializeObject(new JRaw(OnCheck));
            if (!string.IsNullOrEmpty(OnDataBound))
                attributes["bound"] = JsonConvert.SerializeObject(new JRaw(OnDataBound));

            foreach (var eventName in Events.Keys)
            {
                attributes[eventName] = Events[eventName];
            }

            if (AllowDelete)
                attributes["AllowDelete"] = JsonConvert.SerializeObject(new
                {
                    template = $"#if(item.AllowDelete){{# <a class=\"k-icon k-i-close-outline\" id=\"#= item.id #\"></a>#}}#",
                    checkChildren = CheckChildren,
                },
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            if (Checkboxes)
                attributes["checkboxes"] = JsonConvert.SerializeObject(new
                {
                    template = $"#if(item.showCheckBox){{# <input type=\"checkbox\" #=(item.checked ? \"checked\" : \"\")# name=\"{ CheckBoxesInputName }\" value=\"#= item.id #\" /> #}}#",
                    checkChildren = CheckChildren,
                    name = CheckBoxesInputName,
                },
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            attributes["image-url-field"] = DataImageUrlField;
            attributes["load-on-demand"] = loadOnDemand.ToString().ToLower();
            attributes["text-field"] = DataTextField;
        }

        private object GetDataSource()
        {
            if (LocalDataSource == null)

                return new
                {
                    transport = new
                    {
                        read = new { url = DataSourceUrl, dataType = "json", cache = false }
                    },
                    schema = new
                    {
                        model = new
                        {
                            id = "id",
                            hasChildren = "hasChildren"
                        }
                    },
                };

            else
                return LocalDataSource;

        }
    }
}

