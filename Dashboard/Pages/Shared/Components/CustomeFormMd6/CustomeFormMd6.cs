using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.UI.Views.Shared.Components.CustomeFormMd6
{
    public class CustomeFormMd6 : ViewComponent
    {
        public ViewViewComponentResult Invoke(object model)
        {
            return View("CustomeFormMd6", model);
        }
    }

    public class Model<T>
    {
        public T GetModel { get; set; }
    }
  
}
