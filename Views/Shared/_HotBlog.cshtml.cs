using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TastyVault.Views.Shared
{
  public class _HotBlogModel : PageModel
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    public void OnGet()
    {

    }
  }
}
