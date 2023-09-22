using DocumentFormat.OpenXml.Wordprocessing;
using Services.Models;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModels.Prodcutes
{
    public class ColorViewModel : BaseViewModel
    {

        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [Display(Name = "کد")]
        public string HexCode { get; set; }

    }
}
