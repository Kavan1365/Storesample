using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseCore.Core.ViewModel
{
    public class BaseViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
        [HiddenInput]
        public Guid Guid { get; set; }

        
    }
}
