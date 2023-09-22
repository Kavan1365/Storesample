using BaseCore.Configuration;
using Core.Entities.Prodcutes;

namespace ViewModels.Prodcuts
{
    public class ProductsFilterTitleViewModel : BaseDto<ProductsFilterTitleViewModel, ProductsFilter>
    {
        public string ProductPropertyTitle { get; set; }
    }
}
