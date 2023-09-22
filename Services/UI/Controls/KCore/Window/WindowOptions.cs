using System;
using System.Collections.Generic;
using System.Text;

namespace Services.UI.Controls.KCore.Window
{
    public class WindowOptions : MvcControlAttributes
    {
        internal override string Role { get { return "window"; } }
        public string ContentUrl { get; set; }
        public string Title { get; set; }
        public bool Visible { get; set; } = false;
        public bool Resizable { get; set; } = false;
        public bool Modal { get; set; } = true;
        public string OnRefresh { get; set; } = "kendeHandlers.windowOnRefresh";
        internal override void GetAttributes(Dictionary<string, string> attributes)
        {
            attributes["role"] = Role;
            attributes["content"] = ContentUrl;
            attributes["title"] = Title;
            attributes["visible"] = Visible.ToString().ToLower();
            attributes["resizable"] = Resizable.ToString().ToLower();
            attributes["modal"] = Modal.ToString().ToLower();
            attributes["refresh"] = OnRefresh;
            attributes["error"] = "kendeHandlers.windowError";
        }
    }
}
