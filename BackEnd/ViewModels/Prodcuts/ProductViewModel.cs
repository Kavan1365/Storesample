using AutoMapper;
using BaseCore.Configuration;
using Core.Entities.Prodcutes;

namespace ViewModels.Prodcuts
{
    public class ProductViewModel : BaseDto<ProductViewModel, Product>
    {
        public string Title { get; set; }

        public bool IsFirst { get; set; }

        public int OrderBy { get; set; }

        public int CategoryCount { get; set; }
        public int DefinedCount { get; set; }
        public int PropertyCount { get; set; }

        public override void CustomMappings(IMappingExpression<Product, ProductViewModel> mapping)
        {
            mapping
                .ForMember(
                    dest => dest.CategoryCount,
                    config => config.MapFrom(s => s.Categories.Count))
                .ForMember(
                    dest => dest.DefinedCount,
                    config => config.MapFrom(s => s.DefinedProducts.Count))
                .ForMember(
                    dest => dest.PropertyCount,
                    config => config.MapFrom(s => s.Filters.Count));
        }
    }
}
