using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace BaseCore.Core.AAA
{

    public class UserData
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string ImageUrl { get; set; }
        public Guid Guid { get; set; }
        public string Roles { get; set; }
    }


    public class UserDataValue
    {
        private static IHttpContextAccessor httpContextAccessor;
        public static void SetHttpContextAccessor(IHttpContextAccessor accessor)
        {
            httpContextAccessor = accessor;
        }

        public static UserData _UserData;
        public static UserData getUserData()
        {
            if (_UserData == null)
            {

                if (httpContextAccessor != null && httpContextAccessor.HttpContext != null && httpContextAccessor.HttpContext.User.Identities.FirstOrDefault().Claims.Any(x => x.Type == ClaimTypes.Name))
                {
                    return new UserData()
                    {
                        Id = int.Parse(httpContextAccessor.HttpContext.User.Identities.FirstOrDefault().Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value),
                    };
                }
                else
                {
                    return new UserData();
                }

            }

            return new UserData();

        }
    }
}
