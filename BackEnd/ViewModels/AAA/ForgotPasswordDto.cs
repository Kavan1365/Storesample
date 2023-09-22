using Resources;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.AAA
{
    public class ForgotPasswordDto
    {
        [Required(AllowEmptyStrings = false,
               ErrorMessageResourceType = typeof(ErrorMessages),
               ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.UserName))]
        public string Username { get; set; }
       
    }

}
