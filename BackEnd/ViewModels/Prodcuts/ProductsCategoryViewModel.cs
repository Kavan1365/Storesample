using BaseCore.Configuration;
using Core.Entities.Prodcutes;

namespace ViewModels.Prodcuts
{
    public class ProductsCategoryViewModel : BaseDto<ProductsCategoryViewModel, ProductsCategory>
    {
        public int CategoryId { get; set; }
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string CategoryTitle { get; set; }
    }
}
