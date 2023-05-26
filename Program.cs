using Microsoft.AspNetCore.Identity;
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
builder.Services.AddRazorPages();
builder.Services.AddIdentity<AppUser,IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
  options.Password.RequireDigit = false; 
  options.Password.RequireLowercase = false; 
  options.Password.RequireNonAlphanumeric = false;
  options.Password.RequireUppercase = false; 
  options.Password.RequiredLength = 3; 
  options.Password.RequiredUniqueChars = 1;

  options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); 
  options.Lockout.MaxFailedAccessAttempts = 5; 
  options.Lockout.AllowedForNewUsers = true;

  options.User.AllowedUserNameCharacters = 
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
  options.User.RequireUniqueEmail = true;

  options.SignIn.RequireConfirmedEmail = true;   
  options.SignIn.RequireConfirmedPhoneNumber = false;
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
//app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
