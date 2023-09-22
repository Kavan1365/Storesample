using Services.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Dashboard.ViewModels
{
      public class GuideViewModel : BaseViewModel
{
    [Required(ErrorMessage = "لطفا عنوان را وارد کنید")]
    [Display(Name = "عنوان")]

    public string Title { get; set; }
    [Display(Name = "توضیحات")]
    [DataType(dataType: DataType.Html)]

    public string Description { get; set; }
    [Display(Name = "لینک")]
    public string Link { get; set; }
    [Display(Name = "محل نمایش")]

    public TypePanel TypePanel { get; set; }

}
}
