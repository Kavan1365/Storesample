using System;
using System.Collections;
using System.Linq;

namespace Services.UI.Controls.KCore.DropDownList
{
    public class MvcDropDownList : MvcBaseControl
    {
        internal override string TagName { get { return  "input"; } }
        internal override bool SelfClosing { get { return TagName == "input"; } }
        internal DropDownListOptions Options { get; private set; }


        public MvcDropDownList(ControlFactory controlFactory) : base(controlFactory)
        {
            Options = new DropDownListOptions();
        }

        public MvcDropDownList Config(Action<DropDownListOptions> configurator)
        {
            configurator.Invoke(this.Options);
            return this;
        }

        public MvcDropDownList SetLocalDataSource(IEnumerable dataSource)
        {
     
            if (dataSource != null)
            {

                Options.LocalDataSource = dataSource;
               //var col = dataSource.GetEnumerator();
                //Options.Filter = col. > 10 ? "contains" : "none";
                Options.Filter = "contains";
                Options.AutoBind = true;
            }

            return this;
        }

        public MvcDropDownList SetDataSourceUrl(string url)
        {
            Options.DataSourceUrl = url;
            return this;
        }

        public MvcDropDownList SetValue(object value)
        {
            if (value == null)
                return this;
            int result;
            bool checkint = int.TryParse(value+"", out result);


            if (value is string || checkint)
                Options.Value = value.ToString();

            if (value is Array)
                Options.Value = $"[{string.Join(",", ((Array)value).OfType<int>().Select(x => x.ToString()))}]";

            if (Options.Value == "[]" && value is Array)
                Options.Value = $"[{string.Join(",", ((Array)value).OfType<string>().Select(x =>$"{(char)34 +  x.ToString() + (char)34}"))}]";

            return this;
        }

        public MvcDropDownList BindEvent(string eventName, string function)
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
