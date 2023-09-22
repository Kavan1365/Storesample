using BaseCore.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Prodcutes
{
    public class CustomerReview : BaseEntity
    {
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string Disc { get; set; }
        public int Rate { get; set; }
        public int DefinedProductId { get; set; }
        [ForeignKey(nameof(DefinedProductId))]
        public virtual DefinedProduct DefinedProduct { get; set; }

        public bool IsLike { get; set; }

    }


}
