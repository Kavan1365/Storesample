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


namespace Api.Controllers.Client
{
    

    [ApiVersion("1")]
    public class UserClientController : BaseController
    {
        private readonly IRepository<UserAddress> _userAddressService;
        private readonly ICaptchaService _captchaService;
        private readonly IRepository<User> Userservice;
        private readonly IRepository<ClientIdentity> _clientIdentityrepository;
        private readonly IRepository<UserClientIdentity> _userClientIdentityrepository;
        private readonly UserData Userdata;
        private readonly IRepository<UserRole> UserRoleservice;
        private readonly IRepository<UserTemp> UserTempservice;
        private readonly ILogger<UserClientController> Logger;
        private readonly IJwtService _jwtService;

        private readonly GoogleService _googleService;
        public readonly UrlHelper UrlHelper;

        public readonly IMapper Mapper;
        public readonly ISmsService SmsService;
        public UserClientController(IMapper mapper,
            GoogleService googleService,
              IJwtService jwtService,
             IRepository<UserClientIdentity> userClientIdentityrepository,
              IRepository<ClientIdentity> clientIdentityrepository,
           IRepository<UserAddress> userAddressService,
           UserData userdata, UrlHelper urlHelper, IRepository<UserRole> userRoleservice, ICaptchaService captchaService, IRepository<UserTemp> userTempservice, ILogger<UserClientController> logger, ISmsService smsService, IRepository<User> userservice)
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


        [AllowAnonymous]
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





        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ApiResult<AccessToken>> GetToken(TokenRequest tokenRequest, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(tokenRequest.refresh_token))
            {
                var expriddate = DateTime.Now.AddDays(30);
                var checkGoogleToken = _captchaService.ValidateCaptcha(tokenRequest.Captcha, tokenRequest.CaptchaId);
                if (!checkGoogleToken)
                    return new ApiResult<AccessToken>(false, ApiResultStatusCode.ServerError, null, ErrorMessages.CAPTCHA);

                var clinet = await _clientIdentityrepository.TableNoTracking.SingleOrDefaultAsync(z => z.client_secret == tokenRequest.client_secret && z.client_id == tokenRequest.client_id, cancellationToken);
                if (clinet is null)
                    return new ApiResult<AccessToken>(false, ApiResultStatusCode.ServerError, null, ErrorMessages.NotAllowRequest);


                var date = DateTime.Now.Date;
                var user = await Userservice.Table.Where(p => p.UserName == tokenRequest.username).SingleOrDefaultAsync(cancellationToken);
                if (user == null)
                    throw new BadRequestException(ErrorMessages.NotUserNameIsExists);

                var checkpass = PasswordHasher.VerifyHashedPasswordV2(user.PasswordHash, tokenRequest.password);
                if (!checkpass)
                    throw new BadRequestException(ErrorMessages.WrongUsernameOrPassword);

                //if (checkpass && !user.IsActive)
                //    throw new BadRequestException(ErrorMessages.DisActiveUser);



                var userClinet = await _userClientIdentityrepository.Table.Where(z => z.ClientIdentityId == clinet.Id && z.UserId == user.Id && z.Created > expriddate).ToListAsync(cancellationToken);
                await _userClientIdentityrepository.DeleteRangeAsync(userClinet, cancellationToken);

                var stamp = Guid.NewGuid();
                var refresh_token = Guid.NewGuid().ToString().Replace("-", "");

                await _userClientIdentityrepository.AddAsync(new UserClientIdentity() { ClientIdentityId = clinet.Id, UserId = user.Id, Stamp = stamp, refresh_token = refresh_token }, cancellationToken);

                user.LastLogin = DateTime.Now;
                await Userservice.UpdateAsync(user, cancellationToken);

                var roles = await UserRoleservice
                    .Table.Where(x => x.UserId == user.Id).Include(x => x.Role).ToListAsync(cancellationToken: cancellationToken);


                var jwt = await _jwtService.GenerateAsync(user, stamp, refresh_token, roles.Select(x => x.Role).ToList());

                return new ApiResult<AccessToken>(true, ApiResultStatusCode.Success, jwt);
            }
            else
            {
                var expriddate = DateTime.Now.AddDays(30);
                var clinet = await _clientIdentityrepository.TableNoTracking.SingleOrDefaultAsync(z => z.client_secret == tokenRequest.client_secret && z.client_id == tokenRequest.client_id, cancellationToken);
                if (clinet is null)
                    return new ApiResult<AccessToken>(false, ApiResultStatusCode.ServerError, null, ErrorMessages.NotAllowRequest);
                var getrefreshtoken = _userClientIdentityrepository.TableNoTracking.Where(z => z.refresh_token == tokenRequest.refresh_token)?.FirstOrDefault();
                if (getrefreshtoken is null)
                    return new ApiResult<AccessToken>(false, ApiResultStatusCode.ServerError, null, ErrorMessages.NotAllowRequest);

                var stamp = Guid.NewGuid();
                var refresh_token = Guid.NewGuid().ToString().Replace("-", "");
                await _userClientIdentityrepository.AddAsync(new UserClientIdentity() { ClientIdentityId = clinet.Id, UserId = getrefreshtoken.UserId, Stamp = stamp, refresh_token = refresh_token }, cancellationToken);
                await _userClientIdentityrepository.DeleteAsync(getrefreshtoken, cancellationToken);

                var user = await Userservice.Table.Where(p => p.Id == getrefreshtoken.UserId).SingleOrDefaultAsync(cancellationToken);

                var userClinet = await _userClientIdentityrepository.Table.Where(z => z.ClientIdentityId == clinet.Id && z.UserId == user.Id && z.Created > expriddate).ToListAsync(cancellationToken);
                await _userClientIdentityrepository.DeleteRangeAsync(userClinet, cancellationToken);

                user.LastLogin = DateTime.Now;
                await Userservice.UpdateAsync(user, cancellationToken);

                var roles = await UserRoleservice
                    .Table.Where(x => x.UserId == user.Id).Include(x => x.Role).ToListAsync(cancellationToken: cancellationToken);


                var jwt = await _jwtService.GenerateAsync(user, stamp, refresh_token, roles.Select(x => x.Role).ToList());

                return new ApiResult<AccessToken>(true, ApiResultStatusCode.Success, jwt);
            }

        }








        [HttpPost("[action]")]
        public virtual async Task<ApiResult> Logout(CancellationToken cancellationToken)
        {

            var userclient = await _userClientIdentityrepository.Table.Where(z => z.UserId == Userdata.Id).ToListAsync(cancellationToken);
            await _userClientIdentityrepository.DeleteRangeAsync(userclient, cancellationToken);
            return Ok();
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
            await SmsService.SendSms(dto.Username, string.Format(DataDictionary.sendSmsCode,code));


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
