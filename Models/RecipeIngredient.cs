using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TastyVault.Models
{
  public class RecipeIngredient
  {
    [Key]
    public int Id { get; set; }
    [ForeignKey("RecipeId")]
    public Recipe Recipe { get; set; }
    [ForeignKey("IngredientId")]
    public Ingredient Ingredient { get; set; }

    public string Quantitative { get; set; } 
  }
}
