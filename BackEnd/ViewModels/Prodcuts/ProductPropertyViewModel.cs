using AutoMapper;
using BaseCore.Configuration;
using Core.Entities.Prodcutes;
using Resources;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.Prodcuts
{
    public class ProductPropertyViewModel : BaseDto<ProductPropertyViewModel, ProductProperty>
    {
        public string Title { get; set; }

        public bool IsFilter { get; set; }

        public int ViewOrder { get; set; }

        public int SubFilterCount { get; set; }
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.ProductsCount))]
        public int ProductsCount { get; set; }
        public override void CustomMappings(IMappingExpression<ProductProperty, ProductPropertyViewModel> mapping)
        {
            mapping
                .ForMember(
                    dest => dest.SubFilterCount,
                    config => config.MapFrom(s => s.SubFilters.Count))
               
                ;
        }
    }


    public class ProductPropertyViewModelCreate : BaseDto<ProductPropertyViewModelCreate, ProductProperty>
    {
        public string Title { get; set; }

        public bool IsFilter { get; set; }

        public int ViewOrder { get; set; }
    }
}
