using Dashboard.ViewModels;
using Services.Configuration.Helper;

namespace Dashboard.Pages.Base.City
{
    public class IndexModel : BasePageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IHttpService httpService, ILogger<IndexModel> logger, IHttpContextAccessor accessor, IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpService, accessor, httpClientFactory, configuration)
        {
            _logger = logger;
        }
        public Guid guid { get; set; }
        public int provinceId { get; set; }
        public async Task OnGetAsync(Guid guid)
        {
            this.guid = guid;
            var obj = await _httpService.Get<ProvinceViewModel>(_configuration["urlhelper:enternalurl"] + $"Province/{guid}");
            provinceId = obj.data.Id;
        }
    }
}
