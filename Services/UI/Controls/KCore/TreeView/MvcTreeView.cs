using System;
using System.Collections.Generic;
using System.Text;

namespace Services.UI.Controls.KCore.TreeView
{
   public class MvcTreeView :MvcBaseControl
    {
        internal override string TagName { get { return "div"; } }

        internal override bool SelfClosing { get { return false; } }

        internal TreeViewOptions Options { get; private set; }

        public MvcTreeView(ControlFactory controlFactory) : base(controlFactory)
        {
            Options = new TreeViewOptions();
        }

        public MvcTreeView Config(Action<TreeViewOptions> configurator)
        {
            configurator.Invoke(this.Options);
            return this;
        }

        public MvcTreeView SetLocalDataSource(List<TreeNode> dataSource)
        {
            Options.LocalDataSource = dataSource;
            return this;
        }

        public MvcTreeView SetDataSourceUrl(string url)
        {
            Options.DataSourceUrl = url;
            return this;
        }

        public MvcTreeView BindEvent(string eventName, string function)
        {
            Options.Events.Add(eventName, function);
            return this;
        }

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
