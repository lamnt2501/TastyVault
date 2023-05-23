using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TastyVault.Models
{
  public class RecipeCategory
  {
    [Key]
    public int Id { get; set; }
    [ForeignKey("CategoryId")]
    public Category Category { get; set; }
    [ForeignKey("RecipeId")]
    public Recipe Recipe { get; set; }
  }
}
