using FreelanceProject.Data.Entities;
using FreelanceProject.Data;
using Microsoft.AspNetCore.Identity;
using FreelanceProject.Services.Abstract;
using FreelanceProject.Services.Concrete;
using FreelanceProject.Localizations;

namespace FreelanceProject.Extensions
{
    public static class StartupExtensions
    {
        public static void AddIdentityWithExtension(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.AllowedUserNameCharacters = "abcdefghiüöjklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                options.User.RequireUniqueEmail = true;

                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
            }).AddErrorDescriber<LocalizationIdentityErrorDescriber>()
                .AddEntityFrameworkStores<FreelanceDbContext>();
        }

        public static void AddServicesWithExtension(this IServiceCollection services)
        {
            services.AddScoped<UserManager<AppUser>>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<SignInManager<AppUser>>();
        }
    }
}
