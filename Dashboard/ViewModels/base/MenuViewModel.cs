using Microsoft.AspNetCore.Mvc;
using Services.Models;
using Resources;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Xml.Linq;

namespace Dashboard.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        public string Title { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Parent))]
        public string ParentTitle { get; set; }
        [HiddenInput]
        public int? ParentId { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.OrderBy))]
        public int? Order { get; set; }
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Url))]
        public string Address { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Role))]
        public string RoleTitle { get; set; }

    }
}
