using BaseCore.Core;
using Core.Entities.Countries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    public class Setting : BaseEntity
    {
        /// <summary>
        /// عنوان سایت
        /// </summary>
        [MaxLength(500)]
        public string Title { get; set; } 
     
        [MaxLength(1000)]
        public string Url { get; set; } 
        /// <summary>
        /// لینک چک انلاین
        /// 
        /// </summary>
        [MaxLength(1000)]
        public string Boxchat { get; set; } 
        
        [MaxLength(1000)]
        /// <summary>
        /// لینک  youtube
        /// 
        /// </summary>
        public string linkedin { get; set; } 
        [MaxLength(1000)]
        public string youtube { get; set; } 
        [MaxLength(1000)]
        public string aparat { get; set; } 
        [MaxLength(1000)]
        public string whatsapp { get; set; } 
        [MaxLength(1000)]
        public string twitter { get; set; } 
        [MaxLength(1000)]
        public string instagram { get; set; } 
        [MaxLength(1000)]
        public string Telegram { get; set; } 
        [MaxLength(1000)]
        public string facebook { get; set; } 
        [MaxLength(1000)]
        public string srwsh { get; set; } 
        [MaxLength(1000)]
        public string ay_gp { get; set; } 
        [MaxLength(1000)]
        public string ayta { get; set; } 
        [MaxLength(1000)]
        public string blh { get; set; } 
        [MaxLength(1000)]
        public string rwbyka { get; set; } 


        public int? FaviconsId { get; set; }
        [ForeignKey(nameof(FaviconsId))]
        public File Favicons { get; set; }
        public int? LogoId { get; set; }
        [ForeignKey(nameof(LogoId))]
        public File Logo { get; set; }
        /// <summary>
        /// address
        /// </summary>

        public int? CityId { get; set; }
        [ForeignKey(nameof(CityId))]
        public City City { get; set; }
        [MaxLength(2000)]
        public string Address { get; set; }
        /// <summary>
        /// شامل اطلاعات فروشگاه و درباه فروشگاه ....
        /// </summary>
        [MaxLength(2000000)]
        public string AboutStore { get; set; }
        [MaxLength(2000000)]
        public string ContactUS { get; set; }

        ////////محاسبه حمل و نقل//////////
        /// <summary>
        /// if true=> یعنی حمل و نقل رایگان هست
        /// </summary>
        public bool IsFreeSender { get; set; }
        /// <summary>
        /// پرداخت در محل
        /// </summary>
        public bool IsPayInLocation { get; set; }
        /// <summary>
        /// هزینه حمل توسط کاربر 
        /// </summary>
        public bool IsPaybyUser { get; set; }

        //////هزینه حمل برای مقدار مورد نظر رایگان////
        public long PriceFree { get; set; }


        /// <summary>
        /// محاسبه داخل شهر مورد نظر
        /// </summary>
        public long PriceAsStore { get; set; }

        /// <summary>
        /// محاسبه داخل سایر شهرهای  مورد نظر
        /// </summary>
        public long PriceOtherProvince { get; set; }
        /// <summary>
        /// توضیحات حمل و نقل
        /// </summary>
        [MaxLength(2000000)]
        public string Transportation { get; set; }









    }
}
