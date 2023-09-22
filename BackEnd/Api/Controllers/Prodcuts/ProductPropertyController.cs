using Api.Configuration;
using AutoMapper;
using Core.Entities.Prodcutes;
using Core.Repositories.Base;
using Infrastructure.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Prodcuts;

namespace Api.Controllers.Prodcuts
{
    [AllowAnonymous]
    public class ProductPropertyController : CrudController<ProductPropertyViewModel, ProductPropertyViewModelCreate, ProductProperty>
    {
        public ProductPropertyController(IRepository<ProductProperty> repository, IMapper mapper)
            : base(repository, mapper)
        {
        }

        [HttpPost("[action]")]
        public virtual async Task<IActionResult> UpdateInlineAsync(UpdateProductPropertyViewModel model, CancellationToken cancellationToken)
        {

            foreach (var item in model.models)
            {
                var obj = Repository.GetById(item.Id);
                obj.Title = item.Title;
                obj.IsFilter = item.IsFilter;
                obj.ViewOrder = item.ViewOrder;
                await Repository.UpdateAsync(obj, cancellationToken);
            }
            return Ok();
        }
    }
}
