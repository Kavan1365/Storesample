using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseCore.Configuration;
using BaseCore.ViewModel;
using Core.Entities.Prodcutes;
using Core.Repositories.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections;
using ViewModels.Prodcuts;

namespace Api.Controllers.Prodcuts
{
    public class ColorController : BaseController
    {

        private readonly IRepository<Color> _service;
        private readonly IRepository<Core.Entities.Base.File> _fileservice;
        private IWebHostEnvironment _environment;

        private readonly ILogger<ColorController> _logger;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;


        public ColorController(IMapper mapper,
            IRepository<Core.Entities.Base.File> fileservice,
           IMemoryCache cache,ILogger<ColorController> logger,
            IRepository<Color> service, IWebHostEnvironment environment)
        {
            _fileservice = fileservice ?? throw new ArgumentNullException(nameof(fileservice));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _environment = environment; 
        }

        [HttpGet("{id}")]
        public virtual async Task<ApiResult<ColorViewModel>> Get(Guid id, CancellationToken cancellationToken)
        {
            var dto = await _service.TableNoTracking.ProjectTo<ColorViewModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Guid.Equals(id), cancellationToken);

            if (dto == null)
                return NotFound();

            return dto;
        }
        [HttpGet("[action]/{title}")]
        public virtual async Task<ApiResult<ColorViewModel>> GetByTitle(string title, CancellationToken cancellationToken)
        {
            var dto = await _service.TableNoTracking.Where(p => p.Title.Equals(title)).ProjectTo<ColorViewModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            if (dto == null)
                return NotFound();

            return dto;
        }
        [HttpPost("[action]")]
        public virtual async Task<IActionResult> List(DataSourceRequest request, CancellationToken cancellationToken)
        {
            var query = _service.TableNoTracking;

            return new DataSourceResult<ColorViewModel>(
                               query.ProjectTo<ColorViewModel>(_mapper.ConfigurationProvider),
                               request);
        }




        [HttpGet("[action]")]
        [AllowAnonymous]
        public virtual async Task<ApiResult<IEnumerable>> ColorAllList(CancellationToken cancellationToken)
        {


            var cacheEntry = new List<ColorViewModel>();
            var obj = cacheEntry
                 .Select(z => new { Title = z.Title, Id = z.Id })
                 .ToList();
            return obj;
        }

        [HttpPost("[action]")]

        public async Task<ApiResult<ColorViewModel>> Update(ColorViewModel dto, CancellationToken cancellationToken)
        {
            try
            {

                var obj = _service.GetById(dto.Id);
                if (obj != null)
                {
                    var checkTitle = _service.TableNoTracking.Any(z => z.Title == dto.Title && z.Guid != dto.Guid);
                    if (checkTitle)
                        return new ApiResult<ColorViewModel>(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, null, Resources.ErrorMessages.TitleIsExists);
                    var checkHexCode = _service.TableNoTracking.Any(z => z.HexCode == dto.HexCode && z.Guid != dto.Guid);
                    if (checkHexCode)
                        return new ApiResult<ColorViewModel>(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, null, "کد رنگ تکراری می باشد.");
                    dto.Guid = obj.Guid;

                    var model = dto.ToEntity(_mapper, obj);
                    await _service.UpdateAsync(model, cancellationToken);

                    var resultDto = await _service.TableNoTracking.ProjectTo<ColorViewModel>(_mapper.ConfigurationProvider)
                        .SingleOrDefaultAsync(p => p.Id.Equals(model.Id), cancellationToken);

                    return resultDto;
                }
                return new ApiResult<ColorViewModel>(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, null,Resources.ErrorMessages.NotFound );


            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return null;

            }
        }
        [HttpPost()]
        public async Task<ApiResult<ColorViewModel>> Create(ColorViewModel dto, CancellationToken cancellationToken)
        {
            try
            {

                var checkTitle = _service.TableNoTracking.Any(z => z.Title == dto.Title);
                if (checkTitle)
                    return new ApiResult<ColorViewModel>(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, null, Resources.ErrorMessages.IsNotUniqTitle);

                var checkHexCode = _service.TableNoTracking.Any(z => z.HexCode == dto.HexCode);
                if (checkHexCode)
                    return new ApiResult<ColorViewModel>(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, null, "کد رنگ تکراری می باشد.");

                var model = dto.ToEntity(_mapper);
                await _service.AddAsync(model, cancellationToken);

                var resultDto = await _service.TableNoTracking.ProjectTo<ColorViewModel>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(p => p.Id.Equals(model.Id), cancellationToken);

                return resultDto;


            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return null;

            }
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

    }
}
