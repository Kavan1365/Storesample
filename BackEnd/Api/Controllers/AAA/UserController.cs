using Api.Configurations;
using Api.Configurations.Captcha;
using Api.Configurations.Jwt;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseCore.Configuration;
using BaseCore.Core.AAA;
using BaseCore.Exceptions;
using BaseCore.Helper;
using BaseCore.Sms;
using BaseCore.Utilities;
using BaseCore.ViewModel;
using Core;
using Core.Entities.AAA;
using Core.Repositories.Base;
using Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViewModels.AAA;

namespace Api.Controllers.AAA
{
    [ApiVersion("1")]
    public class UserController : BaseController
    {
        private readonly IRepository<UserAddress> _userAddressService;
        private readonly ICaptchaService _captchaService;
        private readonly IRepository<User> Userservice;
        private readonly IRepository<ClientIdentity> _clientIdentityrepository;
        private readonly IRepository<UserClientIdentity> _userClientIdentityrepository;
        private readonly UserData Userdata;
        private readonly IRepository<UserRole> UserRoleservice;
        private readonly IRepository<UserTemp> UserTempservice;
        private readonly ILogger<UserController> Logger;
        private readonly IJwtService _jwtService;

        private readonly GoogleService _googleService;
        public readonly UrlHelper UrlHelper;

        public readonly IMapper Mapper;
        public readonly ISmsService SmsService;
        public UserController(IMapper mapper,
            GoogleService googleService,
              IJwtService jwtService,
             IRepository<UserClientIdentity> userClientIdentityrepository,
              IRepository<ClientIdentity> clientIdentityrepository,
           IRepository<UserAddress> userAddressService,
           UserData userdata, UrlHelper urlHelper, IRepository<UserRole> userRoleservice, ICaptchaService captchaService, IRepository<UserTemp> userTempservice, ILogger<UserController> logger, ISmsService smsService, IRepository<User> userservice)
        {
            _captchaService = captchaService ?? throw new ArgumentNullException(nameof(captchaService));
            _userAddressService = userAddressService ?? throw new ArgumentNullException(nameof(userAddressService));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
            _userClientIdentityrepository = userClientIdentityrepository ?? throw new ArgumentNullException(nameof(userClientIdentityrepository));
            _clientIdentityrepository = clientIdentityrepository ?? throw new ArgumentNullException(nameof(clientIdentityrepository));
            _googleService = googleService;
            Userdata = userdata;
            UserRoleservice = userRoleservice;
            UserTempservice = userTempservice;
            Mapper = mapper;
            UrlHelper = urlHelper;
            Logger = logger;
            Userservice = userservice;
            SmsService = smsService;
        }

        #region User
        [HttpGet("[action]")]
        public virtual async Task<ApiResult<UserViewModelCreateClient>> GetByUserId(CancellationToken cancellationToken)
        {
            var obj = await Userservice.TableNoTracking.Include(x => x.UserRoles)
                .Include(x => x.City).ProjectTo<UserViewModelCreateClient>(Mapper.ConfigurationProvider)
                  .SingleOrDefaultAsync(p => p.Id.Equals(Userdata.Id), cancellationToken);
            if (obj == null)
                return NotFound();
            return obj;
        }
        [HttpPost("[action]/{guid}")]
        public virtual async Task<ApiResult> convertToAdmin(Guid guid, CancellationToken cancellationToken)
        {
            var obj = await Userservice.Table
                  .SingleOrDefaultAsync(p => p.Guid.Equals(guid), cancellationToken);
            var userrole = await UserRoleservice.Table.Where(z => z.UserId == obj.Id).ToListAsync(cancellationToken);
            if (userrole is not null)
            {
                UserRoleservice.DeleteRange(userrole);
            }
            UserRoleservice.Add(new UserRole() { UserId = obj.Id, RoleId = 1 });
            obj.UserType = UserType.Admin;
            Userservice.Update(obj);
            return Ok();
        }


        [HttpGet("[action]")]
        
        public virtual async Task<ApiResult<string>> GetCount(CancellationToken cancellationToken)
        {
            var obj = await Userservice.TableNoTracking.Include(x => x.UserRoles)
                  .CountAsync(cancellationToken);
            return obj + "";
        }

        [HttpGet("[action]")]
        public virtual async Task<ApiResult<EditProfileUserViewModel>> GetimageUser(CancellationToken cancellationToken)
        {
            var obj = await Userservice.TableNoTracking
                .ProjectTo<EditProfileUserViewModel>(Mapper.ConfigurationProvider)
                  .SingleOrDefaultAsync(p => p.Id.Equals(Userdata.Id), cancellationToken);
            if (obj == null)
                return NotFound();
            return obj;
        }


        [HttpGet("{id}")]
        [AllowAnonymous()]
        public virtual async Task<ApiResult<UserViewModelCreate>> Get(Guid id, CancellationToken cancellationToken)
        {
            var obj = await Userservice.TableNoTracking.Include(x => x.UserRoles).Include(x => x.City).ProjectTo<UserViewModelCreate>(Mapper.ConfigurationProvider)
                  .SingleOrDefaultAsync(p => p.Guid.Equals(id), cancellationToken);
            if (obj == null)
                return NotFound();
            return obj;
        }
        [HttpGet("[action]/{id}")]
        [AllowAnonymous()]
        public virtual async Task<ApiResult<UserViewModel>> GetById(int id, CancellationToken cancellationToken)
        {
            var obj = await Userservice.TableNoTracking.Include(x => x.UserRoles).Include(x => x.City).ProjectTo<UserViewModel>(Mapper.ConfigurationProvider)
                  .SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);
            if (obj == null)
                return NotFound();
            return obj;
        }

        [HttpGet("[action]/{id}")]
        [AllowAnonymous()]
        public virtual async Task<ApiResult<UserViewModel>> GetByUserName(string id, CancellationToken cancellationToken)
        {
            var obj = await Userservice.TableNoTracking
                .Include(x => x.UserRoles)
                .Include(x => x.City).ProjectTo<UserViewModel>(Mapper.ConfigurationProvider)
                  .SingleOrDefaultAsync(p => p.UserName.Equals(id), cancellationToken);
            if (obj == null)
                return NotFound();
            return obj;
        }

        [HttpPost("[action]")]
        public virtual IActionResult ListUser(DataSourceRequest request, CancellationToken cancellationToken)
        {
            var query = Userservice.TableNoTracking;

            return new DataSourceResult<UserViewModel>(
                               query.ProjectTo<UserViewModel>(Mapper.ConfigurationProvider),
                               request);

        }

        [HttpPost("[action]")]
        public virtual IActionResult Getuserclient(DataSourceRequest request, CancellationToken cancellationToken)
        {
            var query = Userservice.TableNoTracking
                ;

            if (request.Filter != null && request.Filter.Filters.Count > 0)
            {
                var title = request.Filter.Filters.Where(z => z.Field == "Title").ToList();

                if (title.Count() > 0)
                {
                    query = query.Where(z => z.UserName.Contains(title.FirstOrDefault().Value) || z.FullName.Contains(title.FirstOrDefault().Value));

                    request.Filter.Filters = null;
                    request.Filter = null;
                }

            }

            query = query.Include(z => z.Image)
                 .Include(z => z.City)
                 .ThenInclude(z => z.Province);

            return new DataSourceResult<UsualUserInfoViewModel>(
                               query.ProjectTo<UsualUserInfoViewModel>(Mapper.ConfigurationProvider),
                               request);

        }

        [HttpPost("[action]")]
        public virtual async Task<ApiResult<EditProfileUserViewModel>> ChangeProfile(EditProfileUserViewModel dto, CancellationToken cancellationToken)
        {
            var obj = Userservice.GetById(dto.Id);
            dto.Guid = obj.Guid;

            obj = dto.ToEntity(Mapper, obj);
                Userservice.Update(obj);

            return dto;
        }
        [HttpPost("[action]")]
        
