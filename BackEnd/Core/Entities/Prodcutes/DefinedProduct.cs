using BaseCore.Core;
using Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Core.Entities.Prodcutes
{
    [Table("DefinedProduct", Schema = "PPP")]
    public class DefinedProduct : BaseEntity
    {
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }

        [IndexColumn(IsUnique = true)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Code { get; set; }

        [MaxLength(100000)]
        public string Description { get; set; }

        [MaxLength(1000)]
        public string KeyWords { get; set; }

        public StatusNames Status { get; set; }

        public int? BrandId { get; set; }
        [ForeignKey(nameof(BrandId))]
        public virtual Brand Brand { get; set; }
        public int? Weight { get; set; }
        public bool IsFirst { get; set; }
        public int? Rate { get; set; }//لایک
        public int? Visit { get; set; } //بازدید
        //////////////////////////////////////////////////

        public virtual ICollection<ProductPropertyValue> ProductPropertyValues { get; set; }
        public virtual ICollection<DefinedProductImage> DefinedProductImages { get; set; }

    }


}
