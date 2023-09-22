using BaseCore.Configuration;
using Core.Entities.Prodcutes;

namespace ViewModels.Prodcuts
{
    public class ProductsFilterViewModelCreate : BaseDto<ProductsFilterViewModelCreate, ProductsFilter>
    {
        public int ProductId { get; set; }
        public int ProductPropertyId { get; set; }
    }
}
