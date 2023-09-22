using BaseCore.Configuration;
using Core.Entities.Prodcutes;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace ViewModels.Prodcuts
{
    public class CategoryViewModelCreate : BaseDtoHierarchical<CategoryViewModelCreate, Category>
    {
        [Required(AllowEmptyStrings = false,
             ErrorMessageResourceType = typeof(ErrorMessages),
             ErrorMessageResourceName = nameof(ErrorMessages.Required))]
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        [MaxLength(1000)]
        public string Title { get; set; }

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.FontIcon))]
        [MaxLength(100)]
        public string FontIcon { get; set; }

        public bool IsShowHome { get; set; }
        public int Order { get; set; }


        public int? ImageId { get; set; }
        public string LogoUrl{ get; set; }

    }


    public class CategoryViewModel : BaseDtoHierarchical<CategoryViewModel, Category>
    {
        public string Title { get; set; }
        public int Order { get; set; }

        public string FontIcon { get; set; }
        public string imageurl { get; set; }
        public int? ParentId { get; set; }
        public bool IsShowHome { get; set; }
        public List<CategoryViewModel> Categories { get; set; }


    }
}
