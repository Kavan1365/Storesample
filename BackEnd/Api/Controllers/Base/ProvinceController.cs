using Api.Configuration;
using AutoMapper;
using BaseCore.Configuration;
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
    public class ProvinceController : CrudController<ProvinceViewModel, ProvinceViewModel, Province>
    {
        public ProvinceController(IRepository<Province> repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
        [HttpGet("[action]")]
        [AllowAnonymous]
        public virtual async Task<ApiResult<IEnumerable>> ProvinceList(CancellationToken cancellationToken)
        {
            var obj = await Repository.TableNoTracking.Select(z => new { Title = z.Title, Id = z.Id }).ToListAsync(cancellationToken);

            return obj;
        }
    }
}
