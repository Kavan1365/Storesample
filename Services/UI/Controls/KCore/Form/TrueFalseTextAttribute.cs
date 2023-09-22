using System;
using System.Collections.Generic;
using System.Text;

namespace Services.UI.Controls.KCore.Form
{
    public class TrueFalseTextAttribute : Attribute
    {
        public string TrueText { get; set; }
        public string FalseText { get; set; }
        public TrueFalseTextAttribute(string trueText = "بله", string falseText = "خیر")
        {
            TrueText = trueText;
            FalseText = falseText;
        }
    }
}