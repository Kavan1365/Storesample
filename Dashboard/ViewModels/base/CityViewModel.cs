using Microsoft.AspNetCore.Mvc;
using Resources;
using Services.Models;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModels
{
    public class ProvinceViewModel : BaseViewModel
    {

        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.Required))]

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        public string Title { get; set; }

    }

    
    public class CityViewModel : BaseViewModel
    {
        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.Required))]

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        public string Title { get; set; }
        [HiddenInput]
        public int ProvinceId { get; set; }
        [HiddenInput]
        public int Province { get; set; }

    }
}
