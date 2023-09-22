using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Services.UI.Controls.KCore.Helper
{
    public class Group : Sort
    {
        [DataMember(Name = "aggregates")]
        public IEnumerable<Aggregator> Aggregates { get; set; }
    }
}
