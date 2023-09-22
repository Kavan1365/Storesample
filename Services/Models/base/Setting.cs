using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.@base
{
    public class Setting : BaseViewModel
    {
        /// <summary>
        /// عنوان سایت
        /// </summary>
        [Display(Name = "عنوان سایت")]
        [MaxLength(500)]
        public string Title { get; set; }
        /// <summary>
        /// توضیحات سایت
        /// </summary>
        [Display(Name = "توضیحات سایت")]
        [MaxLength(10000)]
        public string Description { get; set; }

        /// <summary>
        /// seo
        /// 
        /// </summary>
        [MaxLength(10000)]
        [Display(Name = "سئو")]
        public string Keywords { get; set; }
        /// <summary>
        /// seo
        /// 
        /// </summary>
        [Display(Name = "توضیحات سئو")]
        [MaxLength(10000)]
        public string DescriptionSeo { get; set; }
        /// <summary>
        /// seo
        /// 
        /// </summary>
        [MaxLength(500)]
        [Display(Name = "نوع سئو")]
        public string TypeSeo { get; set; }
        /// <summary>
        /// seo
        /// 
        /// </summary>
        [MaxLength(1000)]
        [Display(Name = "آدرس سایت")]
        public string Url { get; set; }

        /// <summary>
        /// لینک چک انلاین
        /// 
        /// </summary>
        [MaxLength(1000)]
        [Display(Name = "لینک چک انلاین")]
        public string Boxchat { get; set; }

        [Display(Name = "آیکون سایت")]
        public FileViewModel Favicons { get; set; }
        [Display(Name = "لوگوی سایت")]
        public FileViewModel Logo { get; set; }
    }
}
