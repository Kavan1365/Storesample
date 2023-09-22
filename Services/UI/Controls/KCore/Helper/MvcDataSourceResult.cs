using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Services.UI.Controls.KCore.Helper
{
    public class MvcDataSourceResult
    {
        public IEnumerable Data { get; set; }
        public IEnumerable<GroupResult> Group { get; set; }
        public int Total { get; set; }
        public object Aggregates { get; set; }
        public object Errors { get; set; }
    }
}
