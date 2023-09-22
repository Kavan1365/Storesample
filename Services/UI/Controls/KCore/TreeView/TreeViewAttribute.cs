using System;
using System.Collections.Generic;
using System.Text;

namespace Services.UI.Controls.KCore.TreeView
{
    public class TreeViewAttribute : Attribute
    {
        public bool Checkboxes { get; set; }
        public bool CheckChildren { get; set; }
        public string DataImageUrlField { get; set; }
        public string DataTextField { get; set; } = "text";
        public string OnCheck { get; set; }
        public string OnDataBound { get; set; }
        public bool loadOnDemand { get; set; } = true;
        public string DataSourceUrl { get; set; }
        public string LocalSourceFieldName { get; set; }
    }
}
