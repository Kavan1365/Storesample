using Api.Configuration;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseCore.Configuration;
using Core.Entities.Base;
using Core.Repositories.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using ViewModels.Base;

namespace Api.Controllers.Base
{
   // [Authorize(Roles = "Admin,Employee")]
    public class SeoController : CrudController<SeoViewModel, SeoViewModel, Seo>
    {
        public SeoController(IRepository<Seo> repository, IMapper mapper) : base(repository, mapper)
        {
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<List<SeoViewModel>> GetListForClient(string url,CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(url))
            {
                return new List<SeoViewModel>();
            }
            var dto = await Repository.TableNoTracking.Where(z=>z.SeoUrl.Equals(url)).ProjectTo<SeoViewModel>(Mapper.ConfigurationProvider)
                          .ToListAsync(cancellationToken);
            return dto;
        }
        
        [AllowAnonymous]
        [HttpPost("[action]")]
        public Dictionary<string, List<MetadataValue>> RouteDetailMapping(CancellationToken cancellationToken)
        {
            var result = new Dictionary<string, List<MetadataValue>>();
            var obj = Repository.TableNoTracking.Include(z => z.SeoItems).ToList();
            foreach (var item in obj)
            {
                var MetadataValuelist = new List<MetadataValue>();
                foreach (var value in item.SeoItems) {
                    MetadataValuelist.Add(new MetadataValue() {Title=value.Title, ContentMenta = value.ContentMenta, LinkSeo = value.LinkSeo, NameMeta = value.NameMeta, TypeSeo = value.TypeSeo });
                }
                result.Add(item.SeoUrl, MetadataValuelist);

            }
            return result;
        }


    }

    [AllowAnonymous]
    public class SeoClientController : BaseNotMappController
    {
        private readonly IRepository<Seo> Repository;
        private readonly IMapper Mapper;
        public SeoClientController(IRepository<Seo> repository, IMapper mapper) 
        {
            Repository=repository;
            Mapper=mapper;
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public ApiResult<List<MetadataValue>> GetListForClient(string url, CancellationToken cancellationToken)
        {
            var dto = Repository.TableNoTracking.Include(z=>z.SeoItems).Where(z => url.EndsWith(z.SeoUrl)).FirstOrDefault();
            if (dto is null)
                return new ApiResult<List<MetadataValue>>(true,BaseCore.Utilities.ApiResultStatusCode.Success,new List<MetadataValue>());

          var result=  dto.SeoItems.ToList().Select(z => new MetadataValue()
            {
                ContentMenta = z.ContentMenta,
                LinkSeo = z.LinkSeo,
                NameMeta = z.NameMeta,
                Title = z.Title,
                TypeSeo = z.TypeSeo
            }).ToList();
            return result;
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public ApiResult<Dictionary<string, List<MetadataValue>>> RouteDetailMapping(CancellationToken cancellationToken)
        {
            var result = new Dictionary<string, List<MetadataValue>>();
            var obj = Repository.TableNoTracking.Include(z => z.SeoItems).ToList();
            foreach (var item in obj)
            {
                var MetadataValuelist = new List<MetadataValue>();
                foreach (var value in item.SeoItems)
                {
                    MetadataValuelist.Add(new MetadataValue() { Title = value.Title, ContentMenta = value.ContentMenta, LinkSeo = value.LinkSeo, NameMeta = value.NameMeta, TypeSeo = value.TypeSeo });
                }
                result.Add(item.SeoUrl, MetadataValuelist);

            }
            return result;
        }


    }
}
