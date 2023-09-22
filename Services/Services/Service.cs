using Services.Models;
using Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface IService<T> where T : class
    {
        Task<ApiResult<T>> Get(string uri);
        Task Post(string uri, object value);
        Task<ApiResult<T>> PostResult(string uri, object value);
        Task Put(string uri, object value);
        Task<ApiResult<T>> PutResult(string uri, object value) ;
        Task Delete(string uri);
        Task<ApiResult<T>> DeleteResult(string uri);
    }

    public class Service<T> : IService<T> where T : class
    {
        private IHttpService _httpService;
        public Service(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task Delete(string uri)
        {
            await _httpService.Delete(uri);

        }

        public async Task<ApiResult<T>> DeleteResult(string uri)
        {
            return await _httpService.Delete<T>(uri);

        }

        public async Task<ApiResult<T>> Get(string uri)
        {
           return await _httpService.Get<T>(uri);
        }

        public async Task Post(string uri, object value)
        {
            await _httpService.Post(uri,value);

        }

        public async Task<ApiResult<T>> PostResult(string uri, object value)
        {
            return await _httpService.Post<T>(uri,value);
        }

        public async Task Put(string uri, object value)
        {
            await _httpService.Put(uri,value);
        }

        public async Task<ApiResult<T>> PutResult(string uri, object value)
        {
            return await _httpService.Put<T>(uri, value);


        }
    }
}
