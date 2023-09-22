using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Configuration.Helper;
using Services.Models;

namespace Dashboard.Pages.File
{
    public class DeleteModel : BasePageAllowModel
    {
        private readonly ILogger<IndexModel> _logger;
        private HttpClient _httpClient;


        public DeleteModel(IHttpService httpService, HttpClient httpClient, ILogger<IndexModel> logger, IHttpContextAccessor accessor, IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpService, accessor, httpClientFactory, configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(int guid,CancellationToken cancellationToken)
        {
           await _httpService.Delete(_configuration["urlhelper:enternalurl"]+ "file/DeleteFile/" + guid);
            return Content("");

        }
    }
}
