using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRekreacija.Services.Database.Context
{
    public class IdentityContext:IdentityDbContext
    {
        public IdentityContext(DbContextOptions<IdentityContext>options):base(options) { }
        
            public DbSet<User> User {  get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("Identity");
            builder.Entity<IdentityUser>(entity =>
            {
                entity.ToTable(name: "User");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name=enums.Roles.SuperAdmin.ToString(),NormalizedName=enums.Roles.SuperAdmin.ToString().ToUpper() },
               new IdentityRole { Name = enums.Roles.FizickoLice.ToString(), NormalizedName = enums.Roles.FizickoLice.ToString().ToUpper() },
               new IdentityRole { Name = enums.Roles.PravnoLice.ToString(), NormalizedName = enums.Roles.PravnoLice.ToString().ToUpper() }
               );
        }

    }
}
