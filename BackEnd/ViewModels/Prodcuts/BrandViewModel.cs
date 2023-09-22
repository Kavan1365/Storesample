using BaseCore.Configuration;
using Core.Entities.Prodcutes;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Prodcuts
{
    public class BrandViewModel : BaseDto<BrandViewModel, Brand>
    {
        [Required(AllowEmptyStrings = false,
           ErrorMessageResourceType = typeof(ErrorMessages),
           ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        [MaxLength(1000)]

        public string Title { get; set; }
        public bool IsSelected { get; set; }
        public int? ImageId { get; set; }
        public string ImageUrl { get; set; }


    }


}
