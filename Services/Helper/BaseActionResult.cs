using Services.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Services.Models;

namespace Services.Helper
{
    public class BaseActionResult : ActionResult
    {
        private BaseResult _result;
        public BaseActionResult()
        {
            _result = new BaseResult();
        }
        public BaseActionResult(ApiResult apiResult)
        {
            BaseResult result = new BaseResult();
            result.Success=apiResult.isSuccess;
            result.Messages=apiResult.message;
            _result = result;
        }
        public BaseActionResult(bool success) : this()
        {
            _result.Success = success;
        }



        public BaseActionResult(bool success, string message) : this(success)
        {
            _result.Messages = message;
        }
        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var modelState = context.ModelState;

            if (!_result.Success.HasValue)
            {
                _result.Success = modelState.IsValid;
                _result.Messages = string.Join("\n", modelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage + "\n" + x.Exception?.Message));
            }
            var jsonSerializer = Newtonsoft.Json.JsonSerializer.Create(new JsonSerializerSettings());
            context.HttpContext.Response.ContentType = "application/json";

            using (var sw = new StringWriter())
            {
                jsonSerializer.Serialize(sw, _result);

                await context.HttpContext.Response.WriteAsync(sw.ToString(), cancellationToken: CancellationToken.None);
            }

        }
    }
    public class BaseResult
    {
        [JsonProperty(PropertyName = "success")]
        public bool? Success { get; set; }

        [JsonProperty(PropertyName = "messages")]
        public string Messages { get; set; }

        //[JsonProperty(PropertyName = "tag")]
        //public object Tag { get; set; }
    }
}
