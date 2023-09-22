using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using BaseCore.Core.AAA;

namespace BaseCore.Core
{
    public class BaseEntityNoKey
    {
        public byte[] RowVersion { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DefaultValue("getdate()")]
        public DateTime? Created { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedByUserId { get; set; }

        public Guid Guid { get; set; }
    }
}
