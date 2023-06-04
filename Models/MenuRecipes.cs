using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TastyVault.Models{
    public class MenuRecipes{
        [Key]
        public int id {set; get;}


        public int? MenuId {set; get;}
        [ForeignKey("UserId")]
        public AppUser? User {set; get;}
        public string? UserId {set; get;}
        [ForeignKey("MenuId")]
        public Menus? Menus {set; get;}

    }
}
    }
}