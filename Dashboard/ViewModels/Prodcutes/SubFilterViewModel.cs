using Microsoft.AspNetCore.Mvc;
using Resources;
using Services.Models;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModels.Prodcutes
{
    public class SubFilterViewModel : BaseViewModel
    {
        [HiddenInput]
        public int ProductPropertyId { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        public string Title { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.OrderBy))]
        public int ViewOrder { get; set; }

   
    }
}
