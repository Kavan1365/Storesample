using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.UI.Controls.KCore.Editor
{
    public class MvcEditor : MvcBaseControl
    {
        internal override string TagName { get { return "textarea"; } }
        internal override bool SelfClosing { get { return false; } }
        internal string Value { get; set; }

        internal EditorOptions Options { get; private set; }

        public MvcEditor(ControlFactory controlFactory) : base(controlFactory)
        {
            Options = new EditorOptions();
        }

        internal override MvcControlAttributes GetAttributes()
        {
            return Options;
        }

        internal override string GetContent()
        {
            return ControlFactory.HtmlHelper.Raw(Value).ToString();
        }
    }
}
