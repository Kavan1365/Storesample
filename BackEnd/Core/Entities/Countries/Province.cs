using BaseCore.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Countries
{
    public class Province : BaseEntity
    {
        [MaxLength(100)]
        [Required()]
        public string Title { get; set; }
    }
}
