using BaseCore.Helper.DynamicLinq;
using System.Collections;
using System.Collections.Generic;

namespace BaseCore.Api
{

    public class ApiDataSourceResult
    {
        public IEnumerable Data { get; set; }
        public IEnumerable<GroupResult> Group { get; set; }
        public int Total { get; set; }
        public object Aggregates { get; set; }
        public object Errors { get; set; }
    }



}
