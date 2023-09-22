using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseCore.Configuration;
using BaseCore.ViewModel;
using Core.Entities.Base;
using Core.Entities.Prodcutes;
using Core.Repositories.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using ViewModels.Prodcuts;

namespace Api.Controllers.Prodcuts
{
    public class ProductController : BaseController
    {

        private readonly IRepository<Product> _service;
        private readonly IRepository<Core.Entities.Base.File> _fileservice;
        private IWebHostEnvironment _environment;

        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;


        public ProductController(IMapper mapper,
           ILogger<ProductController> logger,
           IWebHostEnvironment environment,
           IRepository<Core.Entities.Base.File> fileservice,
            IRepository<Product> service)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _fileservice = fileservice ?? throw new ArgumentNullException(nameof(fileservice));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet("{id}")]
        public virtual async Task<ApiResult<ProdcutViewModel>> Get(Guid id, CancellationToken cancellationToken)
        {
            var dto = await _service.TableNoTracking.ProjectTo<ProdcutViewModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Guid.Equals(id), cancellationToken);

            if (dto == null)
                return NotFound();

            return dto;
        }


        [HttpPost("[action]")]
        public virtual async Task<IActionResult> List(DataSourceRequest request, CancellationToken cancellationToken)
        {
            var query = _service.TableNoTracking;

            return new DataSourceResult<ProdcutViewModel>(
                               query.ProjectTo<ProdcutViewModel>(_mapper.ConfigurationProvider),
                               request);

        }

        [AllowAnonymous]
        [HttpGet("[action]/{id}")]
        public virtual async Task<ApiResult<IEnumerable>> ListByCategory(int id, CancellationToken cancellationToken)
        {
            var query =await _service.TableNoTracking.Include(z => z.Categories).Where(z => z.Categories.Any(k => k.CategoryId == id)).Select(z=>new {Id=z.Id,Title=z.Title}).ToListAsync(cancellationToken);
            return query;
        }



        [HttpPost("[action]")]

        public async Task<ApiResult<ProdcutViewModel>> Update(ProdcutViewModel dto, CancellationToken cancellationToken)
        {
            try
            {

                var user = _service.GetById(dto.Id);
                if (user != null)
                {
                    var checkTitle = _service.TableNoTracking.Any(z => z.Title == dto.Title && z.Guid != dto.Guid);
                    if (checkTitle)
                        return new ApiResult<ProdcutViewModel>(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, null, Resources.ErrorMessages.TitleIsExists);
                    dto.Guid = user.Guid;

                    var model = dto.ToEntity(_mapper, user);

                    await _service.UpdateAsync(model, cancellationToken);

                    var resultDto = await _service.TableNoTracking.ProjectTo<ProdcutViewModel>(_mapper.ConfigurationProvider)
                        .SingleOrDefaultAsync(p => p.Id.Equals(model.Id), cancellationToken);

                    return resultDto;
                }
                return new ApiResult<ProdcutViewModel>(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, null, " کاربر گرامی اکانت شما قابل شناسایی نیست");


            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return null;

            }
        }
        [HttpPost()]
        public async Task<ApiResult<ProdcutViewModel>> Create(ProdcutViewModel dto, CancellationToken cancellationToken)
        {
            try
            {
                var checkTitle = _service.TableNoTracking.Any(z => z.Title == dto.Title);
                if (checkTitle)
                    return new ApiResult<ProdcutViewModel>(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, null, Resources.ErrorMessages.TitleIsExists);


                var model = dto.ToEntity(_mapper);


                await _service.AddAsync(model, cancellationToken);

                var resultDto = await _service.TableNoTracking.ProjectTo<ProdcutViewModel>(_mapper.ConfigurationProvider)
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