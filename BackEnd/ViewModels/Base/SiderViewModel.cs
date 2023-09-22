using BaseCore.Configuration;
using Core.Entities.Base;

namespace ViewModels.Base
{
    public class SiderViewModel : BaseDto<SiderViewModel, Sider>
    {
        public string UrlLink { get; set; }
        public int ImageId { get; set; }
        public string ImageUrl { get; set; }

    }
}