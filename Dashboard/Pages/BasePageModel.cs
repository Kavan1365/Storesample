using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Services.Configuration.Helper;

namespace Dashboard.Pages
{
    [Authorize]
    public class BasePageModel: PageModel
    {
        public readonly IConfiguration _configuration;
        public readonly IHttpClientFactory _httpClientFactory;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IHttpService _httpService;
        public BasePageModel(IHttpService httpService, IHttpContextAccessor accessor, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = accessor;
            _httpService = httpService;
        }
    }
    public class BasePageAllowModel: PageModel
    {
        public readonly IConfiguration _configuration;
        public readonly IHttpClientFactory _httpClientFactory;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IHttpService _httpService;
        public BasePageAllowModel(IHttpService httpService, IHttpContextAccessor accessor, IHttpClientFactory httpClientFactory,IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = accessor;
            _httpService = httpService;


        }


    }
}
