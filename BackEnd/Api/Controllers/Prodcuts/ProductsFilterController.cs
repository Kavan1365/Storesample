using Api.Configuration;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseCore.Configuration;
using BaseCore.ViewModel;
using Core.Entities.Prodcutes;
using Core.Repositories.Base;
using Infrastructure.Repository.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViewModels.Prodcuts;

namespace Api.Controllers.Prodcuts
{
    public class ProductsFilterController : CrudController<ProductsFilterViewModel, ProductsFilterViewModelCreate, ProductsFilter>
    {
        private readonly IRepository<ProductPropertyValue> _productPropertyValue;
        private readonly IRepository<SubFilter> _subFilter;
        private readonly IRepository<ProductProperty> _productProperty;
        public ProductsFilterController(IRepository<ProductsFilter> repository, IRepository<ProductPropertyValue> productPropertyValue, IRepository<ProductProperty> productProperty, IRepository<SubFilter> subFilter, IMapper mapper)
            : base(repository, mapper)
        {
            _productPropertyValue = productPropertyValue;
            _productProperty = productProperty;
            _subFilter = subFilter;
        }
        [HttpPost("[action]/{productid}/{productpropertyid}")]
        public IActionResult GetSubfilter(int productid, int productpropertyid, DataSourceRequest request)
        {
            var query = _subFilter.TableNoTracking.Where(z => z.ProductPropertyId == productpropertyid);
            return new DataSourceResult<SubFilterViewModel>(
                               query.ProjectTo<SubFilterViewModel>(Mapper.ConfigurationProvider),
                               request);
        }

        [HttpPost("[action]")]
        public virtual async Task<ApiResult<ProductsFilterViewModelCreate>> CreateCustome(ProductsFilterViewModelCreate dto, CancellationToken cancellationToken)
        {

            var check = await Repository.Table.AnyAsync(z => z.ProductId == dto.ProductId && z.ProductPropertyId == dto.ProductPropertyId, cancellationToken);
            if (check)
                return new ApiResult<ProductsFilterViewModelCreate>(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, null, Resources.ErrorMessages.IsInDataCreate);
            var model = dto.ToEntity(Mapper);

            await Repository.AddAsync(model, cancellationToken);

            var resultDto = await Repository.TableNoTracking.ProjectTo<ProductsFilterViewModelCreate>(Mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Id.Equals(model.Id), cancellationToken);

            return resultDto;
        }


        [HttpPost("[action]/{id}")]
        public IActionResult ListByProductId(int id, DataSourceRequest request, CancellationToken cancellationToken)
        {
            var query = Repository.TableNoTracking
                .Where(x => x.ProductId == id)
                .Include(x => x.ProductProperty);

            return new DataSourceResult<ProductsFilterViewModel>(
                               query.ProjectTo<ProductsFilterViewModel>(Mapper.ConfigurationProvider),
                               request);
        }

        [HttpGet("[action]/{id}/{defineId?}")]
        public async Task<ApiResult<List<ProductPropertyWithFiltersViewModel>>> ProdcutPropertyListByProductId(int id, int? defineId, CancellationToken cancellationToken)
        {
            var query = await Repository.TableNoTracking
                .Where(x => x.ProductId == id)
                .Include(z => z.ProductProperty)
                .ToListAsync(cancellationToken);
            var SubFilter = query.Distinct();
            var productProperty = (await _productProperty.TableNoTracking
                .Include(z => z.SubFilters)
                .ToListAsync(cancellationToken))
                .Where(c => SubFilter.Any(z => z.Id == c.Id)).ToList();


            var getproperty1 = productProperty
                .Select(c => new ProductPropertyWithFiltersViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    Selected = defineId.HasValue ? ProductPropertyValues(defineId.Value, c.SubFilters.Select(o => o.Id).ToList()) : null
                }).ToList();

            return getproperty1;
        }
        private List<int> ProductPropertyValues(int id, List<int> subfilters)
        {
            if (subfilters.Count > 0)
            {
                var query = _productPropertyValue.TableNoTracking.Include(p => p.SubFilter)
                               .Where(z=>z.DefinedProductId==id&& subfilters.Any(k=>k==z.SubFilterId))
                               .Select(z => z.SubFilterId)
                               .ToList();
                return query;
            }
            return null;
        }


    }
}
