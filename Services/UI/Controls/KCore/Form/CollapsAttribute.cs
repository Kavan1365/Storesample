using System;
using System.Collections.Generic;
using System.Text;

namespace Services.UI.Controls.KCore.Form
{
    public class CollapsAttribute : Attribute
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public bool Open { get; set; }
        public CollapsAttribute(string title, string id, bool open)
        {
            Title = title;
            Id = id;
            Open = open;

        }
    }

    public class EndCollapsAttribute : Attribute { }
}
