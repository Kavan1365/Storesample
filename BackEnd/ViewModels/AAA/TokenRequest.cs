namespace ViewModels.AAA
{
    public class TokenRequest
    {
        //  [Required]
        public string grant_type { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool isAdmin { get; set; }
        public string refresh_token { get; set; }

        public string tokengoogle { get; set; }
        public string CaptchaId { get; set; }
        public string Captcha { get; set; }
        public string scope { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public Guid Guid { get; set; }

    }
}
