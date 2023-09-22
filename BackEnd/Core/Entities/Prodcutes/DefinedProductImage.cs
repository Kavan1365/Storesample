using BaseCore.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Prodcutes
{
    [Table("DefinedProductImage", Schema = "PPP")]
    public class DefinedProductImage : BaseEntity
    {
        public bool IsCover { get; set; }
        public int DefinedProductId { get; set; }
        [ForeignKey(nameof(DefinedProductId))]
        public virtual DefinedProduct DefinedProduct { get; set; }

        public int ImageId { get; set; }
        [ForeignKey(nameof(ImageId))]
        public virtual Base.File Image { get; set; }

    }


}
