using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TastyVault.Models
{
  public class PostComment
  {

    [Key]
    public int Id { get; set; }
    public string UserId { get; set; }
    public int PostId { get; set; }
    [ForeignKey("UserId")]
    public AppUser User { get; set; }
    [ForeignKey("PostId")]
    public Post? Post { get; set; }
    [Required]
    [Column(TypeName ="ntext")]
    public string Content { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set;}
    [Range(0,1)]
    public int Status { set; get; } = 1;
  }
}
