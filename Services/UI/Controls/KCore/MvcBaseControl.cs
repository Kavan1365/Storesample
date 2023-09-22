using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

namespace Services.UI.Controls.KCore
{

    public abstract class MvcBaseControl : IHtmlContent
    {
        internal ControlFactory ControlFactory;
        public MvcBaseControl(ControlFactory controlFactory)
        {
            ControlFactory = controlFactory;
        }
        private object _htmlAttributes { get; set; }
        internal string Name { get; set; }
        internal abstract string TagName { get; }
        internal abstract bool SelfClosing { get; }
        internal abstract MvcControlAttributes GetAttributes();
        internal abstract string GetContent();
        internal virtual string Render()
        {

            var attributes = GetAttributes();
            var htmlAttributes = _htmlAttributes?.GetType().GetProperties().Select(x => $"{x.Name}='{x.GetValue(_htmlAttributes).ToString()}'");
            var content = GetContent();

            IDictionary<string, string> dictionary =
                            new Dictionary<string, string>();

            bool? requird = false;
            var getrequird =
               ControlFactory.HtmlHelper.ViewData.ModelMetadata.Properties?.Where(x => x.PropertyName == Name)?.FirstOrDefault()?.IsRequired;
            var attributeslist =
                        ((Microsoft.AspNetCore.Mvc.ModelBinding.Metadata.DefaultModelMetadata)ControlFactory.HtmlHelper
                            .ViewData.ModelMetadata).Attributes;
            var requiredAttribute = attributeslist?.PropertyAttributes?.OfType<RequiredAttribute>()?.FirstOrDefault();
            if (getrequird != null)
            {
                requird = getrequird;
            }
            if (ControlFactory.HtmlHelper.ViewData.ModelMetadata.IsRequired || requird.Value)
            {
                dictionary.Add("data-val", "true");
                dictionary.Add("data-val-required",!string.IsNullOrEmpty(requiredAttribute?.ErrorMessage)? requiredAttribute?.ErrorMessage:"این فیلد الزامی است!");

            }
            return $@"<{TagName} id='{Name}' name='{Name}'

            { string.Join(" ", htmlAttributes ?? new List<string>() { })}
                  {   string.Join(" ", dictionary.ToList().Select(x => $"{x.Key}='{x.Value}'").ToList())} 
{attributes.Serialize()} 
                    {(SelfClosing ? "/>" : $">{content}</{TagName}>")}";

        }
        private string MakeIt()
        {
            return Render();
        }
        public string ToHtmlString()
        {
            return MakeIt();
        }
        public override string ToString()
        {
            return MakeIt();
        }

        public object HtmlAttributes(object obj)
        {
            _htmlAttributes = obj;
            return this;
        }

        public void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            
            
            writer.Write(MakeIt());
        }
    }
}
