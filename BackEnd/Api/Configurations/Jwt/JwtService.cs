

using BaseCore.Configuration;
using BaseCore.Utilities;
using Core.Entities.AAA;
using Core.Repositories.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Resources;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Configurations.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly SiteSettings _siteSetting;
        private readonly IRepository<User> _signInManager;

        public JwtService(IOptionsSnapshot<SiteSettings> settings, IRepository<User> signInManager)
        {
            _siteSetting = settings.Value;
            _signInManager = signInManager;
        }

        public async Task<AccessToken> GenerateAsync(User user, Guid stamp, string refresh_token, List<Role> roles)
        {
            var secretKey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.SecretKey); // longer that 16 character
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

            var encryptionkey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.Encryptkey); //must be 16 character
            var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

            var claims = await _getClaimsAsync(user, stamp, roles);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _siteSetting.JwtSettings.Issuer,
                Audience = _siteSetting.JwtSettings.Audience,
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(_siteSetting.JwtSettings.NotBeforeMinutes),
                Expires = DateTime.Now.AddMinutes(_siteSetting.JwtSettings.ExpirationMinutes),
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials,
                Subject = new ClaimsIdentity(claims)

            };

            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            //JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);

            //string encryptedJwt = tokenHandler.WriteToken(securityToken);


            return new AccessToken(securityToken, user.Guid, refresh_token, string.Join(",", roles.Select(z => z.RoleType).ToList()), user.Id);
        }

        private async Task<IEnumerable<Claim>> _getClaimsAsync(User user, Guid stamp, List<Role> roles)
        {
            var securityStampClaimType = new ClaimsIdentityOptions().SecurityStampClaimType;

            var list = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(securityStampClaimType, stamp.ToString())
            };

            foreach (var role in roles)
            {
                list.Add(new Claim(ClaimTypes.Role, "User"));

                if (role.RoleType == RoleType.Admin)
                {
                    list.Add(new Claim(ClaimTypes.Role, "Admin"));

                }



            }

            return list;
        }
    }
}
