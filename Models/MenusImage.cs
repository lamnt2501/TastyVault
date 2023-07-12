using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;

namespace TastyVault.Models
{
  public class MenusImage
  {
    [Key]
    public int Id { get; set; }
    [Required]
    [Column(TypeName = "ntext")]
    public string ImagePath { get; set; }
    public int? MenusId { get; set; }

    [ForeignKey("MenusId")]
    public Menus? Menus { get; set; }
  }
}
