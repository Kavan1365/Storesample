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

namespace Api.Controllers.Base
{
    [Authorize()]
    public class SettingController : BaseController
    {
        private IWebHostEnvironment Environment;
        public readonly IRepository<Setting> Repository;
        public readonly IRepository<City> CityRepository;
        public readonly IRepository<entity.File> FileRepository;
        public readonly IMapper Mapper;

        public SettingController(IRepository<Setting> repository,IRepository<City> cityRepository, IWebHostEnvironment environment, IRepository<entity.File> fileRepository, IMapper mapper)
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
        public virtual async Task<ApiResult<SettingViewModel>> Get(CancellationToken cancellationToken)
        {
            var dto = await Repository.TableNoTracking.ProjectTo<SettingViewModel>(Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (dto == null)
            {
                await Repository.AddAsync(new Setting(), cancellationToken);
                dto = await Repository.TableNoTracking.ProjectTo<SettingViewModel>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

            }

            if (dto == null)
                return NotFound();

            return dto;
        }


        [HttpPost("[action]/{name}/{id}")]
      
        public virtual async Task<ApiResult> DeleteFile(string name, int id, CancellationToken cancellationToken)
        {
            var dto = await Repository.TableNoTracking
                .FirstOrDefaultAsync(cancellationToken);
            if (name.Trim().ToLower()== "logoid")
            {
            dto.LogoId = null;

            }
            else
            {

            dto.FaviconsId = null;
            }
           await Repository.UpdateAsync(dto, cancellationToken);
            var getFile = FileRepository.GetById(id);
            await FileRepository.DeleteAsync(getFile, cancellationToken);
            var root = Environment.WebRootPath;
            var fullpath = root + "\\" + "files" + "\\" + getFile.Url.Replace("/", "\\");
            if (System.IO.File.Exists(fullpath))
                System.IO.File.Delete(fullpath);


            return Ok();
        }


        [HttpPost]
        public virtual async Task<ApiResult> Create(SettingViewModel dto, CancellationToken cancellationToken)
        {
            var obj = await Repository.TableNoTracking.ProjectTo<SettingViewModel>(Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
            if (dto == null)
            {
                await Repository.AddAsync(new Setting(), cancellationToken);
                obj = await Repository.TableNoTracking.ProjectTo<SettingViewModel>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
            }

            var model = dto.ToEntity(Mapper);
            model.Id = obj.Id;
            await Repository.UpdateAsync(model, cancellationToken);
            return Ok();
        }

        #endregion
        [HttpPost("[action]")]
        public virtual async Task<ApiResult> SettingContactUS(SettingContactUSViewModel dto, CancellationToken cancellationToken)
        {
            var obj = await Repository.TableNoTracking.ProjectTo<SettingContactUSViewModel>(Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
            if (dto == null)
            {
                await Repository.AddAsync(new Setting(), cancellationToken);
                obj = await Repository.TableNoTracking.ProjectTo<SettingContactUSViewModel>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
            }

            var model = dto.ToEntity(Mapper);
            model.Id = obj.Id;
            await Repository.UpdateAsync(model, cancellationToken);
            return Ok();
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public virtual async Task<ApiResult<AboutStoreViewModel>> GetAboutStore(CancellationToken cancellationToken)
        {
            var dto = await Repository.TableNoTracking.ProjectTo<AboutStoreViewModel>(Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (dto == null)
            {
                await Repository.AddAsync(new Setting(), cancellationToken);
                dto = await Repository.TableNoTracking.ProjectTo<AboutStoreViewModel>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

            }

            if (dto == null)
                return NotFound();

            return dto;
        }
        [HttpPost("[action]")]
        public virtual async Task<ApiResult> AboutStore(AboutStoreViewModel dto, CancellationToken cancellationToken)
        {
            var obj = await Repository.TableNoTracking.ProjectTo<AboutStoreViewModel>(Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
            if (dto == null)
            {
                await Repository.AddAsync(new Setting(), cancellationToken);
                obj = await Repository.TableNoTracking.ProjectTo<AboutStoreViewModel>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
            }

            var model = dto.ToEntity(Mapper);
            model.Id = obj.Id;
            await Repository.UpdateAsync(model, cancellationToken);
            return Ok();
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public virtual async Task<ApiResult<AddressStoreViewModel>> GetStoreAddress(CancellationToken cancellationToken)
        {
            var dto = await Repository.TableNoTracking.ProjectTo<AddressStoreViewModel>(Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (dto == null)
            {
                await Repository.AddAsync(new Setting(), cancellationToken);
                dto = await Repository.TableNoTracking.ProjectTo<AddressStoreViewModel>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

            }

            if (dto.CityId.HasValue)
            {
                dto.ProvinceId=CityRepository.GetById(dto.CityId).ProvinceId;
            }

            if (dto == null)
                return NotFound();

            return dto;
        }
        [HttpPost("[action]")]
        public virtual async Task<ApiResult> StoreAddress(AddressStoreViewModel dto, CancellationToken cancellationToken)
        {
            var obj = await Repository.TableNoTracking.ProjectTo<AddressStoreViewModel>(Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
            if (dto == null)
            {
                await Repository.AddAsync(new Setting(), cancellationToken);
                obj = await Repository.TableNoTracking.ProjectTo<AddressStoreViewModel>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
            }

            var model = dto.ToEntity(Mapper);
            model.Id = obj.Id;
            await Repository.UpdateAsync(model, cancellationToken);
            return Ok();
        }
        [HttpGet("[action]")]
        [AllowAnonymous]
        public virtual async Task<ApiResult<TransportationViewModel>> GetTransportation(CancellationToken cancellationToken)
        {
            var dto = await Repository.TableNoTracking.ProjectTo<TransportationViewModel>(Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (dto == null)
            {
                await Repository.AddAsync(new Setting(), cancellationToken);
                dto = await Repository.TableNoTracking.ProjectTo<TransportationViewModel>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

            }

            if (dto == null)
                return NotFound();

            return dto;
        }



        [HttpPost("[action]")]
        public virtual async Task<ApiResult> Transportation(TransportationViewModel dto, CancellationToken cancellationToken)
        {
            var obj = await Repository.TableNoTracking.ProjectTo<TransportationViewModel>(Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
            if (dto == null)
            {
                await Repository.AddAsync(new Setting(), cancellationToken);
                obj = await Repository.TableNoTracking.ProjectTo<TransportationViewModel>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
            }

            var model = dto.ToEntity(Mapper);
            model.Id = obj.Id;
            await Repository.UpdateAsync(model, cancellationToken);
            return Ok();
        }



    }


}
