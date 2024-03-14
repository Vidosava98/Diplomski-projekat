using fixit.Data;
using Microsoft.EntityFrameworkCore;
using fixit.Models;
using StatApp.Data;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("fixItConnection");
builder.Services.AddDbContext<DataContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddCors(options =>
{
    options.AddPolicy("allowedOrigin", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRepository<Transakcija>, ServiceRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSignalR();
var app = builder.Build();

app.UseCors("allowedOrigin");
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<TransakcijaHub>("/transakcijaHub");
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Transakcija}/{action=Index}");
});
app.Run();
