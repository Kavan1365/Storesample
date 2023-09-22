using Api.Configuration;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseCore.Configuration;
using Core.Entities.Base;
using Core.Repositories.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Imaging;
using entity=Core.Entities.Base;

using static Dapper.SqlMapper;
using ViewModels;
using Core.Entities.Countries;
using ViewModels.Base;
using BaseCore.Dapper;

namespace Api.Controllers.Base
{
    [Authorize()]
    public class BannerController : BaseController
    {
        private IWebHostEnvironment Environment;
        public readonly IRepository<Banner> Repository;
        public readonly IRepository<City> CityRepository;
        public readonly IRepository<entity.File> FileRepository;
        public readonly IMapper Mapper;

        public BannerController(IRepository<Banner> repository,IRepository<City> cityRepository, IWebHostEnvironment environment, IRepository<entity.File> fileRepository, IMapper mapper)
        {
            CityRepository = cityRepository;
            Environment = environment;
            FileRepository = fileRepository;
            Repository = repository;
            Mapper = mapper;
        }
        #region Client

        [HttpGet()]
        [AllowAnonymous]
        public virtual async Task<ApiResult<BannerViewModel>> Get(CancellationToken cancellationToken)
        {
            var dto = await Repository.TableNoTracking.ProjectTo<BannerViewModel>(Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (dto == null)
            {
                await Repository.AddAsync(new Banner(), cancellationToken);
                dto = await Repository.TableNoTracking.ProjectTo<BannerViewModel>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

            }

            if (dto == null)
                return NotFound();

            return dto;
        }


        [HttpPost]
        public virtual async Task<ApiResult> Create(BannerViewModel dto, CancellationToken cancellationToken)
        {
            var obj = await Repository.TableNoTracking.ProjectTo<BannerViewModel>(Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
            if (dto == null)
            {
                await Repository.AddAsync(new Banner(), cancellationToken);
                obj = await Repository.TableNoTracking.ProjectTo<BannerViewModel>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
            }

            var model = dto.ToEntity(Mapper);
            model.Id = obj.Id;
            await Repository.UpdateAsync(model, cancellationToken);
            return Ok();
        }

    

        #endregion

    }


}
