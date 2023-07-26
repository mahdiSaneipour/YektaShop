using BN_Project.Core.IService.Account;
using BN_Project.Core.Service.Account;
using BN_Project.Data.Context;
using BN_Project.Data.Repository;
using BN_Project.Domain.IRepository;
using EP.Core.Tools.RenderViewToString;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();


#region Repositories

services.AddScoped<IAccountRepository, AccountRepository>();

#endregion

#region Authentication

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(option =>
{
    option.LoginPath = "Account/Login";
    option.LogoutPath = "Account/Logout";
    option.ExpireTimeSpan = TimeSpan.FromMinutes(43200);
});

#endregion

#region Services

services.AddScoped<IAccountServices, AccountServices>();
services.AddScoped<IViewRenderService, RenderViewToString>();

#endregion

#region Database

var connectionString = builder.Configuration.GetConnectionString("BNConnection");

services.AddDbContext<BNContext>(options =>
{
    options.UseSqlServer(connectionString);
});

#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();

app.MapControllerRoute(
    name: "area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();