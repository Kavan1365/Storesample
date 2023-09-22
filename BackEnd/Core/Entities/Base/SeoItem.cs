using BaseCore.Core;
using Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    public class SeoItem : BaseEntity
    {
        public int SeoId { get; set; }
        [ForeignKey(nameof(SeoId))]
        public virtual Seo Seo { get; set; }
        public TypeSeo TypeSeo { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }
       [MaxLength(100)]
        public string NameMeta { get; set; }
        [MaxLength(1000)]
        public string ContentMenta { get; set; }

        [MaxLength(1000)]
        public string LinkSeo { get; set; }


    }
}
