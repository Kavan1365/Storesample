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
    public class ProductViewModel : BaseViewModel
    {
        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        [StringLength(450, MinimumLength = 1)]
        public string Title { get; set; }
      
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.OrderBy))]
        public int OrderBy { get; set; }


    }
}
