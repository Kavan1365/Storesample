using BaseCore.Core;
using Microsoft.EntityFrameworkCore;
using Resources;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Prodcutes
{
    public class ProductPrice:BaseEntity
    {
        public int DefinedProductId { get; set; }
        [ForeignKey("DefinedProductId")]
        public virtual DefinedProduct DefinedProduct { get; set; }


        public ProductPriceType ProductPriceType { get; set; }
        public long Price { get; set; }
        [Precision(18, 2)] 
        public decimal Discount { get; set; } ////تخفیف 
        public int Qty { get; set; }

        public int? ColorId { get; set; }
        [ForeignKey(nameof(ColorId))]
        public Color Color { get; set; }
        public DateTime? EndDateTime { get; set; }
        /// <summary>
        /// محاسبه قیمت اصلی با تخفیف فروشگاه
        /// </summary>
        public long DiscountPrice
        {
            get
            {
                if (Discount != 0)
                    return (long)(Discount * Price / 100);
                return 0;

            }
        }
        public long PriceMain
        {
            get
            {
                long tempPrice = Price;
                if (Discount > 0)
                    tempPrice = tempPrice - DiscountPrice;
                return tempPrice*Qty;
            }
        }





    }
}
