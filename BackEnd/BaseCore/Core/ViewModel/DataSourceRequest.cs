using System.Collections.Generic;
using System.Linq;
using BaseCore.Helper.DynamicLinq;
using Newtonsoft.Json;

namespace BaseCore.ViewModel
{
    public class DataSourceRequest
    {
        public int skip { get; set; }

        public int take { get; set; } = 50;
        //public int page { get; set; }
        //public int pageSize { get; set; }
        public List<SortDescription> sort { get; set; }
        [JsonProperty("filter")]
        public FilterDescription Filter { get; set; }

        public bool HasFilter()
        {
            return Filter?.Filters != null && Filter.Filters.Any();
        }


        public IEnumerable<Aggregator> Aggregate { get; set; }
        public IEnumerable<Group> Group { get; set; }
    }
    public class DataSourceRequestTree
    {
        public int Take { get; set; }
        public int Skip { get; set; }
        public IEnumerable<SortTree> Sort { get; set; }
        public FilterTreeList Filter { get; set; }
        public IEnumerable<Aggregator> Aggregate { get; set; }
        public IEnumerable<Group> Group { get; set; }
        public int? Id { get; set; }
    }
}
