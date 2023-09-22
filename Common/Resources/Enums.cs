using System.ComponentModel.DataAnnotations;

namespace Resources
{
    public enum StatusNames
    {

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Diasable))]
        Diasable = 0,

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Enable))]
        Enable = 1,

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.InProgress))]
        InProgress = 2
    }

    public enum StatusContract

    { 
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Pending))]
        Pending = 0,

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Read))]
        Read = 1,
    }

    public enum ProductPriceType
    {
        [Display(Name ="اصلی")]
        Base = 0,
        [Display(Name ="لحظه ای")]
        Momentary = 1,
        [Display(Name ="پیشنهادی")]
        Suggested =2,
    }
    public enum Sex
    {
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Mr))]
        Mr = 0,
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Mrs))]
        Mrs = 1
    }
    public enum UserType
    {
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Admin))]
        Admin,
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.user))]
        User
    }
    public enum RoleType
    {

        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Admin))]
        Admin,
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.user))]
        User,

    }
    public enum TypeSeo
    {
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Meta))]
        Meta,
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.Title))]
        Title,
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.TypeSeoScript))]
        Script,
        [Display(ResourceType = typeof(DataDictionary), Name = nameof(DataDictionary.TypeSeoLink))]
        Link
    }
}
