using AutoMapper;
using BaseCore.Configuration;
using BaseCore.Helper.Validations;
using Core;
using Core.Entities.AAA;
using Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace ViewModels.AAA
{
    public class UserViewModelCreate : BaseDto<UserViewModelCreate, User>, IValidatableObject
    {
        [Required(AllowEmptyStrings = false,
              ErrorMessageResourceType = typeof(ErrorMessages),
              ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Sex))]

        public Sex Sex { get; set; }
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
        public int? ProvinceId { get; set; }
        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(ErrorMessages),
           ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.City))]
        public int? CityId { get; set; }
        [Required(AllowEmptyStrings = false,
          ErrorMessageResourceType = typeof(ErrorMessages),
         ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Role))]
        public string[]? RoleIds { get; set; }


        public UserType UserType { get; set; }
        public DateTime? BirthDay { get; set; }
        [NationalId(ErrorMessageResourceType = typeof(ErrorMessages),
          ErrorMessageResourceName = nameof(ErrorMessages.NationalIdValid))]
        public string NationalCode { get; set; }

        public string Address { get; set; }

        public string FullName { get; set; }



        public bool IsActive { get; set; }



        public DateTime? LastLogin { get; set; }


        public string FatherName { get; set; }
        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }



        public override void CustomMappings(IMappingExpression<User, UserViewModelCreate> mappingExpression)
        {
            //mappingExpression.ForMember(
            //        dest => dest.RoleIds,
            //        config => config.MapFrom(
            //            s => s.UserRoles.Count() > 0 ? s.UserRoles.ToList().Select(x => x.RoleId.ToString()
            //            ) : null));
            mappingExpression.ForMember(
                         dest => dest.ProvinceId,
                         config => config.MapFrom(s => s.City.ProvinceId));
        }

        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            string strRegex = @"^[0][9][0-9][0-9]{8,8}$";
            Regex re = new Regex(strRegex);
            if (!re.IsMatch(UserName))
                yield return new ValidationResult(ErrorMessages.Usernamemobilenumber, new[] { nameof(UserName) });

            if (UserType == UserType.User || UserType == UserType.Admin)
                yield return new ValidationResult(ErrorMessages.ErrorSelectusertype, new[] { nameof(UserName) });
        }
    }
}
