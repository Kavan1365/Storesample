using BaseCore.Core;
using Core.Entities.Countries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Core.Entities.AAA
{
    [Table("User", Schema = "AAA")]
    public class User : BaseEntity
    {
        [MaxLength(500)]
        public string FirstName { get; set; }
        [MaxLength(500)]
        public string LastName { get; set; }
        [MaxLength(1000)]
        public string FullName { get; set; }

        /// <summary>
        /// براساس کد شماره موبایل
        /// </summary>
        [IndexColumn(IsUnique = true)]
        [MaxLength(11)]
        [Required]
        public string UserName { get; set; }
        [MaxLength(1000)]
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLogin { get; set; }
        #region infouser

        [MaxLength(100)]
        public string FatherName { get; set; }
        [MaxLength(11)]
        public string Phone { get; set; }

        [MaxLength(11)]
        public string Mobile { get; set; }

        [MaxLength(1000)]
        [EmailAddress]
        public string Email { get; set; }
        public DateTime? BirthDay { get; set; }

        [MaxLength(500)]
        public string ZipCode { get; set; }

        [MaxLength(1000)]
        public string Address { get; set; }
        public Sex Sex { get; set; }
        [MaxLength(10)]
        public string NationalCode { get; set; }
        /// <summary>
        /// شماره شناسنامه
        /// </summary>
        [MaxLength(10)]
        public string CertificateId { get; set; }


        public UserType UserType { get; set; }
        #endregion


        #region فیلد های اختصاصی

        public int? ImageId { get; set; }
        [ForeignKey(nameof(ImageId))]
        public virtual Base.File? Image { get; set; }

        public int? CityId { get; set; }
        [ForeignKey(nameof(CityId))]
        public virtual City? City { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        #endregion
    }
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
      .Property(e => e.FullName)
      .HasComputedColumnSql("FirstName+' '+LastName");
        }
    }


    public class ClientIdentity : BaseEntity
    {
        [MaxLength(500)]
        public string client_id { get; set; }
        [MaxLength(500)]
        public string client_secret { get; set; }
    }
    public class UserClientIdentity : BaseEntity
    {
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }


        public Guid Stamp { get; set; }
        [MaxLength(100)]
        public string refresh_token { get; set; }


        public int ClientIdentityId { get; set; }
        [ForeignKey(nameof(ClientIdentityId))]
        public virtual ClientIdentity ClientIdentity { get; set; }

    }




}
