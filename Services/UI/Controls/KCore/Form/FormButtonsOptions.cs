using System;
using System.Collections.Generic;
using System.Text;

namespace Services.UI.Controls.KCore.Form
{
    public class FormButtonsOptions :MvcControlAttributes
    {
        internal override string Role { get { return ""; } }
        public string CallBack { get; set; }
        public bool ShowCancel { get; set; }
        public bool showSave { get; set; }
        internal override void GetAttributes(Dictionary<string, string> attributes)
        {

        }
    }
}
