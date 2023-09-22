using BaseCore.Configuration;
using Core.Entities.Base;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Base
{

    public class SeoViewModelForClient : BaseDto<SeoViewModelForClient, Seo>
    {
        public string SeoUrl { get; set; }
        public List<SeoItemViewModel> SeoItems { get; set; }
    }
    public class SeoViewModel : BaseDto<SeoViewModel, Seo>
    {
        /// <summary>
        /// عنوان سایت
        /// </summary>
        [MaxLength(500,
              ErrorMessageResourceType = typeof(ErrorMessages),
              ErrorMessageResourceName = nameof(ErrorMessages.MaxLength))]
        [Required(AllowEmptyStrings = false,
              ErrorMessageResourceType = typeof(ErrorMessages),
              ErrorMessageResourceName = nameof(ErrorMessages.Required))]

        public string SeoUrl { get; set; }
    }
    public class MetadataValue : BaseDto<MetadataValue, SeoItem>
    {
        public TypeSeo TypeSeo { get; set; }

        public string Title { get; set; }
        public string NameMeta { get; set; }
        public string ContentMenta { get; set; }
        public string LinkSeo { get; set; }


    }

    public class SeoItemViewModel : BaseDto<SeoItemViewModel, SeoItem>
    {

        public int SeoId { get; set; }
        public TypeSeo TypeSeo { get; set; }

        public string Title { get; set; }
        public string Meta { get; set; }

        public string NameMeta { get; set; }
        public string ContentMenta { get; set; }

        public string LinkSeo { get; set; }
    }
}
