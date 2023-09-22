using Resources;
using Services.Models;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModels.Prodcutes
{
    public class ProductPropertyViewModelCreate : BaseViewModel
    {
        [Required(AllowEmptyStrings = false,
             ErrorMessageResourceType = typeof(ErrorMessages),
             ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        public string Title { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.IsFilter))]
        public bool IsFilter { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.OrderBy))]
        public int ViewOrder { get; set; }
    }
    public class UpdateProductPropertyViewModel
    {
        public List<ProductPropertyViewModel> models { get; set; }
    }
    public class ProductPropertyViewModel :BaseViewModel
    {
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        public string Title { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.IsFilter))]
        public bool IsFilter { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.OrderBy))]
        public int ViewOrder { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.SubFilterCount))]
        public int SubFilterCount { get; set; }
    }
}
