using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TastyVault.Models{
    public class Menus{
        [Key]
        public int Id {set; get; }

        [Required]
        [StringLength(100)]
        public string MenuName{set; get; }

        [Required]
        public string path {set; get;}
        public DateTime CreatedDate {set; get;}
        public DateTime ModifiedDate {set; get;}

        [Range(0,1)]
        public int Status {set; get;} = 1;

        public string? UserId {set; get; }
        [ForeignKey("UserId")]
        public AppUser? User {set; get;}
    }
}
        public string? UserId {set; get; }
        [ForeignKey("UserId")]
        public AppUser? User {set; get;}
    }
}