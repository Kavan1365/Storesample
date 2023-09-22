using Api.Configuration;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseCore.Configuration;
using BaseCore.Utilities;
using BaseCore.ViewModel;
using Core.Entities.Prodcutes;
using Core.Repositories.Base;
using Infrastructure.Repository.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViewModels.Prodcuts;

namespace Api.Controllers.Prodcuts
{
    public class ProductsCategoryController : CrudController<ProductsCategoryViewModel, ProductsCategoryViewModel, ProductsCategory>
    {
        private readonly IRepository<DefinedProduct> DefinedProduct;
        private readonly IRepository<ProductsFilter> ProductsFilter;
        private readonly IRepository<Product> Product;
        public ProductsCategoryController(IRepository<ProductsCategory> repository,IRepository<ProductsFilter> productsFilter, IRepository<Product> product, IRepository<DefinedProduct> definedProduct, IMapper mapper)
            : base(repository, mapper)
        {
            ProductsFilter = productsFilter;
            Product = product;
            DefinedProduct = definedProduct;
        }

        [HttpPost("[action]/{id}")]
        public IActionResult ListByCategoryId(int id, DataSourceRequest request, CancellationToken cancellationToken)
        {
            var query = Repository.TableNoTracking
                .Include(x => x.Product)
                .Where(x => x.CategoryId == id).Select(x => x.Product);

            return new DataSourceResult<ProductViewModel>(
                               query.ProjectTo<ProductViewModel>(Mapper.ConfigurationProvider),
                               request);
        }

        [HttpDelete("[action]/{id}/{categoryId}")]
        public async Task<ApiResult> Delete(Guid id, int categoryId, CancellationToken cancellationToken)
        {
            var model = await Repository.Table.Where(x => x.Product.Guid == id && x.CategoryId == categoryId).SingleOrDefaultAsync(cancellationToken);
            await Repository.DeleteAsync(model, cancellationToken);

            return Ok();
        }

        [HttpPost("[action]/{id}")]
        public async Task<ApiResult> Deletecustom(Guid id, int categoryId, CancellationToken cancellationToken)
        {
            var model = await Repository.Table.Where(x => x.Guid == id).SingleOrDefaultAsync(cancellationToken);
            await Repository.DeleteAsync(model, cancellationToken);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> MigrationProduct(MigrationProductViewModel model, CancellationToken cancellationToken)
        {
            int[] ProductIds = model.ProductIds.Where(z => !string.IsNullOrEmpty(z)).ToList().Select(z => int.Parse(z)).ToArray();
            if (model.ProductIds.Length > 0)
            {
                var getDefine = await DefinedProduct.TableNoTracking
                    .Where(x => ProductIds.Any(z => z == x.ProductId) && x.ProductId != model.ProductId)
                    .ToListAsync(cancellationToken);


                foreach (var definedProduct in getDefine)
                {
                    var obj = DefinedProduct.GetById(definedProduct.Id);
                    obj.ProductId = model.ProductId;
                    await DefinedProduct.UpdateAsync(obj, cancellationToken);
                }

                var productes = await Product.TableNoTracking.Where(x => ProductIds.Any(z => z == x.Id) && x.Id != model.ProductId).ToListAsync(cancellationToken); ;

                foreach (var producte in productes)
                {

                    var productcate = await Repository.Table.Where(z => z.ProductId == producte.Id).ToListAsync(cancellationToken);
                    foreach (var productsCategory in productcate)
                    {
                        var objproductcate = Repository.GetById(productsCategory.Id);
                        Repository.Delete(objproductcate);
                    }


                    var productsFilters = await ProductsFilter.Table.Where(z => z.ProductId == producte.Id).ToListAsync(cancellationToken);
                    foreach (var productsFilter in productsFilters)
                    {
                        var filter = ProductsFilter.GetById(productsFilter.Id);
                        filter.ProductId = model.ProductId;
                        await ProductsFilter.UpdateAsync(filter, cancellationToken);
                    }
                    var definedproducts = await DefinedProduct.Table.Where(z => z.ProductId == producte.Id).ToListAsync(cancellationToken);
                    if (definedproducts.Count > 0)
                    {
                        var product = Product.GetById(producte.Id);
                        product.IsSoftDeleted = true;
                        Product.Update(product);
                    }
                    else
                    {
                        Product.Delete(producte);

                    }

                }


            }

            return Ok();
        }

    }
}
