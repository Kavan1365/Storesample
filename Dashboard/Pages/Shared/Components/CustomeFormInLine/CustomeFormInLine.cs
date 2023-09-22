using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Admin.Views.Shared.Components.CustomeFormInLine
{
    public class CustomeFormInLine : ViewComponent
    {
        public ViewViewComponentResult Invoke(object model)
        {
            return View("CustomeFormInLine", model);
        }
    }
   
}