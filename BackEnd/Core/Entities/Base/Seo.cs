using BaseCore.Core;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Base
{
    public class Seo : BaseEntity
    {
        [MaxLength(100)]
        public string SeoUrl { get; set; }

        public virtual ICollection<SeoItem> SeoItems { get; set; }

    }
}
