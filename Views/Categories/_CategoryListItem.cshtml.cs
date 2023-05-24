using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TastyVault.Models;

namespace TastyVault.Views.Categories
{
    public class _CategoryListItemModel : PageModel
    {
        public Category Category { get; set; }
        public void OnGet()
        {
        }
    }
}
