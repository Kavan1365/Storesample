using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Sms
{
    public interface ISmsService
    {
        bool SendMail(string to, string displayName, string subject, string strBody, string path);
        Task<bool> SendSms(string number, string massage);
    }
}
