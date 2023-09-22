using System.ComponentModel.DataAnnotations;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace BaseCore.Core
{
    public class BaseEntity : IBaseEntity
    {

        [Key]
        public int Id { get; set; }
        public DateTime? Created { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedByUserId { get; set; }

        [IndexColumn(IsUnique = true)]
        public Guid Guid { get; set; }

        public bool IsSoftDeleted { get; set; }


    }

}
