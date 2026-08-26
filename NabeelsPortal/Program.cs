using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NabeelsPortal.Data;
using NabeelsPortal.Models;
using NabeelsPortal.Services;
namespace NabeelsPortal
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Set the data directory to the directory of the application's executable file
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);

            var connectionString = builder.Configuration.GetConnectionString("MyDatabaseConnection") ?? throw new InvalidOperationException("Connection string 'AgriEnergyContextConnection' not found.");

            builder.Services.AddDbContext<AgriEnergyContext>(options => options.UseSqlServer(connectionString));

            //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AgriEnergyContext>();

            // Add Identity services
            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.User.RequireUniqueEmail = true; // Ensures each email is unique.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"; // Allow more characters in username.
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                .AddRoles<IdentityRole>() // Add roles support
                .AddEntityFrameworkStores<AgriEnergyContext>();


            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IFarmerService, FarmerService>();

            // Configure authentication cookie
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = context =>
                    {
                        if (context.Request.Path.StartsWithSegments("/api") && context.Response.StatusCode == 200)
                        {
                            context.Response.StatusCode = 401;
                            return Task.CompletedTask;
                        }
                        context.Response.Redirect(context.RedirectUri);
                        return Task.CompletedTask;
                    },
                    OnRedirectToAccessDenied = context =>
                    {
                        if (context.Request.Path.StartsWithSegments("/api") && context.Response.StatusCode == 200)
                        {
                            context.Response.StatusCode = 403;
                            return Task.CompletedTask;
                        }
                        context.Response.Redirect(context.RedirectUri);
                        return Task.CompletedTask;
                    }
                };
            });

            var app = builder.Build();

            // Ensure database is created and roles are seeded
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AgriEnergyContext>();
                    context.Database.EnsureCreated();

                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                    await SeedRolesAsync(roleManager);
                    await SeedUsersAsync(userManager, services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<CustomAuthorizationMiddleware>();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();

            // Seed roles and users
            static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
            {
                if (!await roleManager.RoleExistsAsync("Farmer"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Farmer"));
                }
                if (!await roleManager.RoleExistsAsync("Employee"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Employee"));
                }
            }

            static async Task SeedUsersAsync(UserManager<IdentityUser> userManager, IServiceProvider serviceProvider)
            {
                var users = new List<(string UserName, string Email, string Password, string Role, string FullName, string ContactInfo, string Address)>
    {
        ("farmer1", "farmer1@example.com", "Password123!", "Farmer", "Farmer One", "076-528-3568", "123 Elm Street"),
        ("farmer2", "farmer2@example.com", "Password123!", "Farmer", "Farmer Two", "068-664-3240", "25 Long Street"),
        ("employee1", "employee1@example.com", "Password123!", "Employee", "Employee One", "088-254-2500", "254 Phil Street"),
        ("employee2", "employee2@example.com", "Password123!", "Employee", "Employee Two", "074-587-9956", "54 Kennedy Street")
    };

                foreach (var (userName, email, password, role, fullName, contactInfo, address) in users)
                {
                    if (userManager.Users.All(u => u.UserName != userName))
                    {
                        var user = new IdentityUser { UserName = userName, Email = email, EmailConfirmed = true };
                        var result = await userManager.CreateAsync(user, password);
                        if (result.Succeeded)
                        {
                            user = await userManager.FindByEmailAsync(email);
                            await userManager.AddToRoleAsync(user, role);

                            if (role == "Farmer")
                            {
                                using (var scope = serviceProvider.CreateScope())
                                {
                                    var context = scope.ServiceProvider.GetRequiredService<AgriEnergyContext>();
                                    var farmer = new Farmer
                                    {
                                        FarmerId = user.Id,
                                        Name = fullName,
                                        ContactInfo = contactInfo,
                                        Address = address,
                                        Email = email
                                    };
                                    context.Farmers.Add(farmer);
                                    await context.SaveChangesAsync();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
