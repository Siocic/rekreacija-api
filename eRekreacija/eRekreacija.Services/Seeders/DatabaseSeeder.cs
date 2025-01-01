using eRekreacija.Services.Database;
using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace eRekreacija.Services.Seeders
{
    public class DatabaseSeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole>roleManager,RekreacijaContext context)
        {
            await SeedSuperAdmin(userManager, roleManager);
            await SeedRole(roleManager);
            await SeedSportCategories(context);
        }
        private static async Task SeedSuperAdmin(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Enum.GetValues(typeof(Database.enums.Roles)))
            {
                if (!await roleManager.RoleExistsAsync(role.ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole(role.ToString()));
                }
            }

            var superAdmin = new ApplicationUser
            {
                UserName = "SuperAdmin",
                Email = "superadmin@email.com",
                FirstName = "Super",
                LastName = "Admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            if (userManager.Users.All(u => u.Email != superAdmin.Email))
            {
                var user = await userManager.FindByEmailAsync(superAdmin.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(superAdmin, "123Pa$$word");
                    await userManager.AddToRoleAsync(superAdmin, Database.enums.Roles.SuperAdmin.ToString());
                }
            }
        }
        private static async Task SeedRole(RoleManager<IdentityRole> roleManager)
        {
            var roles = new List<IdentityRole>
            {
                new IdentityRole{Name=Database.enums.Roles.SuperAdmin.ToString(),NormalizedName=Database.enums.Roles.SuperAdmin.ToString(),},
                new IdentityRole{Name=Database.enums.Roles.FizickoLice.ToString(),NormalizedName=Database.enums.Roles.FizickoLice.ToString(),},
                new IdentityRole{Name=Database.enums.Roles.PravnoLice.ToString(),NormalizedName=Database.enums.Roles.PravnoLice.ToString(),},
            };
            foreach (var role in roles) 
            {
                if(!await roleManager.RoleExistsAsync(role.Name))
                {
                    await roleManager.CreateAsync(role);
                }
            }
        }
        private static async Task SeedSportCategories(RekreacijaContext context)
        {
            var sport_categories = new List<tbl_SportCategory>
            {
                new tbl_SportCategory{name="Football"},
                new tbl_SportCategory{name="Basketball"},
                new tbl_SportCategory{name="Handball"},
                new tbl_SportCategory{name="Voleyball"},
                new tbl_SportCategory{name="Tennis"},  
            };

            foreach (var category in sport_categories)
            {
                if(!context.TblSportCategory.Any(c=>c.name==category.name))
                {
                    context.TblSportCategory.Add(category);
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
