using BaseCore.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Prodcutes
{
    [Table("ProductsFilter", Schema = "PPP")]
    public class ProductsFilter : BaseEntity
    {
        public int ProductId { get; set; }
        public int ProductPropertyId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }
        [ForeignKey(nameof(ProductPropertyId))]
        public virtual ProductProperty ProductProperty { get; set; }
        
    }


}
