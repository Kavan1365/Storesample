using BaseCore.Configuration;
using Core.Entities.AAA;

namespace Api.Configurations.Jwt
{
    public interface IJwtService
    {
        Task<AccessToken> GenerateAsync(User user, Guid stamp, string refresh_token, List<Role> roles);
    }
}
