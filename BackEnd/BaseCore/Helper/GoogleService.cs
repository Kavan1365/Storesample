using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BaseCore.Helper
{
    public class ResultGoogle
    {
        public bool success { get; set; }
        public double score { get; set; }
    }
    public class GoogleService
    {
        private readonly IConfiguration _configuration;
        public GoogleService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> VerifyToken(string token)
        {
            try
            {
                var url = $"https://www.google.com/recaptcha/api/siteverify?secret={_configuration["GooglereCAPTCHA:secretKey"]}&response={token}";
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        return false;
                    }

                    var result = await response.Content.ReadAsStringAsync();
                    var resultGoogle = JsonConvert.DeserializeObject<ResultGoogle>(result);
                    return resultGoogle.success && resultGoogle.score > 0.5;

                }


            }
            catch
            {
                return false;
            }
        }
    }
}
