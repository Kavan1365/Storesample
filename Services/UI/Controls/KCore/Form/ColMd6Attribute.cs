using System;
using System.Collections.Generic;
using System.Text;

namespace Services.UI.Controls.KCore.Form
{
    public class ColMd6Attribute : Attribute
    {
        public bool IsLast { get; set; }
        public ColMd6Attribute(bool isLast)
        {
            IsLast = isLast;
        }
    }
}
