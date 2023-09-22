using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Helper.Attribute
{
   public class SigmaAttribute: System.Attribute
    {
        public string Name { get; set; }
        public SigmaAttribute(string name)
        {
            Name = name;
        }
    }
}
