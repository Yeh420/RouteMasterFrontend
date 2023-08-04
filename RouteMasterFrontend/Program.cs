using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RouteMasterFrontend.EFModels;
using RouteMasterFrontend.Models.Infra.DapperRepositories;
using RouteMasterFrontend.Models.Interfaces;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container.
builder.Services.AddDbContext<RouteMasterContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("RouteMaster"));
});



builder.Services.Configure<IdentityOptions>(options =>
{
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(20);
	options.Lockout.MaxFailedAccessAttempts = 2;

});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
	//未登入時導入的網址
	options.LoginPath = new PathString("/Members/MemberLogin");

	//options.AccessDeniedPath = "/"; 存取失敗的路徑
});
builder.Services.Configure<CookiePolicyOptions>(options =>
{
	options.MinimumSameSitePolicy = SameSiteMode.Lax;
	options.HttpOnly = HttpOnlyPolicy.Always;
	options.Secure = (CookieSecurePolicy)SameSiteMode.None; // 設置 Secure 屬性為 None
});




builder.Services.AddControllersWithViews();

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

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
