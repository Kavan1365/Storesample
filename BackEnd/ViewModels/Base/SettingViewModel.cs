using BaseCore.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.Configuration;
using ViewModels.AAA;
using Core.Entities.Base;
using BaseCore.Core.ViewModel;
using Resources;
using Newtonsoft.Json;
using ViewModels.Validations;
using Microsoft.AspNetCore.Mvc;

namespace ViewModels
{


    public class SettingViewModel : BaseDto<SettingViewModel, Setting>
    {
        /// <summary>
        /// عنوان سایت
        /// </summary>
        [MaxLength(500,
              ErrorMessageResourceType = typeof(ErrorMessages),
              ErrorMessageResourceName = nameof(ErrorMessages.MaxLength))]
        [Required(AllowEmptyStrings = false,
              ErrorMessageResourceType = typeof(ErrorMessages),
              ErrorMessageResourceName = nameof(ErrorMessages.Required))]

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        public string Title { get; set; }

        /// <summary>
        /// لینک چک انلاین
        /// 
        /// </summary>
        [MaxLength(1000,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.MaxLength))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Boxchat))]
        public string Boxchat { get; set; }

        [MaxLength(1000,
               ErrorMessageResourceType = typeof(ErrorMessages),
               ErrorMessageResourceName = nameof(ErrorMessages.MaxLength))]
        public string linkedin { get; set; }
        [MaxLength(1000,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.MaxLength))]
        public string youtube { get; set; }
        [MaxLength(1000,
           ErrorMessageResourceType = typeof(ErrorMessages),
           ErrorMessageResourceName = nameof(ErrorMessages.MaxLength))]
        public string aparat { get; set; }
        [MaxLength(1000,
          ErrorMessageResourceType = typeof(ErrorMessages),
          ErrorMessageResourceName = nameof(ErrorMessages.MaxLength))]
        public string whatsapp { get; set; }
        [MaxLength(1000,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.MaxLength))]
        public string twitter { get; set; }
        [MaxLength(1000,
           ErrorMessageResourceType = typeof(ErrorMessages),
           ErrorMessageResourceName = nameof(ErrorMessages.MaxLength))]
        public string instagram { get; set; }
        [MaxLength(1000,
          ErrorMessageResourceType = typeof(ErrorMessages),
          ErrorMessageResourceName = nameof(ErrorMessages.MaxLength))]
        public string Telegram { get; set; }
        [MaxLength(1000,
          ErrorMessageResourceType = typeof(ErrorMessages),
          ErrorMessageResourceName = nameof(ErrorMessages.MaxLength))]
        public string facebook { get; set; }
        [MaxLength(1000,
          ErrorMessageResourceType = typeof(ErrorMessages),
          ErrorMessageResourceName = nameof(ErrorMessages.MaxLength))]
        public string srwsh { get; set; }
        [MaxLength(1000,
           ErrorMessageResourceType = typeof(ErrorMessages),
           ErrorMessageResourceName = nameof(ErrorMessages.MaxLength))]
        public string ay_gp { get; set; }
        [MaxLength(1000,
           ErrorMessageResourceType = typeof(ErrorMessages),
           ErrorMessageResourceName = nameof(ErrorMessages.MaxLength))]
        public string ayta { get; set; }
        [MaxLength(1000,
           ErrorMessageResourceType = typeof(ErrorMessages),
           ErrorMessageResourceName = nameof(ErrorMessages.MaxLength))]
        public string blh { get; set; }
        [MaxLength(1000,
          ErrorMessageResourceType = typeof(ErrorMessages),
          ErrorMessageResourceName = nameof(ErrorMessages.MaxLength))]
        public string rwbyka { get; set; }
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Favicons))]
        public int? FaviconsId { get; set; }
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Logo))]
        public int? LogoId { get; set; }

        [HiddenInput]
        public string FaviconsUrl { get; set; }
        [HiddenInput]
        public string LogoUrl { get; set; }
    }
    public class SettingContactUSViewModel : BaseDto<SettingContactUSViewModel, Setting>
    {
        public string ContactUS { get; set; }

    }
    public class AboutStoreViewModel : BaseDto<AboutStoreViewModel, Setting>
    {
        public string ContactUS { get; set; }
        public string AboutStore { get; set; }

    }
    public class AddressStoreViewModel : BaseDto<AddressStoreViewModel, Setting>
    {
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public string Address { get; set; }
        /// <summary>

    }
    public class TransportationViewModel : BaseDto<TransportationViewModel, Setting>
    {
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