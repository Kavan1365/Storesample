using BaseCore.Configuration;
using Core.Entities.Prodcutes;
using Resources;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.Prodcuts
{
    public class ProdcutViewModel : BaseDto<ProdcutViewModel, Product>
    {
        [Required(AllowEmptyStrings = false,
             ErrorMessageResourceType = typeof(ErrorMessages),
             ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        [MaxLength(1000)]
        public string Title { get; set; }

        public bool IsFirst { get; set; }
        public int OrderBy { get; set; }

    }
}
