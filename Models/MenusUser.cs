using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;

namespace TastyVault.Models
{
  public class MenusUser
  {
    [Key]
    public int Id { get; set; }
    public string? UserId {set; get;}
    [ForeignKey("UserId")]
    public AppUser User {set; get;}
    public int? MenusId { get; set; }

    [ForeignKey("MenusId")]
    public Menus? Menus { get; set; }
  }
}
