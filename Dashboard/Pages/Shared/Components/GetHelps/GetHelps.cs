using Dashboard.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Services.Models;
using Services.Services.Base;

namespace Dashboard.Views.Shared.Components.CustomeFormMd6
{
    public class GetHelps : ViewComponent
    {
        private readonly IHttpService _httpService;
        private readonly IConfiguration _configuration;

        public GetHelps(IHttpService httpService, IConfiguration configuration)
        {
            _httpService = httpService;
            _configuration = configuration;
        }

        public async Task<ViewViewComponentResult> InvokeAsync()
        {
            var path = "";
            if (HttpContext.Request.Path.HasValue)
            {
                path = HttpContext.Request.Path.Value.ToLower();
            }
            var obj = await _httpService.Get<GuideViewModel>(_configuration["urlhelper:enternalurl"] + "Guide/GuideByLink?link=" + path + "&TypePanel=" + TypePanel.Home); ;

            return View("GetHelps", obj.data);
        }
    }


}
