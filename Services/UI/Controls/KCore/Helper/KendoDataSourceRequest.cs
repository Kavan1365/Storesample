using System;
using System.Collections.Generic;

namespace Services.UI.Controls.KCore.Helper
{
    public class MvcDataSourceRequest
    {
        public int Take { get; set; }
        public int Skip { get; set; }
        public IEnumerable<Sort> Sort { get; set; }
        public Filter Filter { get; set; }
        public IEnumerable<Aggregator> Aggregate { get; set; }
        public IEnumerable<Group> Group { get; set; }
        public int? Id { get; set; }
    }
}
