using BaseCore.Core;
using Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.AAA
{
    [Table("Role", Schema = "AAA")]
    public class Role : BaseEntity
    {
        [MaxLength(500)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Disc { get; set; }
        public RoleType RoleType { get; set; }

        public int OrderView { get; set; }
    }
}
