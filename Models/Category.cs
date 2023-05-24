using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TastyVault.Models
{
  public class Category
  {
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    public int ParentCatecoryId { get; set; }

    public string? ImagePath { get; set; }
    public string? Description { get; set; }
  }
}
