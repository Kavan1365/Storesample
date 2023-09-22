using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Services.Models;
using Services.UI.Controls.KCore.DropDownList;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace Dashboard.ViewModels.Prodcutes
{
    public class ProductPriceViewModel : BaseViewModel
    {
        [Required(AllowEmptyStrings = false,
           ErrorMessageResourceType = typeof(ErrorMessages),
           ErrorMessageResourceName = nameof(ErrorMessages.Required))]

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        public string DefinedProductTitle { get; set; }
        [Display(Name = "قیمت اصلی به تومان")]
        public long Price { get; set; }
        [Display(Name = "درصد تخفیف")]
        public decimal Discount { get; set; } ////تخفیف 
        [Display(Name = "مقدار  تخفیف به تومان")]
        public long DiscountPrice { get; set; }

        [Display(Name = "تعداد")]
        public int Qty { get; set; }
        [Display(Name = "مجموع کل به تومان")]
        public long PriceMain { get; set; }
        [Display(Name = "رنگ")]
        public string ColorTitle { get; set; }
        [Display(Name = "تاریخ پایان")]
        public DateTime? EndDateTime { get; set; }
    }
    public class ProductPriceViewModelCreate : BaseViewModel
    {
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [DropDownList(DataSourceUrl = UrlHelper.Url + "DefinedProduct/list")]
        public int DefinedProductId { get; set; }
        [Display(Name = "نوع")]
        public ProductPriceType ProductPriceType { get; set; }
        [Display(Name = "قیمت اصلی")]
        public long Price { get; set; }
        [Display(Name = "درصد تخفیف")]
        public decimal Discount { get; set; } ////تخفیف 
        [Display(Name = "تعداد")]
        public int Qty { get; set; }
        [Required(AllowEmptyStrings = false,
             ErrorMessageResourceType = typeof(ErrorMessages),
             ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(Name = "رنگ")]
        [DropDownList(DataSourceUrl = UrlHelper.Url + "Color/list")]
        public int? ColorId { get; set; }
        [Display(Name = "تاریخ پایان")]
        [DataType(dataType: System.ComponentModel.DataAnnotations.DataType.DateTime)]
        public DateTime? EndDateTime { get; set; }
    }
}
