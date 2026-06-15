using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Yarane.Domain.Entities;
using Yarane.Persistance.EntityConfigs;

namespace Yarane.Persistance.Contexts
{
    public class YaraneDbContext : IdentityDbContext<Users>
    {
        public YaraneDbContext(DbContextOptions<YaraneDbContext> options) : base(options)
        {
        }

        //public virtual DbSet<AccessRule> AccessRules { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        //public virtual DbSet<PasswordHistory> PasswordHistory { get; set; }
        //public virtual DbSet<UserLog> UserLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //YaraneDbContextHelpers.EntityConfigurations(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
