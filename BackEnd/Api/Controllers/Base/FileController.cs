using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseCore.Configuration;
using BaseCore.Helper.ExtensionMethods;
using BaseCore.Utilities;
using Core.Entities.Base;
using Core.Repositories.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using ViewModels.Validations;
using fileentity = Core.Entities.Base;

namespace Api.Controllers.Base
{
    public class FileController : BaseController
    {
        private IMapper _mapper;
        private IWebHostEnvironment _environment;
        private readonly IRepository<fileentity.File> _service;
        private readonly ILogger<FileController> _logger;
        public IConfiguration Configuration { get; }
        public FileController(IConfiguration configuration, IMapper mapper, IWebHostEnvironment environment, IRepository<fileentity.File> service, ILogger<FileController> logger)
        {
            _mapper = mapper;
            _environment = environment;
            _service = service;
            Configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("[action]")]
        public FileStreamResult Download(string path)
        {
            var root = _environment.WebRootPath;
            try
            {
                var fullpath = root + "\\" + "files" + "\\" + path.Replace("/", "\\");
                var stream = new FileStream(fullpath.Replace("/", "\\"), FileMode.Open);
                string contentType;
                new FileExtensionContentTypeProvider().TryGetContentType(path, out contentType);
                var mimeType = contentType ?? "application/octet-stream";
                return File(stream, mimeType);
            }
            catch (System.Exception e)
            {
                _logger.LogError("NotFoundFileDownload", e);

                var fullpath = root + "/" + "files" + "/" + "404.jpg";
                var stream = new FileStream(fullpath.Replace("/", "\\"), FileMode.Open);
                string contentType;
                new FileExtensionContentTypeProvider().TryGetContentType(path, out contentType);
                var mimeType = contentType ?? "application/octet-stream";

                return File(stream, mimeType);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("[action]")]
        public FileStreamResult DownloadCaptcha(string path)
        {
            try
            {
                var root = _environment.WebRootPath;
                var fullpath = root + "\\" + path.Replace("/", "\\");
                var stream = new FileStream(fullpath.Replace("/", "\\"), FileMode.Open);
                string contentType;
                new FileExtensionContentTypeProvider().TryGetContentType(path, out contentType);
                var mimeType = contentType ?? "application/octet-stream";
                return File(stream, mimeType);
            }
            catch (System.Exception e)
            {
                _logger.LogError("NotFoundFileDownload", e);

                var fullpath = Directory.GetCurrentDirectory() + "/" + "Captchas" + "/" + "404.jpg";
                var stream = new FileStream(fullpath.Replace("/", "\\"), FileMode.Open);
                string contentType;
                new FileExtensionContentTypeProvider().TryGetContentType(path, out contentType);
                var mimeType = contentType ?? "application/octet-stream";

                return File(stream, mimeType);
            }
        }



        [HttpGet("[action]/{id}")]
        [AllowAnonymous]
        public async Task<ApiResult<FileViewModel>> GetInfoFile(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _service.TableNoTracking
                    .ProjectTo<FileViewModel>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(z => z.Id.Equals(id), cancellationToken); ;
            }
            catch
            {
                return null;
            }
        }

        [HttpPost("[action]")]
        public async Task<ApiResult> UploadFileslogo([FromForm] IFormFile file, CancellationToken cancellationToken)
        {
            try
            {
                var root = _environment.WebRootPath;
                var FileExtensions = new string[] { "jpg", "jpeg", "png", "docx", "xlsx", "zip", "rar", "pdf", "ico" };
                var Extension = file.FileName.Split(".")[file.FileName.Split(".").Length - 1];
                if (!FileExtensions.Select(x => x.Trim()).Any(t => t.ToLower() == Extension.ToLower()))
                    return BadRequest();
                var path = root + "\\" + "files" + "\\" + "Setting" + "\\logo.jpg";

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);


                var fileEntity = new fileentity.File()
                {
                    Extension = Extension,
                    FileName = file.FileName,
                    Path = "Setting\\logo.jpg",
                    Size = int.Parse(file.Length + "")
                };
                await file.SaveAsAsync(path);
                await _service.AddAsync(fileEntity, cancellationToken);

                return new ApiResult(true, BaseCore.Utilities.ApiResultStatusCode.Success, fileEntity.Id + "");
            }
            catch (System.Exception e)
            {
                _logger.LogError("BadRequestFile", e);
                return BadRequest();

            }

        }


