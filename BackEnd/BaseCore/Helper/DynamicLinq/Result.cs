using System.Collections.Generic;

namespace BaseCore.Helper.DynamicLinq
{
    public class KendoGridResult<T>
    {
        public List<T> Data { get; set; }
        public int Total { get; set; }
    }
}
