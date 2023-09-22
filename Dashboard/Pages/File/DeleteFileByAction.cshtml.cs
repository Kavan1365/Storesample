using Microsoft.AspNetCore.Mvc;
using Services.Configuration.Helper;
using Services.Helper;
using Services.Models;
using Resources;
namespace Dashboard.Pages.File
{
    public class DeleteFileByActionModel : BasePageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public DeleteFileByActionModel(IHttpService httpService, ILogger<IndexModel> logger, IHttpContextAccessor accessor, IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpService, accessor, httpClientFactory, configuration)
        {
            _logger = logger;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string action, string name, int id)
        {
            if (!ModelState.IsValid)
                return new BaseActionResult();

            try
            {
                var obj = await _httpService.Post(_configuration["urlhelper:enternalurl"] +  "file/DeleteFile/" +id);
                return new BaseActionResult(obj.isSuccess, obj.message);
            }
            catch (Exception)
            {
                return new BaseActionResult(false, ErrorMessages.ErrorServer);
            }


        }

    }
}
