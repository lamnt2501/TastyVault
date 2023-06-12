using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Diagnostics;
using TastyVault.Models;

namespace TastyVault.Controllers
{
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _appDbContext;

    public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext)
    {
      _logger = logger;
      _appDbContext = appDbContext;
    }
    public class NewRecipes
    {
      public Recipe Recipe { set; get; }
      public string Path { set; get; }
    }
    public class PopularPost
    {
      public Post Post { set; get; }
      public int Reactioncount { set; get; }
      public string ImagePath { set; get; }
    }
    public class NewPost
    {
      public Post Post { set; get; }
      public string ImagePath { set; get; }
    }
    public IActionResult Index()
    {
      // lấy bài viết mới
      var newrecipes = new List<NewRecipes>();
      foreach (var i in _appDbContext.Recipes.OrderByDescending(r => r.CreatedDate).Take(5))
      {
        newrecipes.Add(new NewRecipes()
        {
          Recipe = i
        });
      }

      foreach (var i in newrecipes)
      {
        i.Path = _appDbContext.RecipeImages.FirstOrDefault(ri => i.Recipe.Id == ri.RecipeId).Path;
      }
      ViewData["NewRecipes"] = newrecipes;
      // lấy bài viết nổi bật   
      ViewData["NewPosts"] = (from p in _appDbContext.Posts.OrderByDescending(p => p.CreatedDate).Take(3)
                             join pi in _appDbContext.PostImages on p.Id equals pi.PostId
                             select new NewPost
                             {
                               Post = p,
                               ImagePath = pi.ImagePath

                             }).ToList();
      var popularPost = (from i in  (from pr in _appDbContext.PostReactions
                                  group pr by pr.PostId into g
                                  select new
                                  {
                                    postId = g.Key,
                                    NumOfReacts = g.Count()
                                  } 
                                  ) join p in _appDbContext.Posts on i.postId equals p.Id
                                 select new PopularPost
                                 {
                                   Post = p,
                                   Reactioncount = i.NumOfReacts
                                 }).OrderByDescending(np=>np.Reactioncount).ToList();
      foreach (var i in popularPost)
      {
        i.ImagePath = _appDbContext.PostImages.FirstOrDefault(pi => pi.PostId == i.Post.Id)?.ImagePath;
      }
      ViewData["PopularPosts"] = popularPost.ToList();
      return View();
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}