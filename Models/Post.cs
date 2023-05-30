using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TastyVault.Models
{
  public class Post
  {
    [Key]
    public int Id { get; set; }
    [Required]
    [Column(TypeName = "ntext")]
    public string Title { get; set; }
    [Required]
    [Column(TypeName ="ntext")]
    public string Content { get; set; } 
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    [Range(0, 1)]
    public int Status { get; set; } = 1;
    public string UserId { get; set; }
    [ForeignKey("UserId")]
    public AppUser? AppUser { get; set; }
  }
}
