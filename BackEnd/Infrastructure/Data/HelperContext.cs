using BaseCore.Core;
using BaseCore.Core.AAA;
using BaseCore.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Toolbelt.ComponentModel.DataAnnotations;

namespace Infrastructure.Data
{
    public static class HelperContext
    {

        public static void AddExtraFields(DbContext context)
        {
            var entities = context.ChangeTracker.Entries().Where(x => x.Entity is BaseEntity).ToList();
            var userData = UserDataValue.getUserData();

            foreach (var entity in entities)
            {


                if (entity.State == EntityState.Added)
                {
                    ((IBaseEntity)entity.Entity).Created = DateTime.Now;
                    if (userData?.Id != 0)
                        ((IBaseEntity)entity.Entity).CreatedByUserId = userData?.Id;
                }


                if (entity.State == EntityState.Modified)
                {
                    ((IBaseEntity)entity.Entity).Modified = DateTime.Now;
                    if (userData?.Id != 0)
                        ((IBaseEntity)entity.Entity).ModifiedByUserId = userData?.Id;
                }



            }
        }




    }
}