        public virtual async Task<ApiResult<RegisterViewModel>> CreateUser(RegisterViewModel dto, CancellationToken cancellationToken)
        {
            var model = new User()
            {
                UserName = dto.UserName,
                FatherName = dto.FristName,
                LastName = dto.LastName,
                CityId = dto.CityId,
                NationalCode = dto.NationalCode,
                UserType = UserType.User

            };

            model.IsActive = true;
            var passwordHash = PasswordHasher.HashPasswordV2(dto.Password);
            model.PasswordHash = passwordHash;
            await Userservice.AddAsync(model, cancellationToken);
            await UserRoleservice.AddAsync(new UserRole() { RoleId = 2, UserId = model.Id }, cancellationToken);

            var getSms = await UserTempservice.TableNoTracking.Where(z => z.UserName == dto.UserName).ToListAsync(cancellationToken);
            await UserTempservice.DeleteRangeAsync(getSms, cancellationToken);


            await SmsService.SendSms(model.UserName, @"کاربر گرامی  ثبت نام شما ، با موفقیت انجام شد.");

            dto.Id = model.Id;

            return dto;
        }
        [HttpPost("[action]")]
        public virtual async Task<ApiResult<RegisterViewModel>> UpdateUser(RegisterViewModel dto, CancellationToken cancellationToken)
        {
            var model = await Userservice.GetByGuidAsync(cancellationToken, dto.Guid);
            dto.Guid = model.Guid;

            model = dto.ToEntity(Mapper, model);
            await Userservice.UpdateAsync(model, cancellationToken);
            var resultDto = await Userservice.TableNoTracking.ProjectTo<RegisterViewModel>(Mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Id.Equals(model.Id), cancellationToken);
            return resultDto;
        }


        #endregion

        #region Employee
        [HttpPost("[action]")]
        public virtual IActionResult ListUserEmployee(DataSourceRequest request, CancellationToken cancellationToken)
        {
            var query = Userservice.TableNoTracking.Where(x => x.UserType == UserType.Admin);
            return new DataSourceResult<UserViewModel>(
                               query.ProjectTo<UserViewModel>(Mapper.ConfigurationProvider),
                               request);

        }

        [HttpPost("[action]")]
        public virtual async Task<ApiResult<UserViewModelCreate>> CreateUserEmployee(UserViewModelCreate dto, CancellationToken cancellationToken)
        {
            dto.UserType = UserType.Admin;
            var model = dto.ToEntity(Mapper);

            var checkUser = await Userservice.Table.AnyAsync(z => z.UserName == model.UserName, cancellationToken);
            if (checkUser)
                return new ApiResult<UserViewModelCreate>(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, null,  ErrorMessages.UserNameIsExists);

            model.UserType = UserType.Admin;
            await Userservice.AddAsync(model, cancellationToken);



            foreach (var item in dto.RoleIds)
            {
                await UserRoleservice.AddAsync(new UserRole() { RoleId = int.Parse(item), UserId = model.Id }, cancellationToken);

            }

            dto.Id = model.Id;
            return dto;
        }

        [HttpPost("[action]")]

        public virtual async Task<ApiResult<UserViewModelCreate>> UpdateUserEmployee(UserViewModelCreate dto, CancellationToken cancellationToken)
        {
            var model = await Userservice.GetByGuidAsync(cancellationToken, dto.Guid);

            model = dto.ToEntity(Mapper, model);
            model.UserType = UserType.Admin;
            dto.Guid = model.Guid;

            await Userservice.UpdateAsync(model, cancellationToken);

            var getuserrole = UserRoleservice.Table.Where(x => x.UserId == model.Id).ToList();
            await UserRoleservice.DeleteRangeAsync(getuserrole, cancellationToken, false);
            foreach (var item in dto.RoleIds)
            {
                await UserRoleservice.AddAsync(new UserRole() { RoleId = int.Parse(item), UserId = model.Id }, cancellationToken);

            }

            return dto;
        }

        #endregion


        [HttpPost("[action]/{guid}")]
        public virtual async Task<ApiResult> Delete(Guid guid, CancellationToken cancellationToken)
        {
            var obj = await Userservice.GetByGuidAsync(cancellationToken, guid);
            try
            {
                var getuserrole = UserRoleservice.Table.Where(x => x.UserId == obj.Id).ToList();
                await UserRoleservice.DeleteRangeAsync(getuserrole, cancellationToken, false);
                await Userservice.DeleteAsync(obj, cancellationToken);

                Ok();
            }
            catch (Exception)
            {
                obj.UserName = "";
                await Userservice.UpdateAsync(obj, cancellationToken);
                return new ApiResult(false, BaseCore.Utilities.ApiResultStatusCode.ServerError);
            }

            return Ok();
        }
        
