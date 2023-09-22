using AutoMapper;
using BaseCore.Configuration;
using Core.Entities.Countries;
using Microsoft.AspNetCore.Mvc;
using Resources;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.Base
{
    public class ProvinceViewModel : BaseDto<ProvinceViewModel, Province>
    {
        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.Required))]

        public string Title { get; set; }
        public string Code { get; set; }

    }
    
    public class CityViewModel : BaseDto<CityViewModel, City>
    {
        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.Required))]

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        public string Title { get; set; }
        [HiddenInput]
        public int ProvinceId { get; set; }
        public int Province { get; set; }
        public override void CustomMappings(IMappingExpression<City, CityViewModel> mapping)
        {
            mapping
            .ForMember(
                   dest => dest.Province,
                   config => config.MapFrom(z => z.ProvinceId));
        }


    }
}
