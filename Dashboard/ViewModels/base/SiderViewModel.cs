using DocumentFormat.OpenXml.Wordprocessing;
using Resources;
using Services.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace Dashboard.ViewModels
{
    public class SiderViewModelCreate : BaseViewModel
    {

        /// <summary>
        /// عنوان سایت
        /// </summary>
        [Required(AllowEmptyStrings = false,
              ErrorMessageResourceType = typeof(ErrorMessages),
              ErrorMessageResourceName = nameof(ErrorMessages.Required))]

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Url))]
        public string UrlLink { get; set; }
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Image))]
        [FileUpload(1000, ".jpg,.png,.jpeg", false, false, null, null)]
        [DataType(dataType: System.ComponentModel.DataAnnotations.DataType.Upload)]
        public int ImageId { get; set; }
    }
    public class SiderViewModel : BaseViewModel
    {

        /// <summary>
        /// عنوان سایت
        /// </summary>
        [Required(AllowEmptyStrings = false,
              ErrorMessageResourceType = typeof(ErrorMessages),
              ErrorMessageResourceName = nameof(ErrorMessages.Required))]

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Url))]
        public string UrlLink { get; set; }
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Image))]
        public string ImageUrl { get; set; }
    }
}
