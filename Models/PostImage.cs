using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;

namespace TastyVault.Models
{
  public class PostImage
  {
    [Key]
    public int Id { get; set; }
    [Required]
    [Column(TypeName = "ntext")]
    public string ImagePath { get; set; }
    public int PostId { get; set; }
    [ForeignKey("PostID")]
    public Post? Post { get; set; }
  }
}
