using BaseCore.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Prodcutes
{

    [Table("ProductPropertyValue", Schema = "PPP")]
    public class ProductPropertyValue : BaseEntity
    {
        public int SubFilterId { get; set; }

        [ForeignKey(nameof(SubFilterId))]
        public virtual SubFilter SubFilter { get; set; }


        public int DefinedProductId { get; set; }
        [ForeignKey(nameof(DefinedProductId))]
        public virtual DefinedProduct DefinedProduct { get; set; }

    }


}
