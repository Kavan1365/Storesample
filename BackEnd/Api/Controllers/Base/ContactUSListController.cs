using Api.Configurations.Captcha;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseCore.Configuration;
using BaseCore.Utilities;
using BaseCore.ViewModel;
using Core.Entities.Base;
using Core.Entities.Prodcutes;
using Core.Repositories.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Resources;
using System.Collections;
using ViewModels.Base;
using ViewModels.Prodcuts;

namespace Api.Controllers.Base
{
    public class ContactUSListController : BaseController
    {

        private readonly IRepository<ContactUSList> _service;
        private readonly IRepository<Core.Entities.Base.File> _fileservice;
        private readonly ICaptchaService _captchaService;
        private IWebHostEnvironment _environment;

        private readonly ILogger<ContactUSListController> _logger;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;


        public ContactUSListController(IMapper mapper,
            IRepository<Core.Entities.Base.File> fileservice,
        ICaptchaService captchaService, IMemoryCache cache, ILogger<ContactUSListController> logger,
            IRepository<ContactUSList> service, IWebHostEnvironment environment)
        {
            _captchaService = captchaService ?? throw new ArgumentNullException(nameof(captchaService));
            _fileservice = fileservice ?? throw new ArgumentNullException(nameof(fileservice));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _environment = environment;
        }

        [HttpGet("{id}")]
        public virtual async Task<ApiResult<ContactUSListViewModel>> Get(Guid id, CancellationToken cancellationToken)
        {
            var dto = await _service.TableNoTracking.ProjectTo<ContactUSListViewModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Guid.Equals(id), cancellationToken);

            if (dto == null)
                return NotFound();

            return dto;
        }
        [HttpPost("[action]")]
        public virtual async Task<IActionResult> List(DataSourceRequest request, CancellationToken cancellationToken)
        {
            var query = _service.TableNoTracking.Where(z => z.StatusContract == Resources.StatusContract.Pending);

            return new DataSourceResult<ContactUSListViewModel>(
                               query.ProjectTo<ContactUSListViewModel>(_mapper.ConfigurationProvider),
                               request);
        }

        [HttpPost("[action]")]
        public virtual async Task<IActionResult> ListRead(DataSourceRequest request, CancellationToken cancellationToken)
        {
            var query = _service.TableNoTracking.Where(z => z.StatusContract == Resources.StatusContract.Read);

            return new DataSourceResult<ContactUSListViewModel>(
                               query.ProjectTo<ContactUSListViewModel>(_mapper.ConfigurationProvider),
                               request);
        }



        [AllowAnonymous]
        [HttpPost()]
        public async Task<ApiResult<ContactUSListViewModel>> Create(ContactUSListViewModel dto, CancellationToken cancellationToken)
        {
            try
            {
                var checkGoogleToken = _captchaService.ValidateCaptcha(dto.Captcha, dto.CaptchaId);
                if (!checkGoogleToken)
                    return new ApiResult<ContactUSListViewModel>(false, ApiResultStatusCode.ServerError, null, ErrorMessages.CAPTCHA);

                var model = dto.ToEntity(_mapper);
                await _service.AddAsync(model, cancellationToken);


                return Ok();


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
        [HttpPost("[action]/{id}")]
        public virtual async Task<ApiResult> IsRead(Guid id, CancellationToken cancellationToken)
        {
            try
            {


                var model = await _service.GetByGuidAsync(cancellationToken, id);

                model.StatusContract = StatusContract.Read;
                _service.UpdateAsync(model, cancellationToken);
                return Ok();
            }
            catch (Exception)
            {

                return new ApiResult(false, BaseCore.Utilities.ApiResultStatusCode.LogicError, Resources.ErrorMessages.UnableDeleteItems);

            }
        }

    }
}
