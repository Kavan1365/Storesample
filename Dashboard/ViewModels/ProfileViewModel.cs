using Services.Models;
using Resources;
using Services.UI.Controls.KCore.DropDownList;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        [Required(AllowEmptyStrings = false,
             ErrorMessageResourceType = typeof(ErrorMessages),
             ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Sex))]
        public Sex Sex { get; set; }

        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.Required))]

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.FirstName))]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.Required))]

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.LastName))]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.Required))]

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.UserName))]

        public string UserName { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.NationalCode))]
        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        public string NationalCode { get; set; }

        [Required(AllowEmptyStrings = false,
        ErrorMessageResourceType = typeof(ErrorMessages),
        ErrorMessageResourceName = nameof(ErrorMessages.Required))]

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.FatherName))]
        public string FatherName { get; set; }



        [Required(AllowEmptyStrings = false,
          ErrorMessageResourceType = typeof(ErrorMessages),
          ErrorMessageResourceName = nameof(ErrorMessages.Required))]

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Mobile))]
        public string Mobile { get; set; }

        [Required(AllowEmptyStrings = false,
          ErrorMessageResourceType = typeof(ErrorMessages),
          ErrorMessageResourceName = nameof(ErrorMessages.Required))]

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.CertificateId))]
        public string CertificateId { get; set; }


        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Province))]
        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.Required))]

        [DropDownList(LocalSourceFieldName = "ProvinceList", AutoBind = true)]
        // [ModelShow(true, "ProvinceAdd()")]
        public int ProvinceId { get; set; }
        public IEnumerable ProvinceList { get; set; }


        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.City))]
        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.Required))]

        [DropDownList(LocalSourceFieldName = "CityList", CascadeFrom = "ProvinceId", CascadeFromField = "Province", OptionLabel = "لطفا شهرستان را انتخاب کنید")]
        public int CityId { get; set; }
        public IEnumerable CityList { get; set; }

        [Required(AllowEmptyStrings = false,
      ErrorMessageResourceType = typeof(ErrorMessages),
      ErrorMessageResourceName = nameof(ErrorMessages.Required))]

        [Display(Name = "کد پستی")]
        public string ZipCode { get; set; }
        [Display(Name = " تلفن ثابت ضروری")]
        public string Phone { get; set; }
        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.BirthDay))]
        public DateTime? BirthDay { get; set; }






        [Required(AllowEmptyStrings = false,
             ErrorMessageResourceType = typeof(ErrorMessages),
             ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [DataType(dataType: DataType.MultilineText)]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Address))]
        public string Address { get; set; }



        [Display(Name = "عکس پروفایل")]
        [FileUpload(1000, ".jpg,.png,.jpeg", false, false, null, null)]
        [DataType(dataType: DataType.Upload)]

        public int? ImageId { get; set; }
        [FileUpload(1000, ".jpg,.png,.jpeg", false, false, null, null)]
        [DataType(dataType: DataType.Upload)]
        public string ImageUrl { get; set; }




    }
}
