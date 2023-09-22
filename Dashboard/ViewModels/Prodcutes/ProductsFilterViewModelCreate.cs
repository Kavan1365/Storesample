using Resources;
using Services.Models;
using Services.UI.Controls.KCore.DropDownList;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModels.Prodcutes
{
    public class ProductsFilterViewModelCreate : BaseViewModel
    {
        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.SelectProduct))] 
        [DropDownList(DataSourceUrl = UrlHelper.Url + "Product/list")]
        public int ProductId { get; set; }
        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.SelectProperty))]
        [DropDownList(DataSourceUrl =UrlHelper.Url+ "ProductProperty/list")]
        public int ProductPropertyId { get; set; }
    }
}
