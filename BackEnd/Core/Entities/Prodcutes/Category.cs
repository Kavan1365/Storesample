using BaseCore.Core;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Core.Entities.Prodcutes
{
    [Table("Category", Schema = "PPP")]
    public class Category : BaseEntity
    {
        public int? ParentId { get; set; }
        [ForeignKey(nameof(ParentId))]
        public virtual Category Parent { get; set; }

        [IndexColumn(IsUnique = true)]
        public string Title { get; set; }

        [MaxLength(100)]
        public string FontIcon { get; set; }

        public bool IsShowHome { get; set; }
        public int Order { get; set; }

        public int? ImageId { get; set; }
        [ForeignKey(nameof(ImageId))]
        public Base.File Logo { get; set; }

        /////////////////////////// [<-- Relations -->] ///////////////////////////

        /* <-- Product In Category --> */
        public virtual ICollection<ProductsCategory> ProductsCategory { get; set; }

    }

}
