using eRekreacija.Services.Database;
using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            await SeedObjects(context);
            await SeedSportCategoriesObjects(context);
            await SeedReview(context);
            await SeedNotification(context);
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
                Id = "6e763a89-5198-45af-a8f2-c7b3d3840582",
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

            var fizickoLice = new List<ApplicationUser>
            {
                new ApplicationUser{Id="326aa2d9-36a5-41e7-ab17-2339db9d7dbb",isApproved=true,UserName="FizickoLice",Email="fizickolice@email.com",FirstName="Fizicko",LastName="Lice",Address="Ulica bb",City="Tuzla",PhoneNumber="412414",EmailConfirmed=true,PhoneNumberConfirmed=true},
                new ApplicationUser{Id="8b5f3087-4554-497f-9cd8-df61793e083a",isApproved=true,UserName="Faris",Email="faris.siocic@edu.fit.ba",FirstName="Faris",LastName="Siocic",Address="Ulica bb",City="Tuzla",PhoneNumber="432432",EmailConfirmed=true,PhoneNumberConfirmed=true},
                new ApplicationUser{Id="ce9e09e0-29c7-4ef5-9a76-c020dae967f5",isApproved=true,UserName="Goran",Email="goran@email.com",FirstName="Goran",LastName="Goranovic",Address="Ulica bb",City="Tuzla",PhoneNumber="2321312",EmailConfirmed=true,PhoneNumberConfirmed=true},
                new ApplicationUser{Id="2a38f91f-00fe-4161-b694-77f30f1d4036",isApproved=true,UserName="Mujo",Email="mujo@email.com",FirstName="Mujo",LastName="Mujic",Address="Ulica bb",City="Tuzla",PhoneNumber="321312",EmailConfirmed=true,PhoneNumberConfirmed=true},
                new ApplicationUser{Id="fef4c815-0e26-495c-9372-5b702e148915",isApproved=true,UserName="Nikola",Email="nikola@email.com",FirstName="Nikola",LastName="Nikolic",Address="Ulica bb",City="Tuzla",PhoneNumber="4342323",EmailConfirmed=true,PhoneNumberConfirmed=true},
                new ApplicationUser{Id="d70faa87-9cf2-4cd7-808f-81e34b8dee04",isApproved=true,UserName="Ismir",Email="ismir@email.com",FirstName="Ismir",LastName="Zukic",Address="Ulica bb",City="Tuzla",PhoneNumber="42342343",EmailConfirmed=true,PhoneNumberConfirmed=true},
            };

            foreach (var f in fizickoLice)
            {
                if (userManager.Users.All(u => u.Email != f.Email))
                {
                    var user = await userManager.FindByEmailAsync(f.Email);
                    if (user == null)
                    {
                        await userManager.CreateAsync(f, "123Pa$$word");
                        await userManager.AddToRoleAsync(f, Database.enums.Roles.FizickoLice.ToString());
                    }
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

            var pravnoLice = new List<ApplicationUser>
            {
                new ApplicationUser{Id="b3fd38e0-033f-4069-b068-415841a74e78", isApproved = true, UserName = "PravnoLice", Email = "pravnolice@email.com",FirstName = "Pravno", LastName = "Lice", Address = "Ulica bb", City = "Tuzla", PhoneNumber = "13579", EmailConfirmed = true, PhoneNumberConfirmed = true },
                new ApplicationUser{Id="d42f255c-d841-4d09-8f6e-f195a1c98d10",isApproved = true, UserName ="Ishak",Email="ishak.isabegoic@edu.fit.ba",FirstName="Ishak",LastName="Isabegovic",Address="Ulica bb",City="Tuzla",PhoneNumber="32131", EmailConfirmed = true, PhoneNumberConfirmed = true },
                new ApplicationUser{Id="b86e93f5-b543-48eb-885f-ad0f8edbf257",isApproved = true, UserName ="Pero",Email="pero@email.com",FirstName="Pero",LastName="Peric",Address="Ulica bb",City="Tuzla",PhoneNumber="12123", EmailConfirmed = true, PhoneNumberConfirmed = true },
                new ApplicationUser{Id="2960f5c7-e10c-489a-8958-df60e91b4469",isApproved=false,UserName="Karlo",Email="karlo@email.com",FirstName="Karlo",LastName="Ivic",Address="Ulica bb",City="Tuzla",PhoneNumber="4324234",EmailConfirmed=true,PhoneNumberConfirmed=true},
                new ApplicationUser{Id="5af93ac0-389d-43ec-a6b7-ff58f1b4410c",isApproved = true, UserName ="Berun",Email="berun@email.com",FirstName="Berun",LastName="Agic",Address="Ulica bb",City="Tuzla",PhoneNumber="312414", EmailConfirmed = true, PhoneNumberConfirmed = true },
                new ApplicationUser{Id="4bfed474-0c5a-4c8a-a0bc-8e0198c1d512",isApproved=false,UserName="Skopak",Email="faris@email.com",FirstName="Faris",LastName="Skopak",Address="Ulica bb",City="Tuzla",PhoneNumber="12525432",EmailConfirmed=true,PhoneNumberConfirmed=true},
                new ApplicationUser{Id="bae2b802-2b97-4332-8534-9526d602fb29",isApproved = true, UserName ="Adi",Email="adi@email.com",FirstName="Adi",LastName="Efendic",Address="Ulica bb",City="Tuzla",PhoneNumber="1423423", EmailConfirmed = true, PhoneNumberConfirmed = true },
                new ApplicationUser{Id="84a4952f-20e4-40d8-bd0d-5534943ff3cb",isApproved = true, UserName ="Semir",Email="semir@email.com",FirstName="Semir",LastName="Nisic",Address="Ulica bb",City="Tuzla",PhoneNumber="3423423", EmailConfirmed = true, PhoneNumberConfirmed = true },
                new ApplicationUser{Id="d4801fe1-11b2-43ba-9dbd-c0e59be103ca",isApproved = true, UserName ="Omer",Email="omer@email.com",FirstName="Omer",LastName="Mehanovic",Address="Ulica bb",City="Tuzla",PhoneNumber="423432", EmailConfirmed = true, PhoneNumberConfirmed = true },
                new ApplicationUser{Id="4b1e8c22-4999-4fc8-9290-bee54506b376",isApproved = true, UserName ="Hamza",Email="hamza@email.com",FirstName="Hamaz",LastName="Husni",Address="Ulica bb",City="Tuzla",PhoneNumber="324324", EmailConfirmed = true, PhoneNumberConfirmed = true },
            };

            foreach (var pL in pravnoLice)
            {
                if (userManager.Users.All(u => u.Email != pL.Email))
                {
                    var user = await userManager.FindByEmailAsync(pL.Email);
                    if (user == null)
                    {
                        await userManager.CreateAsync(pL, "123Pa$$word");
                        await userManager.AddToRoleAsync(pL, Database.enums.Roles.PravnoLice.ToString());
                    }
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
        private static async Task SeedObjects(RekreacijaContext context)
        {
            var objects = new List<tbl_Objects>
            {
                new tbl_Objects{id=1,name="Dvorana Mejdan",created_date=DateTime.Now,address="Bosne Srebrene",city="Tuzla",description="Moderna sportska dvorana pogodna za odbojku, mali fudbal, kosarku i rukomet. Odlicna rasvjeta i profesionalna podloga cije je idealnom za rekreatice i takmicenja",price=100,user_id="b3fd38e0-033f-4069-b068-415841a74e78"},
                new tbl_Objects{id=2,name="Univerzitetska dvorana",created_date=DateTime.Now,address="Univerzitetska 2",city="Tuzla",description="Moderna dvorana u sklopu univerzitetskog kampusa, koristi se za odbojku, košarku i rekreativni fudbal. Često domaćin studentskih turnira i sportskih događaja.",price=100,user_id="4b1e8c22-4999-4fc8-9290-bee54506b376"},
                new tbl_Objects{id=3,name="Stadion Tušanj",created_date=DateTime.Now,address="Rudarska bb",city="Tuzla",description="Mali pomocni tereni na stadionu Tusanj. Teren sa vjestackom travom, specijalno prilagođen za rekreativni mali fudbal",price=80,user_id="d4801fe1-11b2-43ba-9dbd-c0e59be103ca"},
                new tbl_Objects{id=4,name="ETS Dvorana",created_date=DateTime.Now,address="Husinskih rudara",city="Tuzla",description="Prostrana dvorana namijenjena isključivo za košarku",price=60,user_id="bae2b802-2b97-4332-8534-9526d602fb29"},
                new tbl_Objects{id=5,name="Dvorana Meša Selimović",created_date=DateTime.Now,address="Tihomila Markovića",city="Tuzla",description="Višenamjenska dvorana u kojoj se igra rukomet, odbojka i mali fudbal. Ima svlačionice i parking ispred objekta.",price=60,user_id="5af93ac0-389d-43ec-a6b7-ff58f1b4410c"},
                new tbl_Objects{id=6,name="Dvorana Ismet Mujezinović",created_date=DateTime.Now,address="Franjevačka",city="Tuzla",description="Višenamjenska sportska dvorana sa savremenom opremom. Pogodna za košarku, odbojku i rekreativni rukomet",price=50,user_id="5af93ac0-389d-43ec-a6b7-ff58f1b4410c"},
                new tbl_Objects{id=7,name="Teniski Tereni Banja",created_date=DateTime.Now,address="Šetalište Slana banja",city="Tuzla",description="Dva otvorena šljakaška terena, idealna za rekreativne i klupske mečeve. Tereni se redovno održavaju.",price=100,user_id="d42f255c-d841-4d09-8f6e-f195a1c98d10"},
                new tbl_Objects{id=8,name="Dvorana Katolički školski centar",created_date=DateTime.Now,address="Klosterska",city="Tuzla",description="Zatvorena dvorana srednje veličine, idealna za timske sportove poput odbojke i malog fudbala. Opremljena svlačionicama, LED rasvjetom i ventilacijom, često je izbor za večernje termine.",price=80,user_id="b86e93f5-b543-48eb-885f-ad0f8edbf257"},
                new tbl_Objects{id=9,name="Dramar Tenis",created_date=DateTime.Now,address=" 1. tuzlanske brigade",city="Tuzla",description="Smješten u mirnom dijelu grada, ovaj otvoreni teniski teren sa šljakom nudi idealne uslove za rekreativnu i takmičarsku igru.",price=100,user_id="d42f255c-d841-4d09-8f6e-f195a1c98d10"},
                new tbl_Objects{id=10,name="Odbojkaska dvorana",created_date=DateTime.Now,address="Slatina bb",city="Tuzla",description="Savremena dvorana namijenjena iskljucivo za odbojku. Opremljena profesionalnim mrezama i savrsenom rasvjetom za vecernje utakmice.",price=80,user_id="84a4952f-20e4-40d8-bd0d-5534943ff3cb"},
            };

            var connection = context.Database.GetDbConnection();
            await connection.OpenAsync();
            var command = connection.CreateCommand();

            command.CommandText = "SET IDENTITY_INSERT tbl_Objects ON";
            await command.ExecuteNonQueryAsync();

            foreach (var obj in objects)
            {
                if (!context.TblObject.Any(c => c.id == obj.id))
                {
                    context.TblObject.Add(obj);
                }
            }
            await context.SaveChangesAsync();

            command.CommandText = "SET IDENTITY_INSERT tbl_Objects OFF";
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();

        }
        private static async Task SeedSportCategoriesObjects(RekreacijaContext context)
        {
            var sport_categoires_objects = new List<tbl_ObjectSportCategory>
            {
                new tbl_ObjectSportCategory{object_id=1,sport_category_id=1},
                new tbl_ObjectSportCategory{object_id=1,sport_category_id=2},
                new tbl_ObjectSportCategory{object_id=1,sport_category_id=3},
                new tbl_ObjectSportCategory{object_id=1,sport_category_id=4},
                new tbl_ObjectSportCategory{object_id=2,sport_category_id=1},
                new tbl_ObjectSportCategory{object_id=2,sport_category_id=2},
                new tbl_ObjectSportCategory{object_id=3,sport_category_id=1},
                new tbl_ObjectSportCategory{object_id=4,sport_category_id=2},
                new tbl_ObjectSportCategory{object_id=5,sport_category_id=3},
                new tbl_ObjectSportCategory{object_id=5,sport_category_id=4},
                new tbl_ObjectSportCategory{object_id=5,sport_category_id=1},
                new tbl_ObjectSportCategory{object_id=6,sport_category_id=3},
                new tbl_ObjectSportCategory{object_id=6,sport_category_id=4},
                new tbl_ObjectSportCategory{object_id=6,sport_category_id=1},
                new tbl_ObjectSportCategory{object_id=7,sport_category_id=5},
                new tbl_ObjectSportCategory{object_id=8,sport_category_id=4},
                new tbl_ObjectSportCategory{object_id=8,sport_category_id=1},
                new tbl_ObjectSportCategory{object_id=9,sport_category_id=5},
                new tbl_ObjectSportCategory{object_id=10,sport_category_id=4},
            };

            foreach (var so in sport_categoires_objects)
            {
                if (!context.TblObjectSportCategory.Any(c => c.object_id == so.object_id && c.sport_category_id == c.sport_category_id))
                {
                    context.TblObjectSportCategory.Add(so);
                }
            }
            await context.SaveChangesAsync();
        }
        private static async Task SeedReview(RekreacijaContext context)
        {
            var review = new List<tbl_Review>
            {
                new tbl_Review{id=1,comment="Savršena dvorana! Sve je čisto, moderno i odlično organizovano. Definitivno se vraćam!",created_date=DateTime.Now,rating=5,user_id="326aa2d9-36a5-41e7-ab17-2339db9d7dbb",object_id=1},
                new tbl_Review{id=2,comment="Vrlo ljubazno osoblje, odlična rasvjeta i kvalitetan parket. Sve preporuke!",created_date=DateTime.Now,rating=5,user_id="d70faa87-9cf2-4cd7-808f-81e34b8dee04",object_id=2},
                new tbl_Review{id=3,comment="Igrali smo turnir i sve je prošlo bez greške. Odlična akustika i klimatizacija.",created_date=DateTime.Now,rating=5,user_id="d70faa87-9cf2-4cd7-808f-81e34b8dee04",object_id=5},
                new tbl_Review{id=4,comment="Jedna od boljih dvorana u gradu. Ima sve što treba za ozbiljan trening.",created_date = DateTime.Now, rating=5,user_id="2a38f91f-00fe-4161-b694-77f30f1d4036",object_id=1},
                new tbl_Review{id=5,comment="Super atmosfera i dobra oprema. Rezervacija termina ide glatko.",created_date = DateTime.Now, rating=5,user_id="326aa2d9-36a5-41e7-ab17-2339db9d7dbb",object_id=8},
                new tbl_Review{id=6,comment="Vrlo dobra dvorana, ali svlačionice bi mogle biti malo modernije.",created_date=DateTime.Now,rating=3,user_id="ce9e09e0-29c7-4ef5-9a76-c020dae967f5",object_id=6},
                new tbl_Review{id=7,comment="Generalno zadovoljan, samo ponekad bude gužva na parkingu.",created_date = DateTime.Now, rating=4,user_id="8b5f3087-4554-497f-9cd8-df61793e083a",object_id=1},
                new tbl_Review{id=8,comment="Dvorana je okej, ali mreže za odbojku su bile malo oštećene.",created_date = DateTime.Now, rating=4,user_id="ce9e09e0-29c7-4ef5-9a76-c020dae967f5",object_id=10},
                new tbl_Review{id=9,comment="Sve je bilo super, samo bi rasvjeta mogla biti bolja u večernjim terminima.",created_date = DateTime.Now, rating=4,user_id="ce9e09e0-29c7-4ef5-9a76-c020dae967f5",object_id=9},
                new tbl_Review{id=10,comment="Lijep prostor i dobra organizacija, ali klima nije uvijek dovoljno jaka.",created_date = DateTime.Now, rating=4,user_id="2a38f91f-00fe-4161-b694-77f30f1d4036",object_id=5},
                new tbl_Review{id=11,comment="Teren je savršeno održavan, mreža čvrsta, idealno za rekreaciju i ozbiljan tenis.",created_date = DateTime.Now, rating=5,user_id="d70faa87-9cf2-4cd7-808f-81e34b8dee04",object_id=7},
                new tbl_Review{id=12,comment="Teren je ok, ali okolina nije baš uređena. Svejedno fino smo odigrali.",created_date = DateTime.Now, rating=3,user_id="2a38f91f-00fe-4161-b694-77f30f1d4036",object_id=9},
                new tbl_Review{id=13,comment="Trebalo bi da se linije češće obnavljaju, ali generalno dobar teren.",created_date = DateTime.Now, rating=3,user_id="ce9e09e0-29c7-4ef5-9a76-c020dae967f5",object_id=7},
                new tbl_Review{id=14,comment="Nedovoljno svlačionica za veće grupe. Teren je okej.",created_date = DateTime.Now, rating=2,user_id="ce9e09e0-29c7-4ef5-9a76-c020dae967f5",object_id=10},
                new tbl_Review{id=15,comment="Solidno iskustvo, ali bi mogli srediti bolje označene linije za različite sportove.",created_date = DateTime.Now, rating=2,user_id="326aa2d9-36a5-41e7-ab17-2339db9d7dbb",object_id=4},
                new tbl_Review{id=16,comment="Odličan mali teren! Idealna podloga i super rasvjeta za večernje termine.",created_date = DateTime.Now, rating=5,user_id="8b5f3087-4554-497f-9cd8-df61793e083a",object_id=3},
                new tbl_Review{id=17,comment="Teren je solidan, linije su jasno označene, ali mreža na golovima bi mogla biti u boljem stanju.",created_date = DateTime.Now, rating=4,user_id="d70faa87-9cf2-4cd7-808f-81e34b8dee04",object_id=3},
                new tbl_Review{id=18,comment="Savršeno mjesto za rekreativno igranje sa ekipom! Rezervacija je bila brza i bez problema.",created_date = DateTime.Now, rating=5,user_id="326aa2d9-36a5-41e7-ab17-2339db9d7dbb",object_id=3},
            };

            var connection = context.Database.GetDbConnection();
            await connection.OpenAsync();
            var command = connection.CreateCommand();

            command.CommandText = "SET IDENTITY_INSERT tbl_Review ON";
            await command.ExecuteNonQueryAsync();

            foreach (var r in review)
            {
                if (!context.TblReview.Any(c => c.id == r.id))
                {
                    context.TblReview.Add(r);
                }
            }
            await context.SaveChangesAsync();

            command.CommandText = "SET IDENTITY_INSERT tbl_Review OFF";
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }
        private static async Task SeedNotification(RekreacijaContext context)
        {
            var notification = new List<tbl_Notification>
            {
                new tbl_Notification{id=1,name="Nova oprema dostupna",description="Dvorana je opremljena novim mrežama, loptama i rekvizitima – slobodno ih koristite tokom termina.",created_date=DateTime.Now,user_id="b3fd38e0-033f-4069-b068-415841a74e78"},
                new tbl_Notification{id=2,name="Specijalni popusti za grupe",description="Za grupe veće od 10 osoba odobravamo popust od 20% na redovne cijene termina.",created_date=DateTime.Now,user_id="d4801fe1-11b2-43ba-9dbd-c0e59be103ca"},
                new tbl_Notification{id=3,name="Besplatan Wi-Fi za korisnike",description="Nova pogodnost za sve posjetioce – besplatan Wi-Fi dostupan u cijelom prostoru dvorane.",created_date=DateTime.Now,user_id="4b1e8c22-4999-4fc8-9290-bee54506b376"},
                new tbl_Notification{id=4,name="Nova rasvjeta u glavnoj dvorani",description="Ugrađena LED rasvjeta za još bolje osvjetljenje i udobniji boravak – dođite i isprobajte!",created_date=DateTime.Now,user_id="d42f255c-d841-4d09-8f6e-f195a1c98d10"},
            };

            var connection = context.Database.GetDbConnection();
            await connection.OpenAsync();
            var command = connection.CreateCommand();

            command.CommandText = "SET IDENTITY_INSERT tbl_Notification ON";
            await command.ExecuteNonQueryAsync();

            foreach (var n in notification)
            {
                if (!context.TblNotification.Any(c => c.id == n.id))
                {
                    context.TblNotification.Add(n);
                }
            }
            await context.SaveChangesAsync();

            command.CommandText = "SET IDENTITY_INSERT tbl_Notification OFF";
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }
    }
}
