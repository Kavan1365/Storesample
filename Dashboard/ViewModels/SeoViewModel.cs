using Microsoft.AspNetCore.Mvc;
using Resources;
using Services.Models;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModels
{
    public class SeoViewModel :BaseViewModel
    {
        /// <summary>
        /// عنوان سایت
        /// </summary>
        [MaxLength(500,
              ErrorMessageResourceType = typeof(ErrorMessages),
              ErrorMessageResourceName = nameof(ErrorMessages.MaxLength))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.SeoUrl))]
        public string SeoUrl { get; set; }
    }

    public class SeoItemViewModel : BaseViewModel
    {

      [HiddenInput]
        public int SeoId { get; set; }
           [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.TypeSeo))]
        public TypeSeo TypeSeo { get; set; }

           [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        public string Title { get; set; }
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.NameMeta))]
        public string NameMeta { get; set; }
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.ContentMenta))]
        public string ContentMenta { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.LinkSeo))]
        public string LinkSeo { get; set; }
    }
}
