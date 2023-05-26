using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TastyVault.Models
{
  public class RecipeImage
  {
    [Key]
    public int Id { get; set; }
    public int? RecipeId { get; set; } 
    [ForeignKey("RecipeId")]
    public Recipe Recipe { get; set; }
    [Required]
    public string Path { get; set; }
  }
}
