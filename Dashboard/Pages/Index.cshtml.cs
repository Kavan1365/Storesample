using Services.Configuration.Helper;


namespace Dashboard.Pages
{
    public class IndexModel : BasePageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(IHttpService httpService, ILogger<IndexModel> logger, IHttpContextAccessor accessor, IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpService, accessor, httpClientFactory, configuration)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    
    }
    }