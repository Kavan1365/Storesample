using AutoMapper;
using BaseCore.Configuration;
using Core.Entities.AAA;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ViewModels.Validations;

namespace ViewModels.AAA
{
    public class UsualUserInfoViewModel : BaseDto<UsualUserInfoViewModel, User>
    {
        [DisplayName("نام ونام خانوداگی")]
        public string FullName { get; set; }

        [DisplayName("نام کاربری")]
        public string UserName { get; set; }
        [DisplayName("موبایل")]
        public string Mobile { get; set; }
        public string CityProvinceTitle { get; set; }
        public string CityTitle { get; set; }

        [DisplayName("ایمیل")]
        public string Email { get; set; }

        [DisplayName("عکس")]
        public FileViewModel Image { get; set; }

        [DisplayName("وضعیت")]

        public bool Disabled { get; set; }

        [DisplayName("اخرین لاگین")]
        public DateTime? LastLogin { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime? Created { get; set; }

        public string Title { get; set; }

        public override void CustomMappings(IMappingExpression<User, UsualUserInfoViewModel> mappingExpression)
        {
            mappingExpression.ForMember(
                 dest => dest.Title,
                 config => config.MapFrom(s => $"{s.UserName}({s.FullName})"));

        }

    }
}
