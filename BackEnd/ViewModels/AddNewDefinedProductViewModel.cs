using BaseCore.Configuration;
using Core.Entities.Prodcutes;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Prodcuts
{
    public class DefinedProductViewModel : BaseDto<DefinedProductViewModel, DefinedProduct>
    {
        public string Title { get; set; }

        public string ProductTitle { get; set; }


        public StatusNames Status { get; set; }
        public string BrandTitle { get; set; }


        public int? Rate { get; set; }//لایک
        public int? Visit { get; set; } //بازدید

    }
    public class AddNewDefinedProductViewModel : BaseDto<AddNewDefinedProductViewModel, DefinedProduct>
    {
        [Required]
        public string Title { get; set; }
        [Required]

        public int ProductId { get; set; }
        [Required]

        public string KeyWords { get; set; }
        [Required]

        public string Description { get; set; }

        public List<int> Filters { get; set; }

        public int? BrandId { get; set; }


        public string Code { get; set; }

        public int? FileId { get; set; }

    }
    public class DefinedProductImagesViewModelList : BaseDto<DefinedProductImagesViewModelList, DefinedProductImage>
    {
        public bool IsCover { get; set; }
        public string ImageUrl { get; set; }
    }
    public class DefinedProductImagesViewModel : BaseDto<DefinedProductImagesViewModel, DefinedProductImage>
    {
        public int DefinedProductId { get; set; }
        public bool IsCover { get; set; }
        public int ImageId { get; set; }
    }
}
