using DocumentFormat.OpenXml.Wordprocessing;
using System.Security.AccessControl;

namespace Dashboard.ViewModels
{
    public class CaptchaDto
    {
        public string CaptchaImage { get; set; }
        public string CaptchaId { get; set; }
    }
}
