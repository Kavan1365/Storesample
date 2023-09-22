using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.UI.Controls.KCore
{
    //public abstract class MvcControlAttributes
    //{
    //    internal abstract string Role { get; }
    //    internal string Serialize()
    //    {
    //        var attributes = new Dictionary<string, string>();
    //        GetAttributes(attributes);
    //        var dataAttributes = attributes.Select(item => $"ng-{ item.Key }='{ item.Value }'");
    //        return string.Join(" ", dataAttributes);
    //    }
    //    internal abstract void GetAttributes(Dictionary<string, string> attributes);

    //}
    public abstract class MvcControlAttributes
    {
        internal abstract string Role { get; }
        internal string Serialize()
        {
            var attributes = new Dictionary<string, string>();
            GetAttributes(attributes);
            var dataAttributes = attributes.Select(item => $"data-{ item.Key }='{ item.Value }'");
            return string.Join(" ", dataAttributes);
        }
        internal abstract void GetAttributes(Dictionary<string, string> attributes);

    }
}
