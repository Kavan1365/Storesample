using BaseCore.EFCore;
using BaseCore.Utilities;
using Core;
using Core.Entities.AAA;
using Core.Entities.Base;
using Core.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Resources;

namespace Api.Configuration
{
    public class UserDataInitializer : IDataInitializer
    {

        private readonly IRepository<User> userManager;
        private readonly IRepository<Role> roleManager;
        private readonly IRepository<ClientIdentity> clientIdentity;
        private readonly IRepository<UserRole> userRoleManager;

        public UserDataInitializer(IRepository<User> userManager,
         IRepository<UserRole> userRoleManager,
         IRepository<ClientIdentity> clientIdentity,
          IRepository<Role> roleManager
            )
        {
            this.clientIdentity = clientIdentity;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.userRoleManager = userRoleManager;

        }
        public void InitializeDataAsync()
        {

            if (!roleManager.Table.AsNoTracking().Any(p => p.Id == 1))
            {
                var role = new Role
                {
                    Title = "مدیر",
                    RoleType = RoleType.Admin,
                };
                roleManager.Add(role);

                var role1 = new Role
                {
                    Title = "کاربر",
                    RoleType = RoleType.User,
                };
                roleManager.Add(role1);
                var client = new ClientIdentity
                {
                    client_id= "Admin_UI",
                    client_secret= "98fa8e277ccb46b9a518284eba50f671",
                    
                };
                var client1 = new ClientIdentity
                {
                    client_id= "Client_UI",
                    client_secret= "98fa8e277ccb46b9a518284eba50f671",
                    
                };
                clientIdentity.Add(client);
                clientIdentity.Add(client1);


            }


            if (!userManager.Table.AsNoTracking().Any(p => p.Id == 1))
            {
                var user = new User
                {
                    FirstName = "کاوان",
                    LastName = "احمدی",
                    UserName = "09187708317",
                   PasswordHash = PasswordHasher.HashPasswordV2("123456"),
                };
                userManager.Add(user);
            }

            if (!userRoleManager.Table.AsNoTracking().Any(p => p.Id == 1))
            {
                userRoleManager.Add(new UserRole() { RoleId = 1, UserId = 1 });
            }
        }
    }


    public class SettingDataInitializer : IDataInitializer
    {

        private readonly IRepository<Setting> settingManager;

        public SettingDataInitializer(IRepository<Setting> settingManager
            )
        {
            this.settingManager = settingManager;

        }
        public void InitializeDataAsync()
        {


            if (!settingManager.Table.AsNoTracking().Any())
            {
                var user = new Setting
                {
                  Title=string.Empty,
                  Url=string.Empty,
                  Boxchat=string.Empty,
                };
                settingManager.Add(user);
            }

            
        }
    }


}
