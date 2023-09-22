using BaseCore.Core;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.AAA
{
    /// <summary>
    /// ثبت نام کاربر در حال ارسال اس ام اس بعد از بررسی 
    /// وارد کلاس کاربر میشود و از اینجا حذف میشه
    /// </summary>
    public class UserTemp : BaseEntity
    {

        [MaxLength(12)]
        public string UserName { get; set; }
        [MaxLength(10)]
        public string Code { get; set; }


    }
}
