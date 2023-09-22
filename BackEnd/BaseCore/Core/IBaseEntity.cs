using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BaseCore.Core
{
    public interface IBaseEntity
    {
        int Id { get; set; }
        DateTime? Created { get; set; }
        int? CreatedByUserId { get; set; }
        DateTime? Modified { get; set; }
        int? ModifiedByUserId { get; set; }
        Guid Guid { get; set; }
        public bool IsSoftDeleted { get; set; }
    }
}
