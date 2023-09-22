using Dashboard.ViewModels;
using Dashboard.ViewModels.Prodcutes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Resources;
using Services.Configuration.Helper;
using Services.Helper;
using System.Security.Claims;

namespace Dashboard.Pages.Base.Province
{
    public class CreateModel : BasePageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public CreateModel(IHttpService httpService, ILogger<IndexModel> logger, IHttpContextAccessor accessor, IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpService, accessor, httpClientFactory, configuration)
        {
            _logger = logger;
        }

        [BindProperty]
        public ProvinceViewModel model { get; set; }
        public async Task OnGetAsync(Guid? guid)
        {
            model = new ProvinceViewModel();
            if (guid.HasValue)
            {
                var obj = await _httpService.Get<ProvinceViewModel>(_configuration["urlhelper:enternalurl"] + $"Province/{guid.Value}");
                model = obj.data;
            }


        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return new BaseActionResult();

            try
            {
                if (model.Id > 0)
                {
                    var obj = await _httpService.Post<ProvinceViewModel>(_configuration["urlhelper:enternalurl"] + $"Province/update", model);
                    return new BaseActionResult(obj.isSuccess, obj.message);
                }
                else
                {
                    var obj = await _httpService.Post<ProvinceViewModel>(_configuration["urlhelper:enternalurl"] + $"Province", model);
                    return new BaseActionResult(obj.isSuccess, obj.message);
                }

            }
            catch (Exception)
            {
                return new BaseActionResult(false, ErrorMessages.ErrorServer);
            }


        }
    }
}