        [HttpGet("[action]")]
        public virtual async Task<ApiResult<CaptchaDto>> Captcha(CancellationToken cancellationToken)
        {
            var CaptchaId = await _captchaService.GenerateCaptcha();

            return new CaptchaDto()
            {
                CaptchaId = CaptchaId,
                CaptchaImage = CaptchaList.Captchas.Find(i => i.CaptchaId == CaptchaId).CaptchaImage
            };
        }

        [HttpPost("[action]/{username}")]
        
        public async Task<ApiResult> SendSms(string username, CancellationToken cancellationToken)
        {
            var date = DateTime.Now;
            var checkUserName = await Userservice.TableNoTracking.AnyAsync(z => z.UserName == username, cancellationToken);
            if (!checkUserName)
            {
                var getSmsTimer = await UserTempservice.TableNoTracking.Where(z => z.UserName == username).ToListAsync(cancellationToken);


                var getSms = await UserTempservice.TableNoTracking.Where(z => z.UserName == username).ToListAsync(cancellationToken);
                await UserTempservice.DeleteRangeAsync(getSms, cancellationToken);
                Random random = new Random();
                var code = random.Next(10000, 99999).ToString();

                await SmsService.SendSms(username, "کد احراز هویت شما " + code + " می باشد ");

                ////send coder panel///
                await UserTempservice.AddAsync(new UserTemp() { Code = code, UserName = username }, cancellationToken);
                return new ApiResult(true, BaseCore.Utilities.ApiResultStatusCode.Success);
            }
            else
            {
                return new ApiResult(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, ErrorMessages.UserNameIsExists);

            }
        }
        
        [HttpPost("[action]/{oldPassword}/{confirmPassword}/{newpassword}")]
        public virtual async Task<ApiResult> ChangePassword(string oldPassword, string confirmPassword, string newpassword, CancellationToken cancellationToken)
        {

            if (confirmPassword != newpassword)
                throw new BadRequestException(ErrorMessages.ComparePassword);

            if (oldPassword == newpassword)
                throw new BadRequestException(ErrorMessages.NewPasswordNotPreviousPassword);

            var oldpasswordHash = PasswordHasher.HashPasswordV2(oldPassword);

            var getuser = Userservice.GetById(Userdata.Id);
            var checkpass = PasswordHasher.VerifyHashedPasswordV2(getuser.PasswordHash, oldPassword);
            if (!checkpass)
                throw new BadRequestException(ErrorMessages.OldPasswrod);

            var passwordHash = PasswordHasher.HashPasswordV2(newpassword);
            getuser.PasswordHash = passwordHash;
            var userclient = await _userClientIdentityrepository.Table.Where(z => z.UserId == Userdata.Id).ToListAsync(cancellationToken);
            await _userClientIdentityrepository.DeleteRangeAsync(userclient, cancellationToken);

            await Userservice.UpdateAsync(getuser, cancellationToken);
            return Ok();


        }


        [HttpPost("[action]/{UserId}/{Password}")]
        public virtual async Task<ApiResult> ChangePasswordByAdmin(int UserId, string Password, CancellationToken cancellationToken)
        {

            var getuser = Userservice.GetById(UserId);
            var passwordHash = PasswordHasher.HashPasswordV2(Password);
            getuser.PasswordHash = passwordHash;
            var userclient = await _userClientIdentityrepository.Table.Where(z => z.UserId == UserId).ToListAsync(cancellationToken);
            await _userClientIdentityrepository.DeleteRangeAsync(userclient, cancellationToken);
            await Userservice.UpdateAsync(getuser, cancellationToken);

            return Ok();
        }
        [HttpPost("[action]")]
        public virtual async Task<ApiResult> Logout(CancellationToken cancellationToken)
        {

            var userclient = await _userClientIdentityrepository.Table.Where(z => z.UserId == Userdata.Id).ToListAsync(cancellationToken);
            await _userClientIdentityrepository.DeleteRangeAsync(userclient, cancellationToken);
            return Ok();
        }

