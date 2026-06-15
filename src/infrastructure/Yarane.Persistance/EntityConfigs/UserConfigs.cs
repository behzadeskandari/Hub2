using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yarane.Domain.Entities;
using Yarane.Domain.Enums;
using System.Reflection.Emit;

namespace Yarane.Persistance.EntityConfigs
{
    public class UserConfigs : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.FirstName).HasMaxLength(100).IsRequired(false);
            builder.Property(p => p.LastName).HasMaxLength(100).IsRequired(false);
            builder.Property(x => x.UserName).HasMaxLength(50).IsRequired(false);
            builder.Property(x => x.Password).HasMaxLength(50).IsRequired(false);
            builder.Property(x => x.Address).HasMaxLength(250).IsRequired(false);
            builder.Property(x => x.Otp).IsRequired(false);
            builder.Property(x => x.CreateAt).IsRequired(false);
            builder.Property(x => x.ModifiedAt).IsRequired(false);
            builder.Property(x => x.Email).IsRequired(false);
            builder.Property(x => x.EmailActiveCode).IsRequired(false);
            builder.Property(x => x.IsActivated).IsRequired(false);
            builder.Property(x => x.Phone).IsRequired(false);
            builder.Property(x => x.Status).IsRequired(false);


            builder.ToTable("User");
        }
    }
}
