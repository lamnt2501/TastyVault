using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TastyVault.Models{
    public class MenuRecipes{
        [Key]
        public int id {set; get;}
        public int? MenuId {set; get;}

        [ForeignKey("MenuId")]
        public Menus? Menus {set; get;}
        
        public int? RecipeId {set; get;}

        [ForeignKey("RecipeId")]
        public Recipe Recipe {set; get;}
    }
}