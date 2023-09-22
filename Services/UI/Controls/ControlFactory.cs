using Microsoft.AspNetCore.Mvc.Rendering;

namespace Services.UI.Controls
{
    public class ControlFactory
    {
        public IHtmlHelper HtmlHelper { get; set; }
        public ControlFactory(IHtmlHelper htmlHelper)
        {
            this.HtmlHelper = htmlHelper;
        }
    }

    public class ControlFactory<TModel> : ControlFactory
    {
        public IHtmlHelper<TModel> HtmlHelper { get; set; }
        public ControlFactory(IHtmlHelper<TModel> htmlHelper) : base(htmlHelper)
        {
            this.HtmlHelper = htmlHelper;
        }
    }


}
