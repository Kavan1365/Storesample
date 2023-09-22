using Api.Configuration;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseCore.Configuration;
using BaseCore.ViewModel;
using Core.Entities.Base;
using Core.Repositories.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViewModels.Base;

namespace Api.Controllers.Base
{
    [Authorize(Roles = "Admin,Employee")]
    public class SeoItemController : CrudController<SeoItemViewModel, SeoItemViewModel, SeoItem>
    {
        public SeoItemController(IRepository<SeoItem> repository, IMapper mapper) : base(repository, mapper)
        {
        }

        [HttpPost("[action]/{id}")]
        public IActionResult Listcustom(Guid id, DataSourceRequest request, CancellationToken cancellationToken)
        {

            var query = Repository.TableNoTracking.Include(z=>z.Seo.Guid).Where(z=>z.Seo.Equals(id));
            return new DataSourceResult<SeoItemViewModel>(
                               query.ProjectTo<SeoItemViewModel>(Mapper.ConfigurationProvider),
                               request);
        }
    }
}
