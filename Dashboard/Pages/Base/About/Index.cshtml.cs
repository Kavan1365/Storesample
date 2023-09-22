using Dashboard.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Services.Configuration.Helper;
using Services.Helper;
namespace Dashboard.Pages.Base.About
{
    public class IndexModel : BasePageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IHttpService httpService, ILogger<IndexModel> logger, IHttpContextAccessor accessor, IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpService, accessor, httpClientFactory, configuration)
        {
            _logger = logger;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var obj = await _httpService.Get<AboutStoreViewModel>(_configuration["urlhelper:enternalurl"] + "setting/GetAboutStore");
            model = obj.data;
            return Page();
        }

        [BindProperty]
        public AboutStoreViewModel model { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return new BaseActionResult();

            try
            {
                var obj = await _httpService.Post<AboutStoreViewModel>(_configuration["urlhelper:enternalurl"] + "setting/AboutStore", model);
                return new BaseActionResult(obj.isSuccess, obj.message);
            }
            catch (Exception)
            {
                return new BaseActionResult(false, ErrorMessages.ErrorServer);
            }


        }

    }
}
