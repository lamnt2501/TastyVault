using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TastyVault.Models
{
  public class PostReaction
  {
    [Key]
    public int Id { get; set; }
    public int PostId { set; get; }
    public string UserId { get; set; }
    [ForeignKey("UserId")]
    public AppUser? User { get; set; }
    [ForeignKey("PostId")]
    public Post? Post { get; set; }
  }
}
