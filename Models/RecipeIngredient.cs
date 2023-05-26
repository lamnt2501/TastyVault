using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TastyVault.Models
{
  public class RecipeIngredient
  {
    [Key]
    public int Id { get; set; }
    public int? RecipeId { get; set; }
    public int? IngredientId { set; get; }
    [ForeignKey("RecipeId")]
    public Recipe Recipe { get; set; }
    [ForeignKey("IngredientId")]
    public Ingredient Ingredient { get; set; }

    public string Quantitative { get; set; } 
  }
}
