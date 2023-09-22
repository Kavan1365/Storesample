using BaseCore.Core;
using DNTPersianUtils.Core.IranCities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Countries
{
    public class City : BaseEntity
    {

        [MaxLength(100)]
        [Required()]
        public string Title { get; set; }
        public int ProvinceId { get; set; }
        [ForeignKey(nameof(ProvinceId))]
        public virtual Province Province { get; set; }
    }
}
