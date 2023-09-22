using Api.Configuration;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseCore.Configuration;
using BaseCore.Dapper;
using BaseCore.ViewModel;
using Core.Entities.Prodcutes;
using Core.Repositories.Base;
using Infrastructure.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ViewModels.Prodcuts;

namespace Api.Controllers.Prodcuts
{


    public class SubFilterController : BaseController
    {

        private readonly IRepository<SubFilter> _service;
        private readonly ILogger<SubFilterController> _logger;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;


        public SubFilterController(IMapper mapper,
           IMemoryCache cache, ILogger<SubFilterController> logger,
            IRepository<SubFilter> service)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }



        [HttpPost("[action]/{id}")]
        public IActionResult List(int id, DataSourceRequest request, CancellationToken cancellationToken)
        {

            var query = _service.TableNoTracking.Include(z=>z.ProductProperty).Where(z=>z.ProductPropertyId.Equals(id));
            return new DataSourceResult<SubFilterViewModel>(
                               query.ProjectTo<SubFilterViewModel>(_mapper.ConfigurationProvider),
                               request);

        }

        [HttpPost("[action]/{id}")]
        public virtual async Task<ApiResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var model = await _service.GetByGuidAsync(cancellationToken, id);
                await _service.DeleteAsync(model, cancellationToken);
                return Ok();
            }
            catch (Exception)
            {

                return new ApiResult(false, BaseCore.Utilities.ApiResultStatusCode.LogicError, Resources.ErrorMessages.UnableDeleteItems);
            }

        }
        [HttpGet("{id}")]
        public virtual async Task<ApiResult<SubFilterViewModel>> Get(Guid id, CancellationToken cancellationToken)
        {
            var dto = await _service.TableNoTracking.ProjectTo<SubFilterViewModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Guid.Equals(id), cancellationToken);

            if (dto == null)
                return NotFound();

            return dto;
        }

        [HttpPost("[action]")]

        public async Task<ApiResult<SubFilterViewModel>> Update(SubFilterViewModel dto, CancellationToken cancellationToken)
        {
            try
            {

                var obj = _service.GetById(dto.Id);
                if (obj != null)
                {
                    var checkTitle = _service.TableNoTracking.Any(z => z.Title == dto.Title && z.Guid != dto.Guid);
                    if (checkTitle)
                        return new ApiResult<SubFilterViewModel>(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, null, Resources.ErrorMessages.TitleIsExists);
                    dto.Guid = obj.Guid;

                    var model = dto.ToEntity(_mapper, obj);
                    await _service.UpdateAsync(model, cancellationToken);

                    var resultDto = await _service.TableNoTracking.ProjectTo<SubFilterViewModel>(_mapper.ConfigurationProvider)
                        .SingleOrDefaultAsync(p => p.Id.Equals(model.Id), cancellationToken);

                    return resultDto;
                }
                return new ApiResult<SubFilterViewModel>(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, null, Resources.ErrorMessages.NotFound);


            }
            catch (Exception e)
            {
                 return null;

            }
        }
        [HttpPost()]
        public async Task<ApiResult<SubFilterViewModel>> Create(SubFilterViewModel dto, CancellationToken cancellationToken)
        {
            try
            {

                var checkTitle = _service.TableNoTracking.Any(z => z.Title == dto.Title);
                if (checkTitle)
                    return new ApiResult<SubFilterViewModel>(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, null, Resources.ErrorMessages.IsNotUniqTitle);
                var model = dto.ToEntity(_mapper);
                await _service.AddAsync(model, cancellationToken);

                var resultDto = await _service.TableNoTracking.ProjectTo<SubFilterViewModel>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(p => p.Id.Equals(model.Id), cancellationToken);

                return resultDto;


            }
            catch (Exception e)
            {
                return null;

            }
        }




        [HttpPost("[action]/{productPropertyId}")]
        public IActionResult ListByProductPropertyId(int productPropertyId, DataSourceRequest request)
        {
            var query = _service.TableNoTracking.Where(x => x.ProductPropertyId == productPropertyId);

            return new DataSourceResult<SubFilterViewModel>(
                               query.ProjectTo<SubFilterViewModel>(_mapper.ConfigurationProvider),
                               request);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public virtual async Task<IActionResult> UpdateInlineSubFilter(int storeid, UpdateInlineSubFilterViewModel model, CancellationToken cancellationToken)
        {

            foreach (var item in model.models)
            {
                var obj = _service.GetById(item.Id);
                obj.Title = item.Title;
                obj.ViewOrder = item.ViewOrder;
                await _service.UpdateAsync(obj, cancellationToken);
            }
            return Ok();
        }

    }
}
