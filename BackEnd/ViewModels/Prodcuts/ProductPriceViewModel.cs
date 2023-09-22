using BaseCore.Configuration;
using Core.Entities.Prodcutes;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Prodcuts
{
    public class ProductPriceViewModel : BaseDto<ProductPriceViewModel, ProductPrice>
    {
        public int DefinedProductId { get; set; }
        public string DefinedProductTitle { get; set; }
        public ProductPriceType ProductPriceType { get; set; }
        public long Price { get; set; }
        public decimal Discount { get; set; } ////تخفیف 
        public int Qty { get; set; }
        public int? ColorId { get; set; }
        public string ColorTitle { get; set; }
        public DateTime? EndDateTime { get; set; }
        public long DiscountPrice { get; set; }
        public long PriceMain { get; set; }
        public string ImageUrl { get; set; }

    }
}
