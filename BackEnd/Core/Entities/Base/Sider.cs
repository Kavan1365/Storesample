using BaseCore.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base
{
    public class Sider : BaseEntity
    {

        [MaxLength(10000)]
        public string UrlLink { get; set; }
        public int ImageId { get; set; }
        [ForeignKey(nameof(ImageId))]
        public File Image { get; set; }
    }
}