        [HttpGet("[action]/{username}/{Password}")]
        public virtual async Task<ApiResult> ChangePasswordByForget(string username, string Password, CancellationToken cancellationToken)
        {

            var getuser = await Userservice.Table.Where(z => z.UserName == username).SingleAsync(cancellationToken);
            var passwordHash = PasswordHasher.HashPasswordV2(Password);
            getuser.PasswordHash = passwordHash;
            var userclient = await _userClientIdentityrepository.Table.Where(z => z.UserId == getuser.Id).ToListAsync(cancellationToken);
            await _userClientIdentityrepository.DeleteRangeAsync(userclient, cancellationToken);
            await Userservice.UpdateAsync(getuser, cancellationToken);

            return Ok();
        }


        [HttpGet("[action]/{Password}")]
        public virtual async Task<string> getChangePassword(string Password, CancellationToken cancellationToken)
        {

            var passwordHash = PasswordHasher.HashPasswordV2(Password);

            return passwordHash;
        }

        [HttpPost("[action]")]
        public async Task<ApiResult> ForgotPassword(ForgotPasswordDto dto, CancellationToken cancellationToken)
        {

            //var checkGoogleToken = await _googleService.VerifyToken(dto.Token);
            //if (!checkGoogleToken)
            //    return new ApiResult(false, ApiResultStatusCode.ServerError, ErrorMessages.GoogleReCAPTCHA);


            var user = await Userservice.TableNoTracking.AnyAsync(z => z.UserName == dto.Username, cancellationToken);
            if (!user)
                return new ApiResult(false, ApiResultStatusCode.ServerError, ErrorMessages.NotUserNameIsExists);

            var date = DateTime.Now;

            var getSmsTimer = await UserTempservice.TableNoTracking.Where(z => z.UserName == dto.Username).ToListAsync(cancellationToken);


            var getSms = await UserTempservice.TableNoTracking.Where(z => z.UserName == dto.Username).ToListAsync(cancellationToken);
            await UserTempservice.DeleteRangeAsync(getSms, cancellationToken);

            ////send coder panel///

            Random random = new Random();
            var code = random.Next(10000, 99999).ToString();
            await SmsService.SendSms(dto.Username, "کد احراز هویت شما " + code + " می باشد ");


            await UserTempservice.AddAsync(new UserTemp() { Code = code, UserName = dto.Username }, cancellationToken);
            return new ApiResult(true, BaseCore.Utilities.ApiResultStatusCode.Success);

        }

        [HttpPost("[action]")]
        public async Task<ApiResult> ResetPassword(ResetPasswordDto dto, CancellationToken cancellationToken)
        {

            #region valid

            //var checkGoogleToken = await _googleService.VerifyToken(dto.Token);
            //if (!checkGoogleToken)
            //    return new ApiResult(false, ApiResultStatusCode.ServerError, ErrorMessages.GoogleReCAPTCHA);


            if (dto.ComfirmPassword == dto.Password)
                throw new BadRequestException(@ErrorMessages.ComparePassword);

            #endregion

            var checkCodeSms = await UserTempservice.TableNoTracking.AnyAsync(z => z.UserName == dto.Username && z.Code == dto.Code, cancellationToken);
            if (checkCodeSms)
            {
                if (string.IsNullOrEmpty(dto.Password))
                    return new ApiResult(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, ErrorMessages.PasswordRequired);


                var result = await Userservice.TableNoTracking.SingleAsync(z => z.UserName == dto.Username, cancellationToken);
                var passwordHash = PasswordHasher.HashPasswordV2(dto.Password);
                result.PasswordHash = passwordHash;
                var userclient = await _userClientIdentityrepository.Table.Where(z => z.UserId == result.Id).ToListAsync(cancellationToken);
                await _userClientIdentityrepository.DeleteRangeAsync(userclient, cancellationToken);

                await Userservice.UpdateAsync(result, cancellationToken);


                return new ApiResult(true, BaseCore.Utilities.ApiResultStatusCode.Success);
            }
            else
            {
                return new ApiResult(false, BaseCore.Utilities.ApiResultStatusCode.BadRequest, ErrorMessages.equalCodeSms);

            }


        }




    }
}
