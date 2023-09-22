using System;
using System.Collections.Generic;
using System.Text;

namespace Services.UI.Controls.KCore.Window
{
   public class MvcWindow : MvcBaseControl
    {
        public MvcWindow(ControlFactory controlFactory) : base(controlFactory)
        {
            Options = new WindowOptions();
        }
        private WindowOptions Options { get; set; }
        public MvcWindow Config(Action<WindowOptions> configurator)
        {
            configurator.Invoke(this.Options);
            return this;
        }
        internal override string TagName { get { return "div"; } }

        internal override bool SelfClosing { get { return false; } }

        internal override MvcControlAttributes GetAttributes()
        {
            return Options;
        }

        internal override string GetContent()
        {
            return string.Empty;
        }
    }
}
