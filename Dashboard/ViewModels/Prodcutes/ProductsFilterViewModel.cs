using Resources;
using Services.Models;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModels.Prodcutes
{
    public class ProductsFilterViewModel : BaseViewModel
    {
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.ProductTitle))]
        public string ProductTitle{ get; set; }


        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.ProductPropertyTitle))]
        public string ProductPropertyTitle { get; set; }
    }
    public class ProductsCategoryViewModel : BaseViewModel
    {
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.ProductTitle))]
        public string ProductTitle{ get; set; }


        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Category))]
        public string CategoryTitle { get; set; }
    }
}
