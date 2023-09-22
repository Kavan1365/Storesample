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
    public class CategoryController : BaseController
    {

        private readonly IRepository<Category> _service;
        private readonly IRepository<Core.Entities.Base.File> _fileservice;
        private IWebHostEnvironment _environment;

        private readonly ILogger<CategoryController> _logger;
        private readonly IMapper _mapper;


        public CategoryController(IMapper mapper,
           ILogger<CategoryController> logger,
           IWebHostEnvironment environment,
           IRepository<Core.Entities.Base.File> fileservice,
            IRepository<Category> service)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _fileservice = fileservice ?? throw new ArgumentNullException(nameof(fileservice));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet("{id}")]
        public virtual async Task<ApiResult<CategoryViewModelCreate>> Get(Guid id, CancellationToken cancellationToken)
        {
            var dto = await _service.TableNoTracking.ProjectTo<CategoryViewModelCreate>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Guid.Equals(id), cancellationToken);

            if (dto == null)
                return NotFound();

            return dto;
        }
       
      
       
        [HttpPost("[action]")]
        public virtual async Task<IActionResult> List(DataSourceRequestTree request, CancellationToken cancellationToken)
        {
            var query = _service.TableNoTracking.Include(x=>x.Logo);

            return new DataSourceResultTree<CategoryViewModelCreate>(
                               query.ProjectTo<CategoryViewModelCreate>(_mapper.ConfigurationProvider),
                               request, _mapper,true);

        }
       

        [HttpGet("[action]/{type?}/{take?}")]
        [AllowAnonymous]
        public virtual async Task<ApiResult<List<CategoryViewModel>>> CategoryBaseList(int? take, CancellationToken cancellationToken)
        {
            var obj = _service.TableNoTracking.Include(z=>z.Logo).Where(x => !x.ParentId.HasValue);
            var dto = await obj.OrderBy(z => z.Id)
                .Select(z => new CategoryViewModel
                {
                    Title = z.Title,
                    Id = z.Id,
                    FontIcon = z.FontIcon,
                    imageurl = (z.ImageId.HasValue ? z.Logo.Url : "")
                })
                 .ToListAsync(cancellationToken);
            if (take.HasValue)
            {
                dto = dto.Take(take.Value).ToList();
            }
            return dto;
        }
        [HttpGet("[action]")]
        [AllowAnonymous]
        public virtual async Task<ApiResult<IEnumerable>> CategoryAllList(CancellationToken cancellationToken)
        {
            var obj = await _service.TableNoTracking
                .Select(z => new { Title = z.Title, Id = z.Id})
                .ToListAsync(cancellationToken);
            return obj;
        }

        [HttpPost("[action]")]

        public async Task<ApiResult<CategoryViewModelCreate>> Update(CategoryViewModelCreate dto, CancellationToken cancellationToken)
        {
            try
            {

                var obj = _service.GetById(dto.Id);
                if (obj != null)
                {
                    dto.Guid = obj.Guid;
                    var checkTitle = _service.TableNoTracking.Any(z => z.Title == dto.Title && z.Guid != dto.Guid);
                    if (checkTitle)
                        return new ApiResult<CategoryViewModelCreate>(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, null, Resources.ErrorMessages.TitleIsExists);

                    var model = dto.ToEntity(_mapper, obj);

                    await _service.UpdateAsync(model, cancellationToken);

                    var resultDto = await _service.TableNoTracking.ProjectTo<CategoryViewModelCreate>(_mapper.ConfigurationProvider)
                        .SingleOrDefaultAsync(p => p.Id.Equals(model.Id), cancellationToken);

                    return resultDto;
                }
                return new ApiResult<CategoryViewModelCreate>(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, null,Resources.ErrorMessages.NotFound);


            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return null;

            }
        }
        [HttpPost()]
        public async Task<ApiResult<CategoryViewModelCreate>> Create(CategoryViewModelCreate dto, CancellationToken cancellationToken)
        {
            try
            {
                var checkTitle = _service.TableNoTracking.Any(z => z.Title == dto.Title);
                if (checkTitle)
                    return new ApiResult<CategoryViewModelCreate>(false,BaseCore.Utilities.ApiResultStatusCode.BadRequest,null,Resources.ErrorMessages.TitleIsExists);


                var model = dto.ToEntity(_mapper);

              
                await _service.AddAsync(model, cancellationToken);

                var resultDto = await _service.TableNoTracking.ProjectTo<CategoryViewModelCreate>(_mapper.ConfigurationProvider)
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
                var file=   await _fileservice.GetByIdAsync(cancellationToken,fileid.Value);
                await _fileservice.DeleteAsync(file, cancellationToken);

            }

            return Ok();
            }
            catch (Exception)
            {

                return new ApiResult(false, BaseCore.Utilities.ApiResultStatusCode.LogicError, Resources.ErrorMessages.UnableDeleteItems);

            }
        }




        #region Client


        [HttpGet("[action]")]
        [AllowAnonymous]
        public ApiResult<List<CategoryViewModel>> GetAll()
        {
            var obj = _service.TableNoTracking
                .Where(z => !z.ParentId.HasValue)
                .Include(z => z.Logo)
                .OrderBy(z=>z.Order)
                .ToList();
            var dto = obj
                 .Select(z => new CategoryViewModel
                 {
                     Title = z.Title,
                     FontIcon = z.FontIcon,
                     imageurl = z.ImageId.HasValue ? z.Logo.Url : null,
                     Categories = GetByParentId(z.Id)
                 }).ToList();

            if (dto == null)
                return NotFound();

            return dto;
        }
        private List<CategoryViewModel> GetByParentId(int id)
        {

            var dto = _service.TableNoTracking
                .Where(z => z.ParentId.HasValue && z.ParentId == id)
                .Include(z => z.Logo)
                .OrderBy(z=>z.Order)
                .ToList()
                .Select(z => new CategoryViewModel
                {
                    Title = z.Title,
                    FontIcon = z.FontIcon,
                    imageurl = z.ImageId.HasValue ? z.Logo.Url : null,
                    Categories = GetByParentId(z.Id)
                })
                .ToList();

            return dto;
        }


        [HttpGet("[action]/{ParentId?}")]
        [AllowAnonymous]
        public virtual async Task<ApiResult<List<CategoryViewModel>>> GetListByParentId(int? ParentId, CancellationToken cancellationToken)
        {

            var category = new List<CategoryViewModel>();

            category = await _service.TableNoTracking
             .Select(c => new CategoryViewModel { Title = c.Title, ParentId = c.ParentId, Id = c.Id, Guid = c.Guid, FontIcon = c.FontIcon, IsShowHome = c.IsShowHome })
             .ToListAsync(cancellationToken);


            if (!ParentId.HasValue)
            {
                var obj = category.OrderBy(z => z.Id).Where(x => !x.ParentId.HasValue )
          .Select(c => new CategoryViewModel { Title = c.Title, ParentId = c.ParentId, Id = c.Id, Guid = c.Guid, FontIcon = c.FontIcon, IsShowHome = c.IsShowHome })
            .ToList();
                return obj;
            }
            if (!ParentId.HasValue && ParentId.Value == 0)
            {
                var obj = category.OrderBy(z => z.Id).Where(x => !x.ParentId.HasValue)
                         .Select(c => new CategoryViewModel { Title = c.Title, ParentId = c.ParentId, Id = c.Id, Guid = c.Guid, FontIcon = c.FontIcon, IsShowHome = c.IsShowHome })
                           .ToList();
                return obj;
            }
            else
            {
                var obj = category.OrderBy(z => z.Id).Where(x => x.ParentId == ParentId)
          .Select(c => new CategoryViewModel { Title = c.Title, ParentId = c.ParentId, Id = c.Id, Guid = c.Guid, FontIcon = c.FontIcon, IsShowHome = c.IsShowHome })
          .ToList();
                return obj;
            }


        }




        [AllowAnonymous]
        [HttpGet("[action]/{id}")]
        public virtual async Task<ApiResult<string>> GetParent(int id, CancellationToken cancellationToken = default)
        {
            var data = await _service.GetByIdAsync(cancellationToken, id);
            if (data == null)
                return new ApiResult<string>(false, BaseCore.Utilities.ApiResultStatusCode.Success, "0");
            return new ApiResult<string>(true, BaseCore.Utilities.ApiResultStatusCode.Success, (data.ParentId.HasValue ? data.ParentId.Value + "" : "0"));

        }
  
        [HttpGet("[action]/{id}")]
        [AllowAnonymous]
        public virtual async Task<ApiResult> GetCategoryTitle(int id, CancellationToken cancellationToken = default)
        {

            var category = await _service.GetByIdAsync(cancellationToken, id);
            if (category == null)
                return new ApiResult(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, "");

            return new ApiResult(true, BaseCore.Utilities.ApiResultStatusCode.Success, category.Title);
        }




        /////service///////



        

        #endregion

    }
}