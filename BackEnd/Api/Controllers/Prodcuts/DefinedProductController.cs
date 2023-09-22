using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseCore.Configuration;
using BaseCore.Dapper;
using BaseCore.Utilities;
using BaseCore.ViewModel;
using Core.Entities.Prodcutes;
using Core.Repositories.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Resources;
using System.Drawing.Imaging;
using ViewModels.Prodcuts;

namespace Api.Controllers.Prodcuts
{
    public class DefinedProductController : BaseController
    {

        //private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRepository<Core.Entities.Base.File> _fileService;
        private readonly IRepository<ProductPropertyValue> _productPropertyValueService;
        private readonly IRepository<DefinedProductImage> _definedProductImageservice;
        private readonly IRepository<CustomerReview> _customerReviewservice;
        private readonly IRepository<DefinedProduct> _service;
        private readonly IRepository<ProductsCategory> _productsCategoryService;
        private readonly IRepository<ProductProperty> _productPropertyService;
        private readonly IRepository<Product> _productService;
        private readonly IRepository<Brand> _brandService;
        private readonly IRepository<SubFilter> _subFilterService;
        private readonly IRepository<ProductsFilter> _ProductsFilterService;
        private readonly ILogger<DefinedProductController> _logger;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public DefinedProductController(IMapper mapper,
            //IBackgroundJobClient backgroundJobClient,
         IWebHostEnvironment environment,

        IRepository<Core.Entities.Base.File> fileService,
            IRepository<Product> productService,
           IRepository<ProductProperty> productPropertyService,
           IRepository<SubFilter> subFilterService,
           IRepository<ProductsCategory> productsCategoryService,
           IRepository<DefinedProductImage> definedProductImageservice,
           IRepository<Brand> brandService,
           IRepository<ProductsFilter> ProductsFilterService,
             IRepository<CustomerReview> customerReviewservice,
             IRepository<ProductPropertyValue> productPropertyValueService
            , ILogger<DefinedProductController> logger,
                 IRepository<DefinedProduct> service)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _customerReviewservice = customerReviewservice ?? throw new ArgumentNullException(nameof(customerReviewservice));
            _subFilterService = subFilterService ?? throw new ArgumentNullException(nameof(subFilterService));
            _ProductsFilterService = ProductsFilterService ?? throw new ArgumentNullException(nameof(ProductsFilterService));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _productPropertyService = productPropertyService ?? throw new ArgumentNullException(nameof(productPropertyService));
            _brandService = brandService ?? throw new ArgumentNullException(nameof(brandService));
            _productsCategoryService = productsCategoryService ?? throw new ArgumentNullException(nameof(productsCategoryService));
            //_backgroundJobClient = backgroundJobClient ?? throw new ArgumentNullException(nameof(backgroundJobClient));
            _productPropertyValueService = productPropertyValueService ?? throw new ArgumentNullException(nameof(productPropertyValueService));
            _definedProductImageservice = definedProductImageservice ?? throw new ArgumentNullException(nameof(definedProductImageservice));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _environment = environment;
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet("{id}")]
        public virtual async Task<ApiResult<AddNewDefinedProductViewModel>> Get(Guid id, CancellationToken cancellationToken)
        {
            var dto = await _service.TableNoTracking.ProjectTo<AddNewDefinedProductViewModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Guid.Equals(id), cancellationToken);

            if (dto == null)
                return NotFound();

