using BaseCore.Configuration;
using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Base
{
    public class BannerViewModel : BaseDto<BannerViewModel, Banner>
    {
        [MaxLength(2000)]
        public string BannerOneRightUrl { get; set; }
        public int? BannerOneRightId { get; set; }

        [MaxLength(2000)]
        public string Url { get; set; }

        [MaxLength(2000)]
        public string BannerOneLeftUrl { get; set; }
        public int? BannerOneLeftId { get; set; }



        [MaxLength(2000)]
        public string BannerTwoUrl { get; set; }
        public int? BannerTwoId { get; set; }

        [MaxLength(2000)]
        public string BannerThreeUrl { get; set; }
        public int? BannerThreeId { get; set; }
    }
}
