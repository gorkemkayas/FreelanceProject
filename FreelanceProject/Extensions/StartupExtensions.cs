using FreelanceProject.Data.Entities;
using Microsoft.AspNetCore.Identity;
using FreelanceProject.Services.Abstract;
using FreelanceProject.Services.Concrete;
using FreelanceProject.Localizations;
using FreelanceProject.Data.Context;
using FreelanceProject.CustomMethods.Abstract;
using FreelanceProject.CustomMethods.Concrete;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using FreelanceProject.Data.MappingProfile;
using Microsoft.AspNetCore.Authentication.Cookies;

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
              .AddEntityFrameworkStores<FreelanceDbContext>()
              .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(1);
            });

            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromMinutes(5);
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User/Signin";
            });
        }

        public static void AddServicesWithExtension(this IServiceCollection services)
        {
            services.AddScoped<UserManager<AppUser>>();
            services.AddScoped<SignInManager<AppUser>>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUrlGenerator, UrlGenerator>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();

            services.AddAutoMapper(typeof(MappingProfile));
        }
    }
}
