using Resources;
using Resources;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Dashboard.ViewModels
{
  public enum TypePanel
{
    [Display(Name = "صفحه اصلی")]
    Home = 0,
    [Display(Name = "مدیریت")]
    Admin = 1,
    [Display(Name = "فروشگاه")]
    Store = 2
}
    public enum Sex
    {
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Mr))]
        Mr = 0,
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Mrs))]
        Mrs = 1
    }
}
