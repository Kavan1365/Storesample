using BaseCore.Exceptions;
using BaseCore.Utilities;
using Core.Entities.AAA;
using Core.Repositories.Base;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Api.Configuration
{
    public static class ServiceCollectionExtensionsz
    {

        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<BaseContext>(options =>
            {
                options
                    .UseSqlServer(configuration.GetConnectionString("SqlServer"));
            });
        }
        public static void AddJwtAuthentication(this IServiceCollection services, JwtSettings jwtSettings)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var secretkey = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
                var encryptionkey = Encoding.UTF8.GetBytes(jwtSettings.Encryptkey);

                var validationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero, // default: 5 min
                    RequireSignedTokens = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretkey),

                    RequireExpirationTime = true,
                    ValidateLifetime = true,

                    ValidateAudience = true, //default : false
                    ValidAudience = jwtSettings.Audience,

                    ValidateIssuer = true, //default : false
                    ValidIssuer = jwtSettings.Issuer,

                    TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey)
                };

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = validationParameters;
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception != null)
                            throw new AppException(ApiResultStatusCode.UnAuthorized, "Authentication failed.", HttpStatusCode.Unauthorized, context.Exception, null);

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = async context =>
                    {
                        var userRepository = context.HttpContext.RequestServices.GetRequiredService<IRepository<User>>();
                        var userRoleRepository = context.HttpContext.RequestServices.GetRequiredService<IRepository<UserRole>>();
                        var userClientIdentityRepository = context.HttpContext.RequestServices.GetRequiredService<IRepository<UserClientIdentity>>();

                        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;

                        if (claimsIdentity.Claims?.Any() != true)
                            context.Fail("اطلاعات توکن خالی می باشد.");
                        var roles = claimsIdentity.RoleClaimType;


                        var securityStamp = claimsIdentity.FindFirstValue(new ClaimsIdentityOptions().SecurityStampClaimType);


                        if (!securityStamp.HasValue())
                            context.Fail("امضای توکن مشکل دارد");

                        //Find user and token from database and perform your custom validation
                        var userId = claimsIdentity.GetUserId<int>();
                        var user = await userRepository.GetByIdAsync(context.HttpContext.RequestAborted, userId);
                        if (user == null)
                            context.Fail("کاربر گرامی اکانت شما مشکل دارد لطفا با مدیر سایت تماس بگیرید.");

                        var role = await userRoleRepository.Table.Where(x => x.UserId == user.Id).ToListAsync(context.HttpContext.RequestAborted);

                        if (role == null)
                            context.Fail("کاربر گرامی شما به این درخواست درسترسی ندارید");

                        var userClientIdentity = await userClientIdentityRepository.TableNoTracking.Where(z => z.UserId == user.Id).ToListAsync(context.HttpContext.RequestAborted);

                        if (userClientIdentity is null)
                            context.Fail("امضای توکن مشکل دارد");

                        if (userClientIdentity.Count() == 0)
                            context.Fail("امضای توکن مشکل دارد");

                        if (userClientIdentity?.Count() > 0 && !userClientIdentity.Any(z => z.Stamp == Guid.Parse(securityStamp)))
                            context.Fail("امضای توکن مشکل دارد");


                    },
                    OnChallenge = context =>
                    {

                        if (context.AuthenticateFailure != null)
                            throw new AppException(ApiResultStatusCode.UnAuthorized, "Authenticate failure.", HttpStatusCode.Unauthorized, context.AuthenticateFailure, null);
                        throw new AppException(ApiResultStatusCode.UnAuthorized, "You are unauthorized to access this resource.", HttpStatusCode.Unauthorized);

                    }
                };
            });
        }


    }
}
