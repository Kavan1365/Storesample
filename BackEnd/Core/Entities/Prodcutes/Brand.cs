using BaseCore.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Core.Entities.Prodcutes
{
    [Table("Brand", Schema = "PPP")]
    public class Brand : BaseEntity
    {
        [IndexColumn(IsUnique = true)]
        public string Title { get; set; }

        public int? ImageId { get; set; }
        [ForeignKey(nameof(ImageId))]
        public virtual Base.File Image { get; set; }

        public virtual ICollection<DefinedProduct> DefinedProducts { get; set; }
    }

}
