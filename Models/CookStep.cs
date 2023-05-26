using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace TastyVault.Models
{
  public class CookStep
  {
    [Key]
    public int Id { get; set; }
    [Required]
    public int StepOrder { get; set; }
    [Required]
    [Column(TypeName ="ntext")]
    public string Description { get; set; }
    public int? RecipeId { get; set; }
    [ForeignKey("RecipeId")]
    public Recipe Recipe { get; set; }
  }
}
