using BaseCore.Core;
using Core.Entities.Countries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.AAA
{
    public class UserAddress : BaseEntity
    {

        [MaxLength(500)]
        public string TransfereeName { get; set; }
        [MaxLength(500)]
        public string TransfereeMobile { get; set; }


        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { set; get; }


        public int? CityId { get; set; }
        [ForeignKey("CityId")]
        public virtual City City { get; set; }

        [MaxLength(500)]
        public string PostalCode { get; set; }
        [MaxLength(5000)]
        public string Address { get; set; }
        [MaxLength(500)]
        public string Location { get; set; }

        /// <summary>
        /// محله
        /// </summary>
        [MaxLength(500)]
        public string District { get; set; }

        /// <summary>
        /// پلاک
        /// </summary>
        [MaxLength(500)]
        public string HomePlate { get; set; }

        /// <summary>
        /// واحد
        /// </summary>
        [MaxLength(500)]
        public string Unit { get; set; }

    }
}
