using BaseCore.Core;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Base
{
  
    public class ContactUSList:BaseEntity
    {
        [MaxLength(100)]
        public string FullName { set; get; }
        [MaxLength(100)]
        public string EmailName { set; get; }
        [MaxLength(1000)]
        public string Title { set; get; }
        [MaxLength(10000)]
        public string Description { get; set; }
        public StatusContract StatusContract { get; set; }

    }
}
