using BaseCore.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Prodcutes
{
    [Table("ProductProperty", Schema = "PPP")]
    public class ProductProperty : BaseEntity
    {
        [MaxLength(10000)]
        public string Title { get; set; }

        public bool IsFilter { get; set; }

        public int ViewOrder { get; set; }

        public virtual ICollection<SubFilter> SubFilters { get; set; }
    }


}
