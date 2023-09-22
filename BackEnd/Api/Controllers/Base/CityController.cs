using Api.Configuration;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseCore.Configuration;
using BaseCore.ViewModel;
using Core.Entities.Countries;
using Core.Repositories.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using ViewModels.Base;

namespace Api.Controllers.Base

{
    [Authorize()]
    public class CityController : CrudController<CityViewModel, CityViewModel, City>
    {
        public CityController(IRepository<City> repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
        [AllowAnonymous]
        [HttpPost("[action]/{id:Guid}")]
        public virtual async Task<IActionResult> List(Guid id, DataSourceRequest request, CancellationToken cancellationToken)
        {
            var query = Repository.TableNoTracking.Include(x => x.Province).Where(x => x.Province.Guid == id);

            return new DataSourceResult<CityViewModel>(
                               query.ProjectTo<CityViewModel>(Mapper.ConfigurationProvider),
                               request);
        }
        [AllowAnonymous]
        [HttpPost("[action]")]
        public virtual async Task<IActionResult> ListAll (DataSourceRequest request, CancellationToken cancellationToken)
        {
            var query = Repository.TableNoTracking.Include(x => x.Province);

            return new DataSourceResult<CityViewModel>(
                               query.ProjectTo<CityViewModel>(Mapper.ConfigurationProvider),
                               request);
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public virtual async Task<ApiResult<IEnumerable>> ListByProvince(CancellationToken cancellationToken)
        {
            var obj = await Repository.TableNoTracking.Select(x => new { Id = x.Id, Title = x.Title, Province = x.ProvinceId }).ToListAsync(cancellationToken);

            return obj;
        }
        [HttpGet("[action]/{id}")]
        [AllowAnonymous]
        public virtual async Task<ApiResult<IEnumerable>> ListByProvinceId(int id, CancellationToken cancellationToken)
        {
            var obj = await Repository.TableNoTracking.Where(z => z.ProvinceId == id).Select(x => new { Id = x.Id, Title = x.Title, Province = x.ProvinceId }).ToListAsync(cancellationToken);

            return obj;
        }

    }
}
