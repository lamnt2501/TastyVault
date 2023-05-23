using System.ComponentModel.DataAnnotations;

namespace TastyVault.Models
{
  public class Ingredient
  {
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    public string? Description { get; set; }
  }
}
