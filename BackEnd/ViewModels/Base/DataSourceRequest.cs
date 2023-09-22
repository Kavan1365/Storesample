using BaseCore.Helper.DynamicLinq;
using Newtonsoft.Json;

namespace ViewModels
{
    public class DataSourceRequest
    {
        public int skip { get; set; }

        public int take { get; set; } = 10;
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
}
