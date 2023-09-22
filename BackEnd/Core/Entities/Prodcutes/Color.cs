using BaseCore.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Prodcutes
{
    public class Color : BaseEntity
    {
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(100)]
        public string HexCode { get; set; }
    }

}
