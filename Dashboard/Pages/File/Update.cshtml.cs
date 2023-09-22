using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Configuration.Helper;
using Services.Models;


namespace Dashboard.Pages.File
{
    public class UpdateModel : BasePageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private HttpClient _httpClient;


        public UpdateModel(IHttpService httpService, HttpClient httpClient, ILogger<IndexModel> logger, IHttpContextAccessor accessor, IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpService, accessor, httpClientFactory, configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(int guid, CancellationToken cancellationToken)
        {
            var files = HttpContext.Request.Form.Files;
            if (files is not null)
            {
                var file = HttpContext.Request.Form.Files[0];
                var request = new HttpRequestMessage(HttpMethod.Post, _configuration["urlhelper:enternalurl"] + "file/UpdateUpload/"+guid);
                var formDataContent = new MultipartFormDataContent();

                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    byte[] fileData = stream.ToArray();
                    ByteArrayContent byteContent = new ByteArrayContent(fileData);
                    formDataContent.Add(byteContent, "file", HttpContext.Request.Form.Files[0].FileName);
                    request.Content = formDataContent;
                   var result= await _httpClient.SendAsync(request, cancellationToken);
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
