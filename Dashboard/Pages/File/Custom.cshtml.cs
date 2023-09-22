using Microsoft.AspNetCore.Mvc;
using Services.Configuration.Helper;
using Services.Models;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Dashboard.Pages.File
{
    public class CustomModel : BasePageAllowModel
    {
        private readonly ILogger<CustomModel> _logger;
        private HttpClient _httpClient;


        public CustomModel(IHttpService httpService, HttpClient httpClient, ILogger<CustomModel> logger, IHttpContextAccessor accessor, IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpService, accessor, httpClientFactory, configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string action,int? fileid, CancellationToken cancellationToken)
        {
            var files = HttpContext.Request.Form.Files;
            if (files is not null)
            {
                var file = HttpContext.Request.Form.Files[0];
                var url = "file/"+ action;
                if (fileid.HasValue)
                {
                    url = "file/"+ action+"/" + fileid;
                }
                var request = new HttpRequestMessage(HttpMethod.Post, _configuration["urlhelper:enternalurl"] + url);

                var accessToken = _httpContextAccessor.HttpContext.User.Identities.Where(z => z.NameClaimType == ClaimTypes.Name).FirstOrDefault().Name;
                if (!string.IsNullOrEmpty(accessToken))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }

                var formDataContent = new MultipartFormDataContent();

                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    byte[] fileData = stream.ToArray();
                    ByteArrayContent byteContent = new ByteArrayContent(fileData);
                    formDataContent.Add(byteContent, "file", HttpContext.Request.Form.Files[0].FileName);
                    request.Content = formDataContent;
                    var result = await _httpClient.SendAsync(request, cancellationToken);
                    var data = await result.Content.ReadFromJsonAsync<ApiResult>();
                    if (data.isSuccess)
                    {
                        return Content(data.message);

                    }
                    return Content("false");

                }

            }
            else
            {
                return Content("false");

            }

        }
    }
}
