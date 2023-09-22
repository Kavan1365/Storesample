using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Services.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace Dashboard.ViewModels.Prodcutes
{
    public class BrandViewModelCreate :BaseViewModel
    {
        [Required(AllowEmptyStrings = false,
           ErrorMessageResourceType = typeof(ErrorMessages),
           ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        [MaxLength(1000)]
        public string Title { get; set; }


        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Image))]
        [FileUpload(1000, ".jpg,.png,.jpeg", false, false, null, null)]
        [DataType(dataType: System.ComponentModel.DataAnnotations.DataType.Upload)]
        public int? ImageId { get; set; }

        [HiddenInput]
        public bool IsSelected { get; set; }

    }
    public class BrandViewModel :BaseViewModel
    {
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        public string Title { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Image))]

        public string ImageUrl { get; set; }

        [HiddenInput]
        public bool IsSelected { get; set; }

    }
}
