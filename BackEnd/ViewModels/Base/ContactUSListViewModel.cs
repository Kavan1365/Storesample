using BaseCore.Configuration;
using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Base
{
    public class ContactUSListViewModel : BaseDto<ContactUSListViewModel, ContactUSList>
    {

        public string FullName { set; get; }
        public string EmailName { set; get; }
        public string Title { set; get; }
        public string Description { get; set; }
        public string CaptchaId { get; set; }
        public string Captcha { get; set; }
    }
}
