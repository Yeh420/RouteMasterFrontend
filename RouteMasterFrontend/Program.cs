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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
