using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TastyVault.Models
{
  public class Recipe
  {
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    [Required]
    [Column(TypeName ="ntext")]
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set;}
    [Range(0,1)]
    public int Status { set; get; } = 1;

    [ForeignKey("UserId")]
    public AppUser User { get; set; }
  }
}
