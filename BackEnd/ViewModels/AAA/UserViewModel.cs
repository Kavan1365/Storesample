using BaseCore.Configuration;
using Core.Entities.AAA;
using Resources;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.AAA
{
    public class UserViewModel : BaseDto<UserViewModel, User>
    {


        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.FullName))]
        public string FullName { get; set; }
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.UserName))]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.NationalCode))]
        public string NationalCode { get; set; }
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Province))]
        public string CityProvinceTitle { get; set; }
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.City))]
        public string CityTitle { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.CreatedFullName))]
        public string CreatedFullName { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.CreatedUserName))]
        public string CreatedUserName { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Created))]
        public DateTime? Created { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.LastLoginDate))]
        public DateTime? LastLogin { get; set; }


        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Image))]
        public string ImageUrl { get; set; }

        //public override void CustomMappings(IMappingExpression<User, UserViewModel> mappingExpression)
        //{
        //    mappingExpression.ForMember(
        //            dest => dest.CreatedUserName,
        //            config => config.MapFrom(s => (s.CreatedByUserId.HasValue)? _userRepository.GetById(s.CreatedByUserId).UserName:""))
        //                   .ForMember(
        //            dest => dest.CreatedFullName,
        //            config => config.MapFrom(s => (s.CreatedByUserId.HasValue) ? _userRepository.GetById(s.CreatedByUserId).FullName : ""));
        //}


    }
}
