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
using Core.Entities.Base;
using ViewModels.Base;
using Resources;

namespace Api.Controllers.Base
{
    [Authorize()]
    public class SiderController : BaseController
    {

        private readonly IRepository<Sider> _service;
        private readonly IRepository<Core.Entities.Base.File> _fileservice;
        private IWebHostEnvironment _environment;

        private readonly ILogger<SiderController> _logger;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;


        public SiderController(IMapper mapper,
            IRepository<Core.Entities.Base.File> fileservice,
           IMemoryCache cache, ILogger<SiderController> logger,
            IRepository<Sider> service, IWebHostEnvironment environment)
        {
            _fileservice = fileservice ?? throw new ArgumentNullException(nameof(fileservice));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _environment = environment;
        }

        [HttpGet("{id}")]
        public virtual async Task<ApiResult<SiderViewModel>> Get(Guid id, CancellationToken cancellationToken)
        {
            var dto = await _service.TableNoTracking.Include(x => x.Image).ProjectTo<SiderViewModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Guid.Equals(id), cancellationToken);

            if (dto == null)
                return NotFound();

            return dto;
        }
     
        [HttpPost("[action]")]
        public virtual async Task<IActionResult> List(DataSourceRequest request, CancellationToken cancellationToken)
        {
            var query = _service.TableNoTracking;

            return new DataSourceResult<SiderViewModel>(
                               query.Include(z => z.Image).ProjectTo<SiderViewModel>(_mapper.ConfigurationProvider),
                               request);
        }




        [HttpGet("[action]")]
        [AllowAnonymous]
        public virtual async Task<ApiResult<List<SiderViewModel>>> SiderAllList(CancellationToken cancellationToken)
        {


            var cacheEntry = new List<SiderViewModel>();
            var obj = await _service.TableNoTracking.Include(x => x.Image).ProjectTo<SiderViewModel>(_mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken);
            return obj;
        }

        [HttpPost("[action]")]

        public async Task<ApiResult<SiderViewModel>> Update(SiderViewModel dto, CancellationToken cancellationToken)
        {
            try
            {

                if (dto.ImageId < 1) { 
                    return new ApiResult<SiderViewModel>(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, null, ErrorMessages.imageValid);
                }
                
                var obj = _service.GetById(dto.Id);
                if (obj != null)
                {

                    var model = dto.ToEntity(_mapper, obj);
                    await _service.UpdateAsync(model, cancellationToken);

                    var resultDto = await _service.TableNoTracking.ProjectTo<SiderViewModel>(_mapper.ConfigurationProvider)
                        .SingleOrDefaultAsync(p => p.Id.Equals(model.Id), cancellationToken);

                    return resultDto;
                }
                return new ApiResult<SiderViewModel>(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, null, Resources.ErrorMessages.NotFound);


            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return null;

            }
        }
        [HttpPost()]
        public async Task<ApiResult<SiderViewModel>> Create(SiderViewModel dto, CancellationToken cancellationToken)
        {
            try
            {
                if (dto.ImageId<1)
                {
                    return new ApiResult<SiderViewModel>(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, null, ErrorMessages.imageValid);
                }

                var model = dto.ToEntity(_mapper);
                await _service.AddAsync(model, cancellationToken);

                var resultDto = await _service.TableNoTracking.ProjectTo<SiderViewModel>(_mapper.ConfigurationProvider)
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

                int? fileid = model.ImageId;
                if (fileid.HasValue)
                {
                    var getFile = _fileservice.GetById(fileid.Value);
                    var root = _environment.WebRootPath;
                    try
                    {
                        await _fileservice.DeleteAsync(getFile, cancellationToken);
                        var fullpath = root + "\\" + "files" + "\\" + getFile.Url.Replace("/", "\\");
                        if (System.IO.File.Exists(fullpath))
                            System.IO.File.Delete(fullpath);

                    }
                    catch (Exception)
                    {

                    }

                }

                return Ok();
            }
            catch (Exception)
            {

                return new ApiResult(false, BaseCore.Utilities.ApiResultStatusCode.LogicError, Resources.ErrorMessages.UnableDeleteItems);

            }
        }

    }
}
