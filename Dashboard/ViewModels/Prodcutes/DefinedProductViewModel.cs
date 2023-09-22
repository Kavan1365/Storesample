using Microsoft.AspNetCore.Mvc;
using Resources;
using Services.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.ViewModels.Prodcutes
{


    public class DefinedProductPriceViewModel : BaseViewModel
    {
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        public string DefinedProductTitle { get; set; }
        [Display(Name = "قیمت اصلی به تومان")]
        public long Price { get; set; }
        [Display(Name ="تخفیف")]
        public decimal Discount { get; set; } ////تخفیف 
        [Display(Name ="قیمت نهایی به تومان")]
        public long PriceMain { get; set; }
        [Display(Name ="تخفیف")]
        public int Qty { get; set; }
        public int? ColorId { get; set; }
        [Display(Name = "عنوان رنگ")]

        public string ColorTitle { get; set; }
        [Display(Name = "عنوان رنگ")]
        public DateTime? EndDateTime { get; set; }

    }
    public class DefinedProductPriceViewModelCreate : BaseViewModel
    {
        public int DefinedProductId { get; set; }
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        public string DefinedProductTitle { get; set; }
        public ProductPriceType ProductPriceType { get; set; }
        public long Price { get; set; }
        public long DiscountPrice { get; set; }
        public long PriceMain { get; set; }
        public decimal Discount { get; set; } ////تخفیف 
        public int Qty { get; set; }
        public int? ColorId { get; set; }
        public string ColorTitle { get; set; }
        public DateTime? EndDateTime { get; set; }

    }
    public class DefinedProductViewModel : BaseViewModel
    {
         [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        [MaxLength(1000)]
        public string Title { get; set; }
       
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.ProductTitle))]
        public string ProductTitle { get; set; }


        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Status))]
        public StatusNames Status { get; set; }
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.BrandTitle))]
        public string BrandTitle { get; set; }


        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.RateCount))]
        public int? Rate { get; set; }//لایک
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Visitcount))]
        public int? Visit { get; set; } //بازدید

    }
    public class DefinedProductViewModelCreate : BaseViewModel
    {
        [Required(AllowEmptyStrings = false,
           ErrorMessageResourceType = typeof(ErrorMessages),
           ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        [MaxLength(1000)]
        public string Title { get; set; }
        public int ProductId { get; set; }

        [Required(AllowEmptyStrings = false,
          ErrorMessageResourceType = typeof(ErrorMessages),
          ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [DataType(dataType:DataType.Html)]
        [MaxLength(100000)]
        public string Description { get; set; }

        [MaxLength(1000)]
        [DataType(dataType:DataType.MultilineText)]
        public string KeyWords { get; set; }

        public int? BrandId { get; set; }
      // public List<int?> Filters { get; set; }

       // public List<ProductPropertyWithFiltersViewModel> PropertyWithFilter { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Image))]
        [FileUpload(1000, ".jpg,.png,.jpeg", false, false, null, null)]
        [DataType(dataType: System.ComponentModel.DataAnnotations.DataType.Upload)]
        public int? ImageId { get; set; }


    }


    public class DefinedProductViewModelEdit : BaseViewModel
    {
        [Required(AllowEmptyStrings = false,
           ErrorMessageResourceType = typeof(ErrorMessages),
           ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        [MaxLength(1000)]
        public string Title { get; set; }
        public int ProductId { get; set; }

        [Required(AllowEmptyStrings = false,
          ErrorMessageResourceType = typeof(ErrorMessages),
          ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [DataType(dataType: DataType.Html)]
        [MaxLength(100000)]
        public string Description { get; set; }

        [MaxLength(1000)]
        [DataType(dataType: DataType.MultilineText)]
        public string KeyWords { get; set; }

        public int? BrandId { get; set; }
     


    }


    public class ProductPropertyWithFiltersViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<int> Selected { get; set; }

        public List<SelectItem> Filters { get; set; }
    }



    public class DefinedProductImagesViewModelList :BaseViewModel
    {
        [Display(Name = "کاور")]
        public bool IsCover { get; set; }
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Image))]
        public string ImageUrl { get; set; }
    }
    public class DefinedProductImagesViewModel : BaseViewModel
    {
        [HiddenInput]
        public int DefinedProductId { get; set; }
        [Display(Name = "کاور")]
        public bool IsCover { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Image))]
        [FileUpload(1000, ".jpg,.png,.jpeg", false, false, null, null)]
        [DataType(dataType: System.ComponentModel.DataAnnotations.DataType.Upload)]
        public int? ImageId { get; set; }
    }
}
