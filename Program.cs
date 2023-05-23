using Microsoft.EntityFrameworkCore;
using TastyVault.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
{
  string? connectionString = builder.Configuration.GetConnectionString("AppConnectionString");
  if (connectionString == null)
  {
    connectionString = "Data Source=LAPTOP-KJH263H7;Initial Catalog=tastyvaultdb;User Id=sa;Password=123;Encrypt=False";
  }
  options.UseSqlServer(connectionString);
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
