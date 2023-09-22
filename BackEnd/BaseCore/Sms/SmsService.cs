using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Sms
{
    public class SmsService : ISmsService
    {
        private readonly HttpClient _httpClient;
        public SmsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> SendSms(string number, string massage)
        {
            try
            {


                var request = new HttpRequestMessage(HttpMethod.Post, "http://sms.parsgreen.ir/Apiv2/Message/SendSms");
                request.Headers.Add("Authorization", "basic apikey:D4DAA701-55FF-4CFF-8B97-9B69729DF637");
                request.Content = new StringContent("{\r\n  \"SmsBody\": \"" + massage + "\",\r\n  \"Mobiles\": [\r\n    \"" + number + "\"\r\n  ],\r\n  \"SmsNumber\": \"10009105719700\"\r\n}", Encoding.UTF8, "application/json");
                using var response = await _httpClient.SendAsync(request);
                var sss = await response.Content.ReadAsStringAsync();
                return true;
            }
            catch 
            {

                return false;
            }


        }

        public class MailViewModel
        {
            public string Text { get; set; }
            public string BtnText { get; set; }
            public string BtnUrl { get; set; }
        }
        public bool SendMail(string to, string displayName, string subject, string strBody, string path)
        {
            try
            {
                string from = "";
                string temp = strBody;

                System.Net.Mail.MailMessage oMailMessage = new System.Net.Mail.MailMessage();
                System.Net.Mail.MailAddress oMailAddress = null;

                oMailAddress = new System.Net.Mail.MailAddress
                    (
                    from.ToString().Trim(),
                    displayName.ToString().Trim(),
                    System.Text.Encoding.UTF8
                    );

                oMailMessage.From = oMailAddress;
                oMailMessage.Sender = oMailAddress;

                oMailMessage.To.Clear();
                oMailMessage.CC.Clear();
                oMailMessage.Bcc.Clear();
                oMailMessage.Attachments.Clear();
                oMailMessage.ReplyToList.Clear();

                oMailMessage.ReplyToList.Add(oMailAddress);

                oMailAddress = new System.Net.Mail.MailAddress
                (
                    to.ToString().Trim(),
                    to.ToString().Trim(),
                    System.Text.Encoding.UTF8
                );

                oMailMessage.To.Add(oMailAddress);
                oMailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                oMailMessage.Body = temp;
                oMailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                oMailMessage.Subject = subject.ToString().Trim();
                oMailMessage.IsBodyHtml = true;
                oMailMessage.Priority = System.Net.Mail.MailPriority.Normal;
                oMailMessage.DeliveryNotificationOptions = System.Net.Mail.DeliveryNotificationOptions.Never;


                System.Net.Mail.SmtpClient oSmtpClient = new System.Net.Mail.SmtpClient();
                oSmtpClient.Timeout = 100000;
                oSmtpClient.EnableSsl = false;

                oSmtpClient.Send(oMailMessage);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
