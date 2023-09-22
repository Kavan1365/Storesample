using BaseCore.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Prodcutes
{
    [Table("SubFilter", Schema = "PPP")]
    public class SubFilter : BaseEntity
    {
        public int ProductPropertyId { get; set; }
        [ForeignKey(nameof(ProductPropertyId))]
        public virtual ProductProperty ProductProperty { get; set; }

        [MaxLength(10000)]
        public string Title { get; set; }

        public int ViewOrder { get; set; }

    }


}
