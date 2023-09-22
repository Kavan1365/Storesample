using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Resources;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace Dashboard.ViewModels
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.Required))]

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.UserName))]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.Required))]

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Password))]
        public string Password { get; set; }
        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.Required))]

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Captcha))]
        public string Captcha{ get; set; }

        //[Display(Name = "مرا بخاطر بسپار")]
        //public bool RememberMe { get; set; }
        public string returnUrl { get; set; }

     
        [HiddenInput]
        public bool ajaxRequest { get; set; }
        [HiddenInput]
        public string CaptchaUrl { get; set; }
        [HiddenInput]
        public string CaptchaId { get; set; }
    }
}
