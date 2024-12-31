using eRekreacija.Services.Database;
using Microsoft.AspNetCore.Identity;

namespace eRekreacija.Services.Seeders
{
    public class DatabaseSeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole>roleManager)
        {
            await SeedSuperAdmin(userManager, roleManager);
            await SeedRole(roleManager);
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
    }
}
