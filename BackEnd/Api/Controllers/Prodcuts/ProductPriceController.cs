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
    public class ProductPriceController : BaseController
    {

        private readonly IRepository<DefinedProductImage> _definedProductImageService;
        private readonly IRepository<ProductPrice> _service;
        private readonly IRepository<Core.Entities.Base.File> _fileservice;
        private IWebHostEnvironment _environment;

        private readonly ILogger<ProductPriceController> _logger;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;


        public ProductPriceController(IMapper mapper,
            IRepository<DefinedProductImage> definedProductImageService,
        IRepository<Core.Entities.Base.File> fileservice,
           IMemoryCache cache, ILogger<ProductPriceController> logger,
            IRepository<ProductPrice> service, IWebHostEnvironment environment)
        {
            _definedProductImageService = definedProductImageService ?? throw new ArgumentNullException(nameof(definedProductImageService));
            _fileservice = fileservice ?? throw new ArgumentNullException(nameof(fileservice));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _environment = environment;
        }

        [HttpGet("{id}")]
        public virtual async Task<ApiResult<ProductPriceViewModel>> Get(Guid id, CancellationToken cancellationToken)
        {
            var dto = await _service.TableNoTracking.ProjectTo<ProductPriceViewModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Guid.Equals(id), cancellationToken);

            if (dto == null)
                return NotFound();

            return dto;
        }
        [AllowAnonymous]
        [HttpGet("[action]/{id}")]
        public virtual async Task<ApiResult<List<ProductPriceViewModel>>> GetByProductId(int id, CancellationToken cancellationToken)
        {
            var dto = await _service.TableNoTracking.Where(p => p.DefinedProductId.Equals(id)).ProjectTo<ProductPriceViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if (dto == null)
                return NotFound();

            return dto;
        }

        [HttpPost("[action]")]
        public virtual async Task<IActionResult> ListBase(DataSourceRequest request, CancellationToken cancellationToken)
        {
            var query = _service.TableNoTracking.Include(x => x.Color).Include(x => x.DefinedProduct).Where(z => z.ProductPriceType == Resources.ProductPriceType.Base);

            return new DataSourceResult<ProductPriceViewModel>(
                               query.ProjectTo<ProductPriceViewModel>(_mapper.ConfigurationProvider),
                               request);
        }
        [HttpPost("[action]")]
        public virtual async Task<IActionResult> ListMomentary(DataSourceRequest request, CancellationToken cancellationToken)
        {
            var query = _service.TableNoTracking.Include(x => x.Color).Include(x => x.DefinedProduct).Where(z => z.ProductPriceType == Resources.ProductPriceType.Momentary);

            return new DataSourceResult<ProductPriceViewModel>(
                               query.ProjectTo<ProductPriceViewModel>(_mapper.ConfigurationProvider),
                               request);
        }
        [HttpPost("[action]")]
        public virtual async Task<IActionResult> ListSuggested(DataSourceRequest request, CancellationToken cancellationToken)
        {
            var query = _service.TableNoTracking.Include(x => x.Color).Include(x => x.DefinedProduct).Where(z => z.ProductPriceType == Resources.ProductPriceType.Suggested);

            return new DataSourceResult<ProductPriceViewModel>(
                               query.ProjectTo<ProductPriceViewModel>(_mapper.ConfigurationProvider),
                               request);
        }


        [AllowAnonymous]
        [HttpGet("[action]")]
        public virtual async Task<ApiResult<List<ProductPriceViewModel>>> ListMomentaryForClient(CancellationToken cancellationToken)
        {
            var lists=new List<ProductPriceViewModel>();
            var query =await _service.TableNoTracking
                .Include(x => x.Color)
                .Include(x => x.DefinedProduct)
                .Where(z => z.ProductPriceType == Resources.ProductPriceType.Momentary).Take(10)
                .ToListAsync(cancellationToken);

            foreach (var item in query.ToList())
            {
                string imageurl ="";

                var image = _definedProductImageService.TableNoTracking.Include(z=>z.Image).Where(z=>z.DefinedProductId==item.DefinedProductId&&z.IsCover).ToList();
                if (image.Count()>0)
                {
                    imageurl = image.FirstOrDefault().Image.Url;
                }
                else
                {
                    var imageNotCover = _definedProductImageService.TableNoTracking.Include(z => z.Image).Where(z => z.DefinedProductId == item.DefinedProductId).ToList();
                    if (imageNotCover.Count()>0)
                    {

                    imageurl = imageNotCover.FirstOrDefault().Image.Url;
                    }

                }
                lists.Add(new ProductPriceViewModel()
                {
                    ColorId = item.ColorId,
                    ColorTitle = item.ColorId.HasValue? item.Color.Title:null,
                    DefinedProductId = item.DefinedProductId,
                    DefinedProductTitle =  item.DefinedProduct.Title,
                    Discount = item.Discount,
                    DiscountPrice = item.DiscountPrice,
                    EndDateTime = item.EndDateTime,
                    Guid = item.Guid,
                    Id = item.Id,
                    ImageUrl = imageurl,
                    Price = item.Price,
                    PriceMain = item.PriceMain,
                    ProductPriceType = item.ProductPriceType,
                    Qty = item.Qty,
                });
            }

            return lists;
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public virtual async Task<ApiResult<List<ProductPriceViewModel>>> ListSuggestedForClient(CancellationToken cancellationToken)
        {
            var date=DateTime.Now.Date;
            var lists = new List<ProductPriceViewModel>();
            var query = await _service.TableNoTracking
                .Include(x => x.Color)
                .Include(x => x.DefinedProduct)
                .Where(z => z.ProductPriceType == Resources.ProductPriceType.Suggested&&z.EndDateTime.HasValue&&z.EndDateTime.Value>= date).Take(10)
                .ToListAsync(cancellationToken);

            foreach (var item in query.ToList())
            {
                string imageurl = "";

                var image = _definedProductImageService.TableNoTracking.Include(z => z.Image).Where(z => z.DefinedProductId == item.DefinedProductId && z.IsCover).ToList();
                if (image.Count() > 0)
                {
                    imageurl = image.FirstOrDefault().Image.Url;
                }
                else
                {
                    var imageNotCover = _definedProductImageService.TableNoTracking.Include(z => z.Image).Where(z => z.DefinedProductId == item.DefinedProductId).ToList();
                    if (imageNotCover.Count() > 0)
                    {

                        imageurl = imageNotCover.FirstOrDefault().Image.Url;
                    }

                }
                lists.Add(new ProductPriceViewModel()
                {
                    ColorId = item.ColorId,
                    ColorTitle = item.Color.Title,
                    DefinedProductId = item.DefinedProductId,
                    DefinedProductTitle = item.DefinedProduct.Title,
                    Discount = item.Discount,
                    DiscountPrice = item.DiscountPrice,
                    EndDateTime = item.EndDateTime,
                    Guid = item.Guid,
                    Id = item.Id,
                    ImageUrl = imageurl,
                    Price = item.Price,
                    PriceMain = item.PriceMain,
                    ProductPriceType = item.ProductPriceType,
                    Qty = item.Qty,
                });
            }

            return lists;
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public virtual async Task<ApiResult<List<ProductPriceViewModel>>> ListBaseForClient(CancellationToken cancellationToken)
        {
            var lists = new List<ProductPriceViewModel>();
            var query = await _service.TableNoTracking
                .Include(x => x.Color)
                .Include(x => x.DefinedProduct)
                .Where(z => z.ProductPriceType == Resources.ProductPriceType.Base).Take(10)
                .ToListAsync(cancellationToken);

            foreach (var item in query.ToList())
            {
                string imageurl = "";

                var image = _definedProductImageService.TableNoTracking.Include(z => z.Image).Where(z => z.DefinedProductId == item.DefinedProductId && z.IsCover).ToList();
                if (image.Count() > 0)
                {
                    imageurl = image.FirstOrDefault().Image.Url;
                }
                else
                {
                    var imageNotCover = _definedProductImageService.TableNoTracking.Include(z => z.Image).Where(z => z.DefinedProductId == item.DefinedProductId).ToList();
                    if (imageNotCover.Count() > 0)
                    {

                        imageurl = imageNotCover.FirstOrDefault().Image.Url;
                    }

                }
                lists.Add(new ProductPriceViewModel()
                {
                    ColorId = item.ColorId,
                    ColorTitle = item.Color.Title,
                    DefinedProductId = item.DefinedProductId,
                    DefinedProductTitle = item.DefinedProduct.Title,
                    Discount = item.Discount,
                    DiscountPrice = item.DiscountPrice,
                    EndDateTime = item.EndDateTime,
                    Guid = item.Guid,
                    Id = item.Id,
                    ImageUrl = imageurl,
                    Price = item.Price,
                    PriceMain = item.PriceMain,
                    ProductPriceType = item.ProductPriceType,
                    Qty = item.Qty,
                });
            }

            return lists;
        }



        [AllowAnonymous]
        [HttpGet("[action]")]
        public virtual async Task<ApiResult<List<ProductPriceViewModel>>> ListBaseFactorForClient(CancellationToken cancellationToken)
        {
            var lists = new List<ProductPriceViewModel>();
            var query = await _service.TableNoTracking
                .Include(x => x.Color)
                .Include(x => x.DefinedProduct)
                .Where(z => z.ProductPriceType == Resources.ProductPriceType.Base).Take(10)
                .ToListAsync(cancellationToken);

            foreach (var item in query.ToList())
            {
                string imageurl = "";

                var image = _definedProductImageService.TableNoTracking.Include(z => z.Image).Where(z => z.DefinedProductId == item.DefinedProductId && z.IsCover).ToList();
                if (image.Count() > 0)
                {
                    imageurl = image.FirstOrDefault().Image.Url;
                }
                else
                {
                    var imageNotCover = _definedProductImageService.TableNoTracking.Include(z => z.Image).Where(z => z.DefinedProductId == item.DefinedProductId).ToList();
                    if (imageNotCover.Count() > 0)
                    {

                        imageurl = imageNotCover.FirstOrDefault().Image.Url;
                    }

                }
                lists.Add(new ProductPriceViewModel()
                {
                    ColorId = item.ColorId,
                    ColorTitle = item.Color.Title,
                    DefinedProductId = item.DefinedProductId,
                    DefinedProductTitle = item.DefinedProduct.Title,
                    Discount = item.Discount,
                    DiscountPrice = item.DiscountPrice,
                    EndDateTime = item.EndDateTime,
                    Guid = item.Guid,
                    Id = item.Id,
                    ImageUrl = imageurl,
                    Price = item.Price,
                    PriceMain = item.PriceMain,
                    ProductPriceType = item.ProductPriceType,
                    Qty = item.Qty,
                });
            }

            return lists;
        }




        [HttpPost("[action]")]

        public async Task<ApiResult> Update(ProductPriceViewModel dto, CancellationToken cancellationToken)
        {
            try
            {

                var obj = _service.GetById(dto.Id);
                if (obj != null)
                {
                    var model = dto.ToEntity(_mapper, obj);
                    await _service.UpdateAsync(model, cancellationToken);

                    return Ok();
                }
                return new ApiResult<ProductPriceViewModel>(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, null, Resources.ErrorMessages.NotFound);


            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return BadRequest();

            }
        }

        [HttpPost()]
        public async Task<ApiResult> Create(ProductPriceViewModel dto, CancellationToken cancellationToken)
        {
            try
            {
                await DeleteLast(cancellationToken);
                var check =await _service.Table.Where(z => z.DefinedProductId == dto.DefinedProductId && z.ProductPriceType == dto.ProductPriceType&&z.ColorId==dto.ColorId).ToListAsync(cancellationToken);
                if (check.Count()>0)
                {
                 await   DeletebyDefinedId(check.ToList(), cancellationToken);
                }

                var model = dto.ToEntity(_mapper);
                await _service.AddAsync(model, cancellationToken);

               

                return Ok();


            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return BadRequest();

            }
        }



        [HttpPost("[action]/{id}")]
        public virtual async Task<ApiResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var model = await _service.GetByGuidAsync(cancellationToken, id);
                model.IsSoftDeleted = true;
                await _service.UpdateAsync(model, cancellationToken);
                return Ok();
            }
            catch (Exception)
            {

                return new ApiResult(false, BaseCore.Utilities.ApiResultStatusCode.LogicError, Resources.ErrorMessages.UnableDeleteItems);

            }
        }

        public virtual async Task DeletebyDefinedId(List<ProductPrice> list, CancellationToken cancellationToken)
        {
            foreach (var item in list)
            {

                item.IsSoftDeleted = true;
                await _service.UpdateAsync(item, cancellationToken);
            }
        }
        public virtual async Task DeleteLast(CancellationToken cancellationToken)
        {
            var date = DateTime.Now.AddMonths(-2);
            var model = await _service.Table.Where(z => z.Created < date).ToListAsync(cancellationToken);
            await _service.DeleteRangeAsync(model, cancellationToken);
        }

    }
}
