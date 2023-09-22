using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class FileViewModel : BaseViewModel
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
        public int Size { get; set; } = 0;
        public string Url { get; set; }

    }
}
