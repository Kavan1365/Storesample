using Dashboard.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Services.Configuration.Helper;
using Services.Models;
using System.Security.Claims;

namespace Dashboard.Pages.Account
{
    public class IndexModel : BasePageAllowModel
    {
       private readonly ILogger<IndexModel> _logger;

        public IndexModel(IHttpService httpService, ILogger<IndexModel> logger, IHttpContextAccessor accessor, IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpService, accessor, httpClientFactory, configuration)
        {
            _logger = logger;
        }
        public async Task<IActionResult> OnGetAsync(string ReturnUrl)
        {
            model = new LoginViewModel();
            var captcha = await _httpService.Get<CaptchaDto>(_configuration["urlhelper:enternalurl"] + "UserClient/Captcha");
            if (User is not null)
            {
                var getpassportIdentity = (ClaimsIdentity)User.Identity;
                if (getpassportIdentity is not null && getpassportIdentity.Claims.Any(z => z.Type == "refresh_token"))
                {
                    var refreshtoken = getpassportIdentity.Claims.Where(z => z.Type == "refresh_token").FirstOrDefault().Value;


                    var data = new
                    {
                        grant_type = "",
                        client_secret = _configuration["AdminConfiguration:ClientSecret"],
                        username = " ",
                        password = " ",
                        refresh_token = refreshtoken,
                        client_id = _configuration["AdminConfiguration:ClientId"],
                        isAdmin = true,
                        CaptchaId = " ",
                        Captcha = " ",
                    };
                    var jwt = await _httpService.Get<AccessToken>(_configuration["urlhelper:enternalurl"] + $"UserClient/GetToken", data, false);
                    if (jwt.isSuccess)
                    {
                        await HttpContext.SignOutAsync();
                        var token = new List<Claim>()
                {
                    new Claim("refresh_token", jwt.data.refresh_token),
                    new Claim(ClaimTypes.Name, jwt.data.access_token),
                    new Claim(ClaimTypes.NameIdentifier, jwt.data.id+""),
                    new Claim("guid", jwt.data.Guid+""),


                };
                        var passportIdentity = new ClaimsIdentity(token, "token");
                        var userPrincipal = new ClaimsPrincipal(new[] { passportIdentity });
                        await HttpContext.SignInAsync(userPrincipal);
                        if (model.ajaxRequest || string.IsNullOrEmpty(ReturnUrl))
                        {
                            return RedirectToPage("/index");

                        }
                        return Redirect(ReturnUrl);

                    }
                    else
                    {
                        _logger.LogError(jwt.message);
                        ModelState.AddModelError("", jwt.message);
                        model.Password = " ";
                        var setting = await _httpService.Get<SettingVeiwModel>(_configuration["urlhelper:enternalurl"] + "setting");
                        settingModel = setting.data;
                        model = new LoginViewModel
                        {
                            returnUrl = ReturnUrl,
                            CaptchaId = captcha.data.CaptchaId,
                            CaptchaUrl = captcha.data.CaptchaImage,
                    Captcha = null
                        };
                        return Page();
                    }
                }
            }


            await HttpContext.SignOutAsync();
            var obj = await _httpService.Get<SettingVeiwModel>(_configuration["urlhelper:enternalurl"] + "setting");
            settingModel = obj.data;
            model = new LoginViewModel
            {
                returnUrl = ReturnUrl,
                CaptchaId = captcha.data.CaptchaId,
                CaptchaUrl = captcha.data.CaptchaImage,
                Captcha = null
            };
            return Page();
        }

        public SettingVeiwModel settingModel { get; set; }
        [BindProperty]
        public LoginViewModel model { get; set; }

        public async Task<IActionResult> OnPostAsync(string ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                var captcha = await _httpService.Get<CaptchaDto>(_configuration["urlhelper:enternalurl"] + "UserClient/Captcha");
                var setting = await _httpService.Get<SettingVeiwModel>(_configuration["urlhelper:enternalurl"] + "setting");
                settingModel = setting.data;
                model = new LoginViewModel
                {
                    returnUrl = ReturnUrl,
                    CaptchaId = captcha.data.CaptchaId,
                    CaptchaUrl = captcha.data.CaptchaImage,
                    Captcha = null
                };
                return Page();
            }

            await HttpContext.SignOutAsync();


            var data = new
            {
                grant_type = "",
                client_secret = _configuration["AdminConfiguration:ClientSecret"],
                username = model.UserName,
                password = model.Password,
                refresh_token = "",
                client_id = _configuration["AdminConfiguration:ClientId"],
                isAdmin = true,
                CaptchaId = model.CaptchaId,
                Captcha = model.Captcha,
            };
            var jwt = await _httpService.Get<AccessToken>(_configuration["urlhelper:enternalurl"] + $"UserClient/GetToken", data, false);
            if (jwt.isSuccess)
            {

                var token = new List<Claim>()
                {
                    new Claim("refresh_token", jwt.data.refresh_token),
                    new Claim(ClaimTypes.Name, jwt.data.access_token),
                    new Claim(ClaimTypes.NameIdentifier, jwt.data.id+""),
                    new Claim("guid", jwt.data.Guid+""),


                };
                var passportIdentity = new ClaimsIdentity(token, "token");
                var userPrincipal = new ClaimsPrincipal(new[] { passportIdentity });
                await HttpContext.SignInAsync(userPrincipal);
                if (model.ajaxRequest || string.IsNullOrEmpty(model.returnUrl))
                {
                    return RedirectToPage("/index");

                }
                return Redirect(model.returnUrl);

            }
            else
            {
                _logger.LogError(jwt.message);
                ModelState.AddModelError("", jwt.message);
                model.Password = "";
                var captcha = await _httpService.Get<CaptchaDto>(_configuration["urlhelper:enternalurl"] + "UserClient/Captcha");
                var setting = await _httpService.Get<SettingVeiwModel>(_configuration["urlhelper:enternalurl"] + "setting");
                settingModel = setting.data;
                model = new LoginViewModel
                {
                    returnUrl = ReturnUrl,
                    CaptchaId = captcha.data.CaptchaId,
                    CaptchaUrl = captcha.data.CaptchaImage,
                    Captcha = null
                };
                return Page();
            }

        }


    }
}
