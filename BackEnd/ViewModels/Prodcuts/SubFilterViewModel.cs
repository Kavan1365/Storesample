using AutoMapper;
using BaseCore.Configuration;
using Core.Entities.Prodcutes;

namespace ViewModels.Prodcuts
{
    public class SubFilterViewModel : BaseDto<SubFilterViewModel, SubFilter>
    {
        public int ProductPropertyId { get; set; }

        public string Title { get; set; }

        public int ViewOrder { get; set; }

       
    }

    public class SubFilterViewModelCreate : BaseDto<SubFilterViewModelCreate, SubFilter>
    {
        public int ProductPropertyId { get; set; }

        public string Title { get; set; }

        public int ViewOrder { get; set; }
    }
}
