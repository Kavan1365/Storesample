using Resources;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.AAA
{
    public class ResetPasswordDto
    {
        [Required(AllowEmptyStrings = false,
             ErrorMessageResourceType = typeof(ErrorMessages),
             ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.UserName))]
        public string Username { get; set; }
        [Required(AllowEmptyStrings = false,
           ErrorMessageResourceType = typeof(ErrorMessages),
           ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Code))]
        public string Code { get; set; }
        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Password))]
        public string Password { get; set; }
        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.ConfirmPassword))]
        public string ComfirmPassword { get; set; }
    }

}
