using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Transportation.Buisness._0.Common.FileManager;
using Transportation.Buisness.Contracts.Identity;
using Transportation.DataAccess._0.Common;
using Transportation.DataAccess.Contexts;
using Transportation.DataAccess.FileManager;
using Transportation.DataAccess.Identity;
using Transportation.DataAccess.SeedManager.Seeds;
using Transportation.DataAccess.SeedManager.Settings;
using Transportation.Entities._0.Common;
using Transportation.Entities.Entities;

namespace Transportation.DataAccess
{
    public static class DIDataAccessBootstrapper
    {
        public static IServiceCollection AddDataAccess(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            services.AddDatabase(configuration);
            services.AddIdentityConfig();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IFileService, FileService>();
            services.Configure<AdminSeedSettings>(configuration.GetSection(AdminSeedSettings.SectionName));

            return services;
        }

        public static async Task UseDataAccessAsync(this WebApplication app)
        {
            await app.SeedDatabaseAsync();
        }

        private static IServiceCollection AddDatabase(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(
                    configuration.GetConnectionString("AppDatabaseConnection"),
                    sql => sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
                ));

            return services;
        }

        private static IServiceCollection AddIdentityConfig(
            this IServiceCollection services)
        {
            services.AddIdentity<User, Role>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequiredUniqueChars = 1;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = "/Auth/Login";
                opt.LogoutPath = "/Auth/Logout";
                opt.AccessDeniedPath = "/Auth/AccessDenied";
                opt.ExpireTimeSpan = TimeSpan.FromHours(8);
                opt.SlidingExpiration = true;
                opt.Cookie.HttpOnly = true;
                opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                opt.Cookie.SameSite = SameSiteMode.Strict;
                opt.Cookie.Name = "Transportation.Auth";
            });

            services.AddScoped<IAuthService, AuthService>();
            return services;
        }

        #region Middlewares
        private static async Task SeedDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            var settings = scope.ServiceProvider.GetRequiredService<IOptions<AdminSeedSettings>>().Value;

            await AdminSeeder.SeedAsync(userManager, roleManager, settings);
        }
        #endregion
    }
}
