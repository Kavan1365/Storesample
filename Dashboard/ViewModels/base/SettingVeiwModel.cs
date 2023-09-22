using Microsoft.AspNetCore.Mvc;
using Services.Models;
using Resources;
using System.ComponentModel.DataAnnotations;
using Services.UI.Controls.KCore.DropDownList;

namespace Dashboard.ViewModels
{
    public class SettingVeiwModel : BaseViewModel
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

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        public string Title { get; set; }
     
        /// <summary>
        /// seo
        /// 
        /// </summary>
        [StringLength(1000,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.StringLength))]
        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Url))]
        public string Url { get; set; }

        /// <summary>
        /// لینک چک انلاین
        /// 
        /// </summary>
        [StringLength(1000,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.StringLength))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Boxchat))]
        public string Boxchat { get; set; }

        [StringLength(1000,
               ErrorMessageResourceType = typeof(ErrorMessages),
               ErrorMessageResourceName = nameof(ErrorMessages.StringLength))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.linkedin))]
        public string linkedin { get; set; }
        [StringLength(1000,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.StringLength))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.youtube))]
        public string youtube { get; set; }
        [StringLength(1000,
           ErrorMessageResourceType = typeof(ErrorMessages),
           ErrorMessageResourceName = nameof(ErrorMessages.StringLength))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.aparat))]
        public string aparat { get; set; }
        [StringLength(1000,
          ErrorMessageResourceType = typeof(ErrorMessages),
          ErrorMessageResourceName = nameof(ErrorMessages.StringLength))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.whatsapp))]
        public string whatsapp { get; set; }
        [StringLength(1000,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.StringLength))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.twitter))]
        public string twitter { get; set; }
        [StringLength(1000,
           ErrorMessageResourceType = typeof(ErrorMessages),
           ErrorMessageResourceName = nameof(ErrorMessages.StringLength))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Instagram))]
        public string instagram { get; set; }
        [StringLength(1000,
          ErrorMessageResourceType = typeof(ErrorMessages),
          ErrorMessageResourceName = nameof(ErrorMessages.StringLength))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Telegram))]
        public string Telegram { get; set; }
        [StringLength(1000,
          ErrorMessageResourceType = typeof(ErrorMessages),
          ErrorMessageResourceName = nameof(ErrorMessages.StringLength))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.facebook))]
        public string facebook { get; set; }
        [StringLength(1000,
          ErrorMessageResourceType = typeof(ErrorMessages),
          ErrorMessageResourceName = nameof(ErrorMessages.StringLength))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.srwsh))]
        public string srwsh { get; set; }
        [StringLength(1000,
           ErrorMessageResourceType = typeof(ErrorMessages),
           ErrorMessageResourceName = nameof(ErrorMessages.StringLength))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.ay_gp))]
        public string ay_gp { get; set; }
        [StringLength(1000,
           ErrorMessageResourceType = typeof(ErrorMessages),
           ErrorMessageResourceName = nameof(ErrorMessages.StringLength))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.ayta))]
        public string ayta { get; set; }
        [StringLength(1000,
           ErrorMessageResourceType = typeof(ErrorMessages),
           ErrorMessageResourceName = nameof(ErrorMessages.StringLength))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.blh))]
        public string blh { get; set; }
        [StringLength(1000,
          ErrorMessageResourceType = typeof(ErrorMessages),
          ErrorMessageResourceName = nameof(ErrorMessages.StringLength))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.blh))]
        public string rwbyka { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Favicons))]
        [FileUpload(1000,".ico", false,false,null,null)]
        [UIHint("UploadFavicons")]
        public int? FaviconsId { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Logo))]
        [FileUpload(1000, ".jpg,.png,.jpeg", false,false,null,null)]
        [UIHint("UploadLogo")]
        public int? LogoId { get; set; }
        [HiddenInput]
        public string FaviconsUrl { get; set; }
        [HiddenInput]
        public string LogoUrl { get; set; }

    }

    public class ContactUSListViewModel : BaseViewModel
    {

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.FullName))]
        public string FullName { set; get; }
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Email))]
        public string EmailName { set; get; }
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        public string Title { set; get; }
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Description))]
        [DataType(dataType:DataType.MultilineText)]
        public string Description { get; set; }
    }
public class AboutStoreViewModel : BaseViewModel
    {
        [Required(AllowEmptyStrings = false,
              ErrorMessageResourceType = typeof(ErrorMessages),
              ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.About))]
        [DataType(dataType:DataType.Html)]
        public string AboutStore { get; set; }
        [Required(AllowEmptyStrings = false,
              ErrorMessageResourceType = typeof(ErrorMessages),
              ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(Name ="تماس با ما")]
        [DataType(dataType:DataType.Html)]
        public string ContactUS { get; set; }
        /// <summary>

    }
    public class AddressStoreViewModel : BaseViewModel
    {
        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Province))]
        [DropDownList(DataSourceUrl = UrlHelper.Url + "Province/list",AutoBind =true)]
        public int? ProvinceId { get; set; }
        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.City))]
        [DropDownList(DataSourceUrl = UrlHelper.Url + "City/ListAll",AutoBind =true, CascadeFrom = "ProvinceId", CascadeFromField = "ProvinceId", OptionLabel = "لطفا شهرستان را انتخاب کنید")]
        public int? CityId { get; set; }
        [Required(AllowEmptyStrings = false,
              ErrorMessageResourceType = typeof(ErrorMessages),
              ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Address))]
        [DataType(dataType:DataType.MultilineText)]
        public string Address { get; set; }
        /// <summary>

    }
    public class TransportationViewModel : BaseViewModel
    {


        [Display(Name ="هزینه حمل و نقل رایگان" )]
        public bool IsFreeSender { get; set; }
        /// <summary>
        /// پرداخت در محل
        /// </summary>
        [Display(Name ="پرداخت در محل" )]
        public bool IsPayInLocation { get; set; }
        /// <summary>
        /// هزینه حمل توسط کاربر 
        /// </summary>
        [Display(Name ="پرداخت حمل توسط کاربر" )]
        public bool IsPaybyUser { get; set; }

        //////هزینه حمل برای مقدار مورد نظر رایگان////
        [Display(Name ="شرط برای حمل رایگان بالای مقدار فاکتور مورد نظر" )]
        public long PriceFree { get; set; }


        /// <summary>
        /// محاسبه داخل شهر مورد نظر
        /// </summary>
        [Display(Name ="هزینه تقریبی برای شهرستان مقصد" )]
        public long PriceAsStore { get; set; }

        /// <summary>
        /// محاسبه داخل سایر شهرهای  مورد نظر
        /// </summary>
        [Display(Name ="هزینه تقریبی برای سایر استانها" )]
        public long PriceOtherProvince { get; set; }
        /// <summary>
        /// توضیحات حمل و نقل
        /// </summary>
        [Display(Name ="توضیحات کامل در مورد حمل و نقل" )]
        [DataType(dataType:DataType.Html)]
        public string Transportation { get; set; }



    }
}
