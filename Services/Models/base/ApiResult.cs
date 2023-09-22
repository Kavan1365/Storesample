using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class ApiResult
    {
        public bool isSuccess { get; set; }
        public int statusCode { get; set; }
        public string message { get; set; }


    }
    public class ApiResult<TData> : ApiResult
       where TData : class
    {
        public TData data { get; set; }


    }


    public class ApiResultCustome
    {
        public object Group { get; set; }
        public string Total { get; set; }
        public string Errors { get; set; }
        public object Aggregates { get; set; }


    }
    public class ApiResultCustome<TData> : ApiResult
       where TData : class
    {
        public List<TData> Data { get; set; }
        public object Group { get; set; }
        public string Total { get; set; }
        public string Errors { get; set; }
        public object Aggregates { get; set; }

    }
}