        [HttpPost("[action]")]
        public async Task<ApiResult> UploadFilesFavicons([FromForm] IFormFile file, CancellationToken cancellationToken)
        {
            try
            {
                var root = _environment.WebRootPath;
                var FileExtensions = new string[] { "jpg", "jpeg", "png", "docx", "xlsx", "zip", "rar", "pdf", "ico" };
                var Extension = file.FileName.Split(".")[file.FileName.Split(".").Length - 1];
                if (!FileExtensions.Select(x => x.Trim()).Any(t => t.ToLower() == Extension.ToLower()))
                    return BadRequest();


                var path = root + "\\" + "files" + "\\" + "Setting" + "\\Favicons.ico";

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var fileEntity = new fileentity.File()
                {
                    Extension = Extension,
                    FileName = "Favicons.ico",
                    Path = "Setting" + "\\Favicons.ico",
                    Size = int.Parse(file.Length + "")
                };
                await file.SaveAsAsync(path);
                await _service.AddAsync(fileEntity, cancellationToken);

                return new ApiResult(true, BaseCore.Utilities.ApiResultStatusCode.Success, fileEntity.Id + "");
            }
            catch (System.Exception e)
            {
                _logger.LogError("BadRequestFile", e);
                return BadRequest();

            }

        }

        [HttpPost("[action]/{fileId}")]
        public async Task<ApiResult> UpdateUploadlogo([FromForm] IFormFile file, int fileId, CancellationToken cancellationToken)
        {
            try
            {
                var FileExtensions = new string[] { "jpg", "jpeg", "png", "docx", "xlsx", "zip", "rar", "pdf" };
                var Extension = file.FileName.Split(".")[file.FileName.Split(".").Length - 1];
                if (!FileExtensions.Select(x => x.Trim()).Any(t => t.ToLower() == Extension.ToLower()))
                    return BadRequest();
                var root = _environment.WebRootPath;
                var getFile = _service.GetById(fileId);

                var path = root + "\\" + "files" + "\\" + "Setting" + "\\logo.jpg";

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);


                var fileEntity = new fileentity.File()
                {
                    Id = getFile.Id,
                    Guid = getFile.Guid,
                    Extension = Extension,
                    FileName = "logo.jpg",
                    Path = "Setting" + "\\logo.jpg",
                    Size = int.Parse(file.Length + "")
                };
                await file.SaveAsAsync(path);
                await _service.AddAsync(fileEntity, cancellationToken);
                return new ApiResult(true, BaseCore.Utilities.ApiResultStatusCode.Success, fileEntity.Id + "");



            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }



        }

        [HttpPost("[action]/{fileId}")]
        public async Task<ApiResult> UpdateUploadFavicons([FromForm] IFormFile file, int fileId, CancellationToken cancellationToken)
        {
            try
            {
                var FileExtensions = new string[] { "jpg", "jpeg", "png", "docx", "xlsx", "zip", "rar", "pdf" };
                var Extension = file.FileName.Split(".")[file.FileName.Split(".").Length - 1];
                if (!FileExtensions.Select(x => x.Trim()).Any(t => t.ToLower() == Extension.ToLower()))
                    return BadRequest();
                var root = _environment.WebRootPath;
                var getFile = _service.GetById(fileId);

                var path = root + "\\" + "files" + "\\" + "Setting" + "\\Favicons.ico";

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);


                var fileEntity = new fileentity.File()
                {
                    Id = getFile.Id,
                    Guid = getFile.Guid,
                    Extension = Extension,
                    FileName = "Favicons.ico",
                    Path = "Setting" + "\\Favicons.ico",
                    Size = int.Parse(file.Length + "")
                };
                await file.SaveAsAsync(path);
                await _service.AddAsync(fileEntity, cancellationToken);
                return new ApiResult(true, BaseCore.Utilities.ApiResultStatusCode.Success, fileEntity.Id + "");



            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }



        }



        [HttpPost("[action]")]
        public async Task<ApiResult> UploadFiles([FromForm] IFormFile file, CancellationToken cancellationToken)
        {
            try
            {
                var root = _environment.WebRootPath;
                var FileExtensions = new string[] { "jpg", "jpeg", "png", "docx", "xlsx", "zip", "rar", "pdf", "ico" };
                var Extension = file.FileName.Split(".")[file.FileName.Split(".").Length - 1];
                if (!FileExtensions.Select(x => x.Trim()).Any(t => t.ToLower() == Extension.ToLower()))
                    return BadRequest();
                var filename = Guid.NewGuid() + "." + Extension.ToLower();
                var path = root + "\\" + "files" + "\\" + filename;
                var fileEntity = new fileentity.File()
                {
                    Extension = Extension,
                    FileName = file.FileName,
                    Path = filename,
                    Size = int.Parse(file.Length + "")
                };
                await file.SaveAsAsync(path);
                await _service.AddAsync(fileEntity, cancellationToken);

                return new ApiResult(true, BaseCore.Utilities.ApiResultStatusCode.Success, fileEntity.Id + "");
            }
            catch (System.Exception e)
            {
                _logger.LogError("BadRequestFile", e);
                return BadRequest();

            }

        }

        [HttpPost("[action]/{fileId}")]
        public async Task<ApiResult> UpdateUpload([FromForm] IFormFile file, int fileId, CancellationToken cancellationToken)
        {
            try
            {
                if (fileId>0)
                {

                var FileExtensions = new string[] { "jpg", "jpeg", "png", "docx", "xlsx", "zip", "rar", "pdf" };
                var Extension = file.FileName.Split(".")[file.FileName.Split(".").Length - 1];
                if (!FileExtensions.Select(x => x.Trim()).Any(t => t.ToLower() == Extension.ToLower()))
                    return BadRequest();
                var root = _environment.WebRootPath;
                var getFile = _service.GetById(fileId);

                var fullpath = root + "\\" + "files" + "\\" + getFile.Url.Replace("/", "\\");
                if (System.IO.File.Exists(fullpath))
                    System.IO.File.Delete(fullpath);


                var filename = Guid.NewGuid() + "." + Extension.ToLower();
                var path = root + "\\" + "files" + "\\" + filename;
                var fileEntity = new fileentity.File()
                {
                    Id = getFile.Id,
                    Guid = getFile.Guid,
                    Extension = Extension,
                    FileName = file.FileName,
                    Path = filename,
                    Size = int.Parse(file.Length + "")
                };
                await file.SaveAsAsync(path);
                await _service.AddAsync(fileEntity, cancellationToken);
                return new ApiResult(true, BaseCore.Utilities.ApiResultStatusCode.Success, fileEntity.Id + "");

                }
                else
                {
                    var root = _environment.WebRootPath;
                    var FileExtensions = new string[] { "jpg", "jpeg", "png", "docx", "xlsx", "zip", "rar", "pdf", "ico" };
                    var Extension = file.FileName.Split(".")[file.FileName.Split(".").Length - 1];
                    if (!FileExtensions.Select(x => x.Trim()).Any(t => t.ToLower() == Extension.ToLower()))
                        return BadRequest();
                    var filename = Guid.NewGuid() + "." + Extension.ToLower();
                    var path = root + "\\" + "files" + "\\" + filename;
                    var fileEntity = new fileentity.File()
                    {
                        Extension = Extension,
                        FileName = file.FileName,
                        Path = filename,
                        Size = int.Parse(file.Length + "")
                    };
                    await file.SaveAsAsync(path);
                    await _service.AddAsync(fileEntity, cancellationToken);

                    return new ApiResult(true, BaseCore.Utilities.ApiResultStatusCode.Success, fileEntity.Id + "");
                }


            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }



        }

        [HttpPost("[action]/{fileId}")]
        public async Task DeleteFile(int fileId, CancellationToken cancellationToken)
        {
            var getFile = _service.GetById(fileId);
            var root = _environment.WebRootPath;
            try
            {
                await _service.DeleteAsync(getFile, cancellationToken);

                var fullpath = root + "\\" + "files" + "\\" + getFile.Url.Replace("/", "\\");
                if (System.IO.File.Exists(fullpath))
                    System.IO.File.Delete(fullpath);

            }
            catch (Exception)
            {
                var fullpath = root + "\\" + "files" + "\\" + getFile.Url.Replace("/", "\\");
                if (System.IO.File.Exists(fullpath))
                    System.IO.File.Delete(fullpath);

            }



        }

    }
}
