using BaseCore.Configuration;
using Core.Entities.Prodcutes;

namespace ViewModels.Prodcuts
{
    public class ProductsFilterViewModel : BaseDto<ProductsFilterViewModel, ProductsFilter>
    {
        public int ProductId { get; set; }
        public int ProductPropertyrId { get; set; }

        public string ProductPropertyTitle { get; set; }
        public string ProductTitle { get; set; }
    }
}
