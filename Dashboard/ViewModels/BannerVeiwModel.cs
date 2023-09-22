using Resources;
using Services.Models;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModels
{
    public class BannerVeiwModel : BaseViewModel
    {

        /// <summary>
        /// عنوان سایت
        /// </summary>
        [StringLength(500,
              ErrorMessageResourceType = typeof(ErrorMessages),
              ErrorMessageResourceName = nameof(ErrorMessages.StringLength))]
        [Required(AllowEmptyStrings = false,
              ErrorMessageResourceType = typeof(ErrorMessages),
              ErrorMessageResourceName = nameof(ErrorMessages.Required))]

        [Display(Name = "لینک بنر یک سمت راست")]
        [MaxLength(2000)]
        public string BannerOneRightUrl { get; set; }
        [Display(Name = "بنر یک سمت راست")]
        [FileUpload(1000, ".jpg,.png,.jpeg", false, false, null, null)]
        [DataType(dataType: System.ComponentModel.DataAnnotations.DataType.Upload)]
        public int? BannerOneRightId { get; set; }


        [MaxLength(2000)]
        [Display(Name = "لینک بنر یک سمت چپ")]
        public string BannerOneLeftUrl { get; set; }
        [Display(Name = "بنر یک سمت چپ")]
        [FileUpload(1000, ".jpg,.png,.jpeg", false, false, null, null)]
        [DataType(dataType: System.ComponentModel.DataAnnotations.DataType.Upload)]
        public int? BannerOneLeftId { get; set; }



        [MaxLength(2000)]
        [Display(Name ="لینک بنر دوم ")]
        public string BannerTwoUrl { get; set; }
        [Display(Name = "بنر دوم")]
        [FileUpload(1000, ".jpg,.png,.jpeg", false, false, null, null)]
        [DataType(dataType: System.ComponentModel.DataAnnotations.DataType.Upload)]
        public int? BannerTwoId { get; set; }

        [MaxLength(2000)]
        [Display(Name ="لینک بنر آخر ")]
        public string BannerThreeUrl { get; set; }
        [Display(Name ="بنر آخر")]
        [FileUpload(1000, ".jpg,.png,.jpeg", false, false, null, null)]
        [DataType(dataType: System.ComponentModel.DataAnnotations.DataType.Upload)]
        public int? BannerThreeId { get; set; }

    }

}
