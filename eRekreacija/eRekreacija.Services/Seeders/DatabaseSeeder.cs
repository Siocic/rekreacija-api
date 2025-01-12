using eRekreacija.Services.Database;
using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Database.Entities;
using Microsoft.AspNetCore.Identity;

namespace eRekreacija.Services.Seeders
{
    public class DatabaseSeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, RekreacijaContext context)
        {
            await SeedSuperAdmin(userManager, roleManager);
            await SeedFizickoLice(userManager, roleManager);
            await SeedPravnoLice(userManager, roleManager);
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
                Address = "Address 1",
                City = "City 1",
                PhoneNumber = "123456",
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
        private static async Task SeedFizickoLice(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Enum.GetValues(typeof(Database.enums.Roles)))
            {
                if (!await roleManager.RoleExistsAsync(role.ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole(role.ToString()));
                }
            }

            var fizickoLice = new ApplicationUser
            {
                UserName = "FizickoLice",
                Email = "fizickolice@email.com",
                FirstName = "Fizicko",
                LastName = "Lice",
                Address = "Address 2",
                City = "City 2",
                PhoneNumber = "246810",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            if (userManager.Users.All(u => u.Email != fizickoLice.Email))
            {
                var user = await userManager.FindByEmailAsync(fizickoLice.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(fizickoLice, "123Pa$$word");
                    await userManager.AddToRoleAsync(fizickoLice, Database.enums.Roles.FizickoLice.ToString());
                }
            }
        }
        private static async Task SeedPravnoLice(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Enum.GetValues(typeof(Database.enums.Roles)))
            {
                if (!await roleManager.RoleExistsAsync(role.ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole(role.ToString()));
                }
            }

            var pravnoLice = new ApplicationUser
            {
                UserName = "PravnoLice",
                Email = "pravnolice@email.com",
                FirstName = "Pravno",
                LastName = "Lice",
                Address = "Address 3",
                City = "City 3",
                PhoneNumber = "13579",
                isApproved = true,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            if (userManager.Users.All(u => u.Email != pravnoLice.Email))
            {
                var user = await userManager.FindByEmailAsync(pravnoLice.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(pravnoLice, "123Pa$$word");
                    await userManager.AddToRoleAsync(pravnoLice, Database.enums.Roles.PravnoLice.ToString());
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
                if (!await roleManager.RoleExistsAsync(role.Name))
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
                if (!context.TblSportCategory.Any(c => c.name == category.name))
                {
                    context.TblSportCategory.Add(category);
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
