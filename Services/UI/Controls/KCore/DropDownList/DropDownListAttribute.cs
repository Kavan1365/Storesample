using System;


namespace Services.UI.Controls.KCore.DropDownList
{
    public class DropDownListAttribute : Attribute
    {
        public string LocalSourceFieldName { get; set; }
        public string DataSourceUrl { get; set; }
        public string OptionLabel { get; set; } = "انتخاب کنید...";
        public string DataTextField { get; set; } = "Title";
        public string DataValueField { get; set; } = "Id";
        public string DataGroupField { get; set; } = "Group";
        public bool Grouping { get; set; }
        public bool AutoBind { get; set; }
        public bool Multiple { get; set; }
        public string Method { get; set; } = "GEt";
        public bool ParameterMap { get; set; }
        public string CascadeFrom { get; set; }
        public string CascadeFromField { get; set; }
        public int MinLength { get; set; } = 1;
        public bool EnforceMinLength { get; set; } = false;
        public string CreateUrl { get; set; }

    }

}
