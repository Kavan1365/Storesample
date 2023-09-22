using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class BaseViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
        [HiddenInput]
        public Guid Guid { get; set; }
    }
    public class BaseHierarchicalViewModel : BaseViewModel
    {
        [HiddenInput]
        public int? ParentId { get; set; }

        [HiddenInput]
        public bool hasChildren { get { return true; } }
    }
}
