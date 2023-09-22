using AutoMapper;
using BaseCore.Configuration;
using Core.Entities.AAA;
using Resources;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.AAA
{
    public class UserViewModelCreateClient : BaseDto<UserViewModelCreateClient, User>
    {

        public string Phone { get; set; }
        public string OldPassword { get; set; }
        public string UserName { get; set; }

        public string AgentTitle { get; set; }
        public string CertificateId { get; set; }


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
        public string Mobile { get; set; }

        [Required(AllowEmptyStrings = false,
        ErrorMessageResourceType = typeof(ErrorMessages),
        ErrorMessageResourceName = nameof(ErrorMessages.Required))]

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.FatherName))]
        public string FatherName { get; set; }

        public string ZipCode { get; set; }






        // [Required(AllowEmptyStrings = false,
        //ErrorMessageResourceType = typeof(ErrorMessages),
        //ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        // [Display(Name = "نام سازمان / اداره")]
        // public int? OfficeId { get; set; }
        [Required(AllowEmptyStrings = false,
          ErrorMessageResourceType = typeof(ErrorMessages),
          ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(Name = "کد ملی")]
        public string NationalCode { get; set; }

        [HiddenInput]
        public int? InstallmentId { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public int? ProvinceId { get; set; }

        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(ErrorMessages),
           ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.City))]
        public int? CityId { get; set; }
        public DateTime? BirthDay { get; set; }
     
        [DataType(dataType: DataType.MultilineText)]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Address))]
        public string Address { get; set; }




        //   public FileViewModel NationalCodeImage { get; set; } = new FileViewModel();

        //public FileViewModel BackNationalCodeImage { get; set; } = new FileViewModel();

        //public FileViewModel CertificateImage { get; set; } = new FileViewModel();


        public override void CustomMappings(IMappingExpression<User, UserViewModelCreateClient> mappingExpression)
        {
            mappingExpression.ForMember(
                         dest => dest.ProvinceId,
                         config => config.MapFrom(s => s.City.ProvinceId));
        }


    }
}
