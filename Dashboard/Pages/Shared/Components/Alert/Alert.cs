using Dashboard.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Services.Models;
using Services.Services.Base;

namespace Dashboard.Views.Shared.Components.CustomeFormMd6
{
    public class Alert : ViewComponent
    {
        private readonly IHttpService _httpService;
        private readonly IConfiguration _configuration;

        public Alert(IHttpService httpService, IConfiguration configuration)
        {
            _httpService = httpService;
            _configuration = configuration;
        }

        public async Task<ViewViewComponentResult> InvokeAsync(string path)
        {
            var ss = path.Split('/');
            if (path.Split('/').Length < 3)
            {
                path = path + "/Index";
            }
            var obj = await _httpService.Get<GuideViewModel>(_configuration["urlhelper:enternalurl"] + "Alert/GuideByLink?link=" + path); ;

            return View("GetHelps", obj.data);
        }
    }


}
