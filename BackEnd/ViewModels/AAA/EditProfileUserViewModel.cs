using BaseCore.Configuration;
using BaseCore.Core.ViewModel;
using Core.Entities.AAA;
using System.ComponentModel.DataAnnotations;
using ViewModels.Validations;

namespace ViewModels.AAA
{
    public class EditProfileUserViewModel : BaseDto<EditProfileUserViewModel, User>
    {
        [Display(Name = "عکس پروفایل")]
        [UIHint("ImageUpload")]
        [FileUpload(maxSize: 1000, fileExtentions: ".jpg,.png,.jpeg", ErrorMessage = "تصویر ارسالی تنها با فرمت های jpg,png,jpeg  قابل ارسال هستند و حداکثر حجم مجاز برای هر فایل 1 مگابایت است")]
        public FileViewModel Image { get; set; }
    }
}
