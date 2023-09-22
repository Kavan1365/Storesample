using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Services.Helper;
using Services.Models;
using Resources;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Services.Configuration.Helper
{
    public interface IHttpService
    {
        Task<ApiResult<string>> GetByString(string uri, bool istoken = true);

        Task<ApiResult> Get(string uri, bool istoken = true);
        Task<ApiResult<T>> Get<T>(string uri, bool istoken = true) where T : class;
        Task<ApiResult<T>> Get<T>(string uri, object value, bool istoken = true) where T : class;
        Task<ApiResult> Post(string uri, bool istoken = true);
        Task<ApiResult<T>> Post<T>(string uri, object value, bool istoken = true) where T : class;
        Task<ApiResult<T>> Put<T>(string uri, object value, bool istoken = true) where T : class;
        Task<FileStream> DownloadFile(string uri);
        Task<ApiResult> Delete(string uri);
        Task<string> GetToken();

    }

    public class HttpService : IHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _httpClient;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<HttpService> _logger;
        private readonly IConfiguration _config;
        public HttpService(
          IHttpClientFactory httpClientFactory,
           HttpClient httpClient,
           IHttpContextAccessor httpContextAccessor,
        IConfiguration config,
        ILogger<HttpService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _logger = logger;
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }


        public Task<ApiResult<T>> GetAnonymous<T>(string url, bool istoken) where T : class
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            if (istoken)
                return sendRequest<T>(request);
            return sendRequestAnonymous<T>(request);

        }
        public Task<ApiResult> Get(string url, bool istoken)
        {

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            if (istoken)
                return sendRequest(request);
            return sendRequestAnonymous(request);
        }
        public Task<ApiResult<T>> Get<T>(string url, bool istoken) where T : class
        {

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            if (istoken)
                return sendRequest<T>(request);
            return sendRequestAnonymous<T>(request);
        }
        public Task<ApiResult<string>> GetByString(string uri, bool istoken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            if (istoken)
                return sendRequest<string>(request);
            return sendRequestAnonymous<string>(request);
        }

        public Task<ApiResult<T>> Get<T>(string uri, object value, bool istoken) where T : class
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            if (istoken)
                return sendRequest<T>(request);
            return sendRequestAnonymous<T>(request);
        }
        public Task<ApiResult<T>> Post<T>(string uri, object value, bool istoken) where T : class
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            if (istoken)
                return sendRequest<T>(request);
            return sendRequestAnonymous<T>(request);
        }

        public Task<ApiResult<T>> Put<T>(string uri, object value, bool istoken) where T : class
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            if (istoken)
                return sendRequest<T>(request);
            return sendRequestAnonymous<T>(request);
        }



        public Task<ApiResult> Delete(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            return sendRequest(request);

        }

        private async Task<ApiResult> sendRequestAnonymous(HttpRequestMessage request)
        {
            try
            {

                _httpClient.DefaultRequestHeaders.Accept.Clear();
                using var response = await _httpClient.SendAsync(request);

                // auto logout on 401 response

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return new ApiResult() { isSuccess = false, message = ErrorMessages.Unauthorized };
                }
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    return new ApiResult() { isSuccess = false, message = ErrorMessages.NotAllowRequest };
                }

                // throw exception on error response
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadFromJsonAsync<ApiResult>();
                    var stringresult = await response.Content.ReadAsStringAsync();
                    _logger.LogError(request.RequestUri + stringresult);
                    return new ApiResult() { isSuccess = false, message = string.Join(",", error.message) };

                }
                var result = await response.Content.ReadFromJsonAsync<ApiResult>();
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(request.RequestUri + e.Message);
                return new ApiResult() { isSuccess = false, message = e.Message };

            }

        }

        private async Task<ApiResult<TData>> sendRequestAnonymous<TData>(HttpRequestMessage request) where TData : class
        {
            try
            {

                _httpClient.DefaultRequestHeaders.Accept.Clear();
                using var response = await _httpClient.SendAsync(request);
                var stringresult = await response.Content.ReadAsStringAsync();
                // auto logout on 401 response

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return new ApiResult<TData>() { data = null, isSuccess = false, message = ErrorMessages.Unauthorized };
                }
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    return new ApiResult<TData>() { data = null, isSuccess = false, message = ErrorMessages.NotAllowRequest };
                }

                // throw exception on error response
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadFromJsonAsync<ApiResult<TData>>();
                    _logger.LogError(request.RequestUri + stringresult);

                    return new ApiResult<TData>() { data = null, isSuccess = false, message = string.Join(",", error.message) };

                }
                var result = await response.Content.ReadFromJsonAsync<ApiResult<TData>>();
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(request.RequestUri + e.Message);
                return new ApiResult<TData>() { data = null, isSuccess = false, message = e.Message };

            }

        }


        private async Task<ApiResult<TData>> sendRequest<TData>(HttpRequestMessage request) where TData : class
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                // set the bearer token to the outgoing request as Authentication Header
                // 
                // var accessToken = await RequestClientCredentialsTokenAsync();

                //var getToken = await GetToken();
                //if (!string.IsNullOrEmpty(getToken))
                //{
                //    request.SetBearerToken(getToken);
                //}
                if (_httpContextAccessor.HttpContext.User != null)
                {

                    var accessToken = _httpContextAccessor.HttpContext.User.Identities.Where(z => z.NameClaimType == ClaimTypes.Name).FirstOrDefault().Name;
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    }
                }
                using var response = await _httpClient.SendAsync(request);
                var stringresult = await response.Content.ReadAsStringAsync();
                // auto logout on 401 response

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return new ApiResult<TData>() { data = null, isSuccess = false, message = ErrorMessages.Unauthorized };
                }
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    return new ApiResult<TData>() { data = null, isSuccess = false, message = ErrorMessages.NotAllowRequest };
                }

                // throw exception on error response
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadFromJsonAsync<ApiResult<TData>>();
                    _logger.LogError(request.RequestUri + stringresult);

                    return new ApiResult<TData>() { data = null, isSuccess = false, message = string.Join(",", error.message) };

                }
                var result = await response.Content.ReadFromJsonAsync<ApiResult<TData>>();
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(request.RequestUri + e.Message);
                return new ApiResult<TData>() { data = null, isSuccess = false, message = e.Message };

            }

        }

        private async Task<ApiResult> sendRequest(HttpRequestMessage request)
        {

            try
            {

                //var getToken = await GetToken();
                //if (!string.IsNullOrEmpty(getToken))
                //{
                //    request.SetBearerToken(getToken);
                //}
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                if (_httpContextAccessor.HttpContext.User != null)
                {

                    var accessToken = _httpContextAccessor.HttpContext.User.Identities.Where(z => z.NameClaimType == ClaimTypes.Name).FirstOrDefault().Name;
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    }
                }
                using var response = await _httpClient.SendAsync(request);
                var stringresult = await response.Content.ReadAsStringAsync();

                // auto logout on 401 response
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return new ApiResult() { isSuccess = false, message = ErrorMessages.Unauthorized, statusCode = 404 };
                }
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    _logger.LogError(request.RequestUri + stringresult);

                    return new ApiResult() { isSuccess = false, message = ErrorMessages.NotAllowRequest };
                }


                // throw exception on error response
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                    _logger.LogError(request.RequestUri + stringresult);

                    throw new Exception(error["message"]);
                }
                var result = await response.Content.ReadFromJsonAsync<ApiResult>();
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(request.RequestUri + e.Message);
                return new ApiResult() { isSuccess = false, message = e.Message };
            }

        }

        public async Task<FileStream> DownloadFile(string uri)
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            var response = await _httpClient.GetAsync(uri);
            //var getToken = await GetToken();
            //if (!string.IsNullOrEmpty(getToken))
            //{
            //    request.SetBearerToken(getToken);
            //}
            if (_httpContextAccessor.HttpContext.User != null)
            {

                var accessToken = _httpContextAccessor.HttpContext.User.Identities.Where(z => z.NameClaimType == ClaimTypes.Name).FirstOrDefault().Name;
                if (!string.IsNullOrEmpty(accessToken))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }
            }

            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            FileStream fileStream = null;
            using (Stream input = stream)
            {
                using (Stream memory = CopyToMemory(input))
                {
                    byte[] data = new byte[memory.Length];
                    memory.Read(data, 0, (int)memory.Length);

                    fileStream.Read(data, 0, (int)memory.Length);
                    memory.Close();
                    return fileStream;

                }
            }


        }
        private MemoryStream CopyToMemory(Stream input)
        {
            // It won't matter if we throw an exception during this method;
            // we don't *really* need to dispose of the MemoryStream, and the
            // caller should dispose of the input stream
            MemoryStream ret = new MemoryStream();

            byte[] buffer = new byte[8192];
            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                ret.Write(buffer, 0, bytesRead);
            }
            // Rewind ready for reading (typical scenario)
            ret.Position = 0;
            return ret;
        }
   

        public Task<ApiResult> Post(string uri, bool istoken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            if (istoken)
                return sendRequest(request);
            return sendRequestAnonymous(request);
        }

        public async Task<string> GetToken()
        {
            if (_httpContextAccessor.HttpContext.User != null)
            {

                var accessToken = _httpContextAccessor.HttpContext.User.Identities.Where(z => z.NameClaimType == ClaimTypes.Name).FirstOrDefault().Name;
                if (!string.IsNullOrEmpty(accessToken))
                {
                    return accessToken;
                }
            }


            return null;

        }

    }
}
