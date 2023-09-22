using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Services.Models;
using Services.UI.Controls.KCore.DropDownList;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace Dashboard.ViewModels.Prodcutes
{
    public class CategoryViewModel : BaseHierarchicalViewModel
    {
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        public string Title { get; set; }


        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.FontIcon))]
        public string FontIcon { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.OrderBy))]
        public int Order { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.IsShowHome))]

        public bool IsShowHome { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Image))]
        public string LogoUrl { get; set; }



    }

    public class CategoryViewModelCreate : BaseViewModel
    {
        [Required(AllowEmptyStrings = false,
           ErrorMessageResourceType = typeof(ErrorMessages),
           ErrorMessageResourceName = nameof(ErrorMessages.Required))]

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        [StringLength(450, MinimumLength = 1)]
        public string Title { get; set; }


        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.FontIcon))]
        [StringLength(50, MinimumLength = 0)]
        [DropDownList(LocalSourceFieldName = "FontIconList")]
        public string FontIcon { get; set; }
        public IEnumerable FontIconList { get; set; }
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.OrderBy))]
        public int Order { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.IsShowHome))]
        public bool IsShowHome { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Image))]
        [FileUpload(1000, ".jpg,.png,.jpeg", false, false, null, null)]
        [DataType(dataType: System.ComponentModel.DataAnnotations.DataType.Upload)]
        public int? ImageId { get; set; }

        [HiddenInput]
        public int? ParentId { get; set; }



    }
}
