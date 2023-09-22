using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseCore.Configuration;
using BaseCore.Core;
using BaseCore.ViewModel;
using Core.Repositories.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Configuration
{

    [ApiVersion("1")]
    public class CrudController<TSelectDto, TDto, TEntity> : BaseController
        where TDto : BaseDto<TDto, TEntity>, new()
        where TSelectDto : BaseDto<TSelectDto, TEntity>, new()
        where TEntity : BaseEntity, new()
    {
        public readonly IRepository<TEntity> Repository;
        public readonly IMapper Mapper;

        public CrudController(IRepository<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        [HttpPost("[action]")]
        public IActionResult List(DataSourceRequest request, CancellationToken cancellationToken)
        {

            var query = Repository.TableNoTracking;

           
            return new DataSourceResult<TSelectDto>(
                               query.ProjectTo<TSelectDto>(Mapper.ConfigurationProvider),
                               request);

        }
        [HttpGet("{id}")]
        public virtual async Task<ApiResult<TDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            var dto = await Repository.TableNoTracking.ProjectTo<TDto>(Mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Guid.Equals(id), cancellationToken);

            if (dto == null)
                return NotFound();

            return dto;
        }

        [HttpPost]
        public virtual async Task<ApiResult<TSelectDto>> Create(TDto dto, CancellationToken cancellationToken)
        {
            var model = dto.ToEntity(Mapper);

            await Repository.AddAsync(model, cancellationToken);

            var resultDto = await Repository.TableNoTracking.ProjectTo<TSelectDto>(Mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Id.Equals(model.Id), cancellationToken);

            return resultDto;
        }

        [HttpPost("[action]")]
        public virtual async Task<ApiResult<TSelectDto>> Update(TDto dto, CancellationToken cancellationToken)
        {
            var model = await Repository.GetByGuidAsync(cancellationToken, dto.Guid);
            dto.Guid = model.Guid;
            model = dto.ToEntity(Mapper, model);

            await Repository.UpdateAsync(model, cancellationToken);

            var resultDto = await Repository.TableNoTracking.ProjectTo<TSelectDto>(Mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Id.Equals(model.Id), cancellationToken);

            return resultDto;
        }

        [HttpPost("[action]/{id}")]
        public virtual async Task<ApiResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var model = await Repository.GetByGuidAsync(cancellationToken, id);

                await Repository.DeleteAsync(model, cancellationToken);
            }
            catch (Exception)
            {

                var model = await Repository.GetByGuidAsync(cancellationToken, id);
                model.IsSoftDeleted = true;
                await Repository.UpdateAsync(model, cancellationToken);
            }


            return Ok();
        }
    }


    [ApiVersion("1")]
    public class CrudHierarchicalController<TSelectDto, TDto, TEntity> : BaseController
    where TDto : BaseDto<TDto, TEntity>, new()
    where TSelectDto : BaseDtoHierarchical<TSelectDto, TEntity>, new()
    where TEntity : BaseEntity, new()
    {
        public readonly IRepository<TEntity> Repository;
        public readonly IMapper Mapper;

        public CrudHierarchicalController(IRepository<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        [HttpPost("[action]")]
        public virtual IActionResult List(Guid id, DataSourceRequestTree request, CancellationToken cancellationToken)
        {
            var query = Repository.TableNoTracking;

            return new DataSourceResultTree<TSelectDto>(
                               query.ProjectTo<TSelectDto>(Mapper.ConfigurationProvider),
                               request, Mapper, true);

        }
        [HttpGet("{id}")]
        public virtual async Task<ApiResult<TDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            var dto = await Repository.TableNoTracking.ProjectTo<TDto>(Mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Guid.Equals(id), cancellationToken);

            if (dto == null)
                return NotFound();

            return dto;
        }

        [HttpPost]
        public virtual async Task<ApiResult<TSelectDto>> Create(TDto dto, CancellationToken cancellationToken)
        {
            var model = dto.ToEntity(Mapper);

            await Repository.AddAsync(model, cancellationToken);

            var resultDto = await Repository.TableNoTracking.ProjectTo<TSelectDto>(Mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Id.Equals(model.Id), cancellationToken);

            return resultDto;
        }

        [HttpPost("[action]")]
        public virtual async Task<ApiResult<TSelectDto>> Update(TDto dto, CancellationToken cancellationToken)
        {
            var model = await Repository.GetByGuidAsync(cancellationToken, dto.Guid);

            model = dto.ToEntity(Mapper, model);

            await Repository.UpdateAsync(model, cancellationToken);

            var resultDto = await Repository.TableNoTracking.ProjectTo<TSelectDto>(Mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Id.Equals(model.Id), cancellationToken);

            return resultDto;
        }

        [HttpPost("[action]/{id}")]
        public virtual async Task<ApiResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var model = await Repository.GetByGuidAsync(cancellationToken, id);

                await Repository.DeleteAsync(model, cancellationToken);
            }
            catch (Exception)
            {

                var model = await Repository.GetByGuidAsync(cancellationToken, id);
                model.IsSoftDeleted = true;
                await Repository.UpdateAsync(model, cancellationToken);
            }

            return Ok();
        }
    }
}
