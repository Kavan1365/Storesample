using BaseCore.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Base
{
    public class Banner:BaseEntity
    {
        [MaxLength(2000)]
        public string BannerOneRightUrl { get; set; }
        public int? BannerOneRightId { get; set; }
        [ForeignKey(nameof(BannerOneRightId))]
        public File BannerOneRight { get; set; }


        [MaxLength(2000)]
        public string Url { get; set; }

        [MaxLength(2000)]
        public string BannerOneLeftUrl { get; set; }
        public int? BannerOneLeftId { get; set; }
        [ForeignKey(nameof(BannerOneLeftId))]
        public File BannerLeftOne { get; set; }



        [MaxLength(2000)]
        public string BannerTwoUrl { get; set; }
        public int? BannerTwoId { get; set; }
        [ForeignKey(nameof(BannerTwoId))]
        public File BannerTwo { get; set; }

        [MaxLength(2000)]
        public string BannerThreeUrl { get; set; }
        public int? BannerThreeId { get; set; }
        [ForeignKey(nameof(BannerThreeId))]
        public File BannerThree { get; set; }

    }
}
