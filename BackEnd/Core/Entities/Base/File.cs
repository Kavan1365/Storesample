using BaseCore.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Base
{
    public class File : BaseEntity
    {
        [MaxLength(1000)]
        [Required]
        public string FileName { get; set; }
        [MaxLength(100)]
        [Required]
        public string Extension { get; set; }
        public int Size { get; set; }

        [MaxLength(10000)]
        [Required]
        public string Path { get; set; }

        public string Url
        {
            get
            {
                return $"{Path}";
            }
        }
    }
}
