using Dashboard.ViewModels;
using Dashboard.ViewModels.Prodcutes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Resources;
using Services.Configuration.Helper;
using Services.Helper;
using System.Security.Claims;

namespace Dashboard.Pages.Base.City
{
    public class CreateModel : BasePageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public CreateModel(IHttpService httpService, ILogger<IndexModel> logger, IHttpContextAccessor accessor, IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpService, accessor, httpClientFactory, configuration)
        {
            _logger = logger;
        }

        [BindProperty]
        public CityViewModel model { get; set; }
        public async Task OnGetAsync(int provinceId, Guid? guid)
        {
            model = new CityViewModel();
            model.ProvinceId = provinceId;
            if (guid.HasValue)
            {
                var obj = await _httpService.Get<CityViewModel>(_configuration["urlhelper:enternalurl"] + $"City/{guid.Value}");
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
                    var obj = await _httpService.Post<CityViewModel>(_configuration["urlhelper:enternalurl"] + $"City/update", model);
                    return new BaseActionResult(obj.isSuccess, obj.message);
                }
                else
                {
                    var obj = await _httpService.Post<CityViewModel>(_configuration["urlhelper:enternalurl"] + $"City", model);
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