            return dto;
        }




        [HttpPost("[action]/{ProductId?}/{BarndId?}")]
        public virtual async Task<IActionResult> List(int? ProductId, int? BarndId, DataSourceRequest request, CancellationToken cancellationToken)
        {
            var query = _service.TableNoTracking;
            if (ProductId.HasValue)
                query = query.Where(x => x.ProductId == ProductId.Value);
            if (BarndId.HasValue)
                query = query.Where(x => x.BrandId == BarndId.Value);

            return new DataSourceResult<DefinedProductViewModel>(
                               query.Include(z => z.Product)
                               .Include(z => z.DefinedProductImages)
                               .Include(z => z.Brand)
                               .ProjectTo<DefinedProductViewModel>(_mapper.ConfigurationProvider),
                               request);

        }

        [HttpPost()]
        public async Task<ApiResult<AddNewDefinedProductViewModel>> CreateAsync(AddNewDefinedProductViewModel dto, CancellationToken cancellationToken)
        {
            var checkTitle = _service.TableNoTracking.Any(z => z.Title == dto.Title);
            if (checkTitle)
                return new ApiResult<AddNewDefinedProductViewModel>(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, null, Resources.ErrorMessages.IsNotUniqTitle);


            var model = dto.ToEntity(_mapper);
            model.Status = StatusNames.Enable;


            await _service.AddAsync(model, cancellationToken);

            //var file = new File();
            int? imageId = null;
            if (dto.FileId.HasValue)
            {
                var deimage = new DefinedProductImage() { DefinedProductId = model.Id, ImageId = imageId.Value };
                await _definedProductImageservice.AddAsync(deimage, cancellationToken);
            }


            if (dto.Filters != null)
                foreach (var item in dto.Filters)
                    await _productPropertyValueService.AddAsync(new ProductPropertyValue { SubFilterId = item, DefinedProductId = model.Id }, cancellationToken);
            return new ApiResult<AddNewDefinedProductViewModel>(true, BaseCore.Utilities.ApiResultStatusCode.Success, new AddNewDefinedProductViewModel() { Id = model.Id });
        }
        [HttpPost("[action]")]
        public async Task<ApiResult<AddNewDefinedProductViewModel>> Update(AddNewDefinedProductViewModel dto, CancellationToken cancellationToken)
        {


            try
            {

                var obj = _service.GetById(dto.Id);
               
                if (obj != null)
                {
                    dto.Guid = obj.Guid;
                    var checkTitle = _service.TableNoTracking.Any(z => z.Title == dto.Title && z.Guid != dto.Guid);
                    if (checkTitle)
                        return new ApiResult<AddNewDefinedProductViewModel>(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, null, Resources.ErrorMessages.TitleIsExists);

                    var model = dto.ToEntity(_mapper, obj);
                    await _service.UpdateAsync(model, cancellationToken);

                    var deleteproperties =await _productPropertyValueService.Table.Where(z => z.DefinedProductId == obj.Id).ToListAsync(cancellationToken);
                    if (deleteproperties.Count>0)
                    {

                       await _productPropertyValueService.DeleteRangeAsync(deleteproperties,cancellationToken);
                    }

                    if (dto.Filters != null)
                        foreach (var item in dto.Filters)
                            await _productPropertyValueService.AddAsync(new ProductPropertyValue { SubFilterId = item, DefinedProductId = model.Id }, cancellationToken);

                    var resultDto = await _service.TableNoTracking.ProjectTo<AddNewDefinedProductViewModel>(_mapper.ConfigurationProvider)
                        .SingleOrDefaultAsync(p => p.Id.Equals(model.Id), cancellationToken);

                    return resultDto;
                }
                return new ApiResult<AddNewDefinedProductViewModel>(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, null, Resources.ErrorMessages.NotFound);


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
            var obj = await _service.GetByGuidAsync(cancellationToken, id);

            try
            {
                var objPropertyValues = await _productPropertyValueService.Table.Where(ppv => ppv.DefinedProductId == obj.Id).ToListAsync(cancellationToken);
                foreach (var propertyValue in objPropertyValues)
                {
                    await _productPropertyValueService.DeleteAsync(propertyValue, cancellationToken);
                }
                var objimage = await _definedProductImageservice.Table.Where(ppv => ppv.DefinedProductId == obj.Id).ToListAsync(cancellationToken);
                foreach (var item in objimage)
                {
                    var getFile = _fileService.GetById(item.ImageId);
                    var root = _environment.WebRootPath;
                    try
                    {
                        await _fileService.DeleteAsync(getFile, cancellationToken);
                        var fullpath = root + "\\" + "files" + "\\" + getFile.Url.Replace("/", "\\");
                        if (System.IO.File.Exists(fullpath))
                            System.IO.File.Delete(fullpath);

                    }
                    catch (Exception)
                    {

                    }
                    await _definedProductImageservice.DeleteAsync(item, cancellationToken);

                }
                await _service.DeleteAsync(obj, cancellationToken);
                var ids = new List<int>();
                return Ok();

            }
            catch
            {



                obj.Title = null;
                obj.IsSoftDeleted = true;
                await _service.UpdateAsync(obj, cancellationToken);
                var ids = new List<int>();
                return Ok();
            }

        }





        #region Image

        [HttpPost("[action]/{definedProductId}")]
        public virtual async Task<IActionResult> ListImage(int definedProductId, DataSourceRequest request, CancellationToken cancellationToken)
        {
            var query = _definedProductImageservice.TableNoTracking.Where(x => x.DefinedProductId == definedProductId);

            return new DataSourceResult<DefinedProductImagesViewModelList>(
                               query.Include(z => z.Image)
                               .ProjectTo<DefinedProductImagesViewModelList>(_mapper.ConfigurationProvider),
                               request);
        }
        [HttpPost("[action]/{definedProductId}")]
        public virtual async Task<ApiResult<List<DefinedProductImagesViewModelList>>> ListImageSelected(int definedProductId, DataSourceRequest request, CancellationToken cancellationToken)
        {
            var query = await _definedProductImageservice.TableNoTracking
                .Where(x => x.DefinedProductId == definedProductId)
                  .Include(z => z.Image)
                               .ProjectTo<DefinedProductImagesViewModelList>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
            return query;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ApiResult<DefinedProductImagesViewModel>> DefinedProductImages(DefinedProductImagesViewModel model, CancellationToken cancellationToken)
        {
            try
            {
                if (model.ImageId < 1)
                {
                    return new ApiResult<DefinedProductImagesViewModel>(false, ApiResultStatusCode.BadRequest, null, Resources.ErrorMessages.imageValid);

                }
                var deimage = new DefinedProductImage() { DefinedProductId = model.DefinedProductId, ImageId = model.ImageId};
                await _definedProductImageservice.AddAsync(deimage, cancellationToken);
                return Ok();
            }
            catch (Exception e)
            {

                _logger.LogError(e.Message);
                throw;
            }
        }


        [HttpPost("[action]")]
        public async Task<ApiResult<DefinedProductImagesViewModel>> UpdateDefinedProductImages(UpdateInlineViewModel<DefinedProductImagesViewModelList> model, CancellationToken cancellationToken)
        {
            try
            {
                foreach (var item in model.models)
                {
                    var obj = _definedProductImageservice.GetByGuid(item.Guid);
                    obj.IsCover = item.IsCover;
                    await _definedProductImageservice.UpdateAsync(obj, cancellationToken);
                }

                return Ok();
            }
            catch (Exception e)
            {

                _logger.LogError(e.Message);
                throw;
            }

        }
        [HttpPost("[action]/{id}")]
        public virtual async Task<ApiResult> DeleteImage(Guid id, CancellationToken cancellationToken)
        {
            var obj = await _definedProductImageservice.GetByGuidAsync(cancellationToken, id);

            try
            {
                var getFile = _fileService.GetById(obj.ImageId);
                var root = _environment.WebRootPath;
                await _definedProductImageservice.DeleteAsync(obj, cancellationToken);

                try
                {
                    await _fileService.DeleteAsync(getFile, cancellationToken);
                    var fullpath = root + "\\" + "files" + "\\" + getFile.Url.Replace("/", "\\");
                    if (System.IO.File.Exists(fullpath))
                        System.IO.File.Delete(fullpath);

                }
                catch (Exception)
                {

                }

                return Ok();

            }
            catch
            {
                return new ApiResult(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, Resources.ErrorMessages.UnableDeleteItems);
            }

        }
        #endregion

    }
}
