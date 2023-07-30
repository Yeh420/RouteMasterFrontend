using Microsoft.EntityFrameworkCore;
using RouteMasterBackend.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container.
string RouteMasterConnectString = builder.Configuration.GetConnectionString("RouteMaster");
builder.Services.AddDbContext<RouteMasterContext>(options =>
{
	options.UseSqlServer(RouteMasterConnectString);
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle




//設定通過CORS適用規則
string MyAllowOrigns = "AllowAny";
builder.Services.AddCors(
	options =>
	{
		options.AddPolicy(
		name: MyAllowOrigns, policy => policy
		.WithOrigins("*")
		.WithHeaders("*")
		.WithMethods("*")
			);
	}
);



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}





//啟用Cors，策略由後續的控制器自行分別指定
app.UseCors();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
