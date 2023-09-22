using BaseCore.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Core.Entities.Prodcutes
{
    [Table("Product", Schema = "PPP")]
    public class Product : BaseEntity
    {
        [IndexColumn(IsUnique = true)]
        public string Title { get; set; }

        public bool IsFirst { get; set; }
        public int OrderBy { get; set; }

        public virtual ICollection<ProductsCategory> Categories { get; set; }
        public virtual ICollection<ProductsFilter> Filters { get; set; }


        /* <-- Product Property --> */
        //  public     virtual ICollection<ProductProperty> productproperty { get; set; }
        public virtual ICollection<DefinedProduct> DefinedProducts { get; set; }
    }


}
