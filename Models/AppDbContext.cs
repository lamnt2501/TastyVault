using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TastyVault.Models
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
    }

    public DbSet<Recipe> Recipes { get; set; } 
    public DbSet<Category> Categories { get; set; }
    public DbSet<CookStep> CookSteps { get; set; }
    public DbSet<Ingredient> Ingredients { get; set;}
    public DbSet<RecipeCategory> RecipeCategories { get; set; }  
    public DbSet<RecipeImage> RecipeImages { get; set; } 
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
  }
}
