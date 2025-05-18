using FreelanceProject.Data.Context;
using FreelanceProject.Data.Entities;
using FreelanceProject.Extensions;
using FreelanceProject.Services.Abstract;
using FreelanceProject.Services.Concrete;
using FreelanceProject.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<FreelanceDbContext>(options => { options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon")); });
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddIdentityWithExtension();
builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.Cookie.Name = "BlogCookie";

    opt.ExpireTimeSpan = TimeSpan.FromDays(30);
    opt.SlidingExpiration = true;

    opt.LoginPath = new PathString("/User/SignIn");
    opt.LogoutPath = new PathString("/User/Logout");
    opt.AccessDeniedPath = new PathString("/User/AccessDenied");
});
builder.Services.AddServicesWithExtension();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
        policy.RequireRole("Admin"));
});


var app = builder.Build();

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

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
