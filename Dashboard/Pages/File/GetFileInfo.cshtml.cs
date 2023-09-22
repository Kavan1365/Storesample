using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Configuration.Helper;
using Services.Models;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Dashboard.Pages.File
{
    public class GetFileInfoModel : BasePageAllowModel
    {
        private readonly ILogger<IndexModel> _logger;
        private HttpClient _httpClient;


        public GetFileInfoModel(IHttpService httpService, HttpClient httpClient, ILogger<IndexModel> logger, IHttpContextAccessor accessor, IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpService, accessor, httpClientFactory, configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var result = await _httpService.Get<FileViewModel>(_configuration["urlhelper:enternalurl"] + "file/GetInfoFile/" + id);
            if (result.isSuccess&&result.data!=null)
            {
                return Content(JsonConvert.SerializeObject(result.data));
            }
            return null;
        }
        
    }
}
