using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TastyVault.Models;

namespace TastyVault.Controllers
{
  public class PostReactions : Controller
 { 
    private readonly AppDbContext _appDbContext;
    public PostReactions(AppDbContext appDbContext)
    {
      _appDbContext = appDbContext;
    }
  
    // GET: PostReactions
    public ActionResult Index()
    {
      return View();
    }

    // GET: PostReactions/Details/5
    public ActionResult Details(int id)
    {
      return View();
    }

    // GET: PostReactions/Create
    public ActionResult Create()
    {
      return View();
    }

    // POST: PostReactions/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<ActionResult> Create(int id, string userid)
    {
      var reaction = new PostReaction()
      {
        PostId = id,
        UserId = userid,
      };
      var reactions = _appDbContext.PostReactions.Where(r => r.UserId == userid && r.PostId == id).ToList();
      if (reactions.Count() == 0)
        _appDbContext.Add(reaction);
      else
      {
        var r = _appDbContext.Find<PostReaction>(reactions.FirstOrDefault().Id);
        _appDbContext.Remove(r);

      }
      await _appDbContext.SaveChangesAsync();
      return RedirectToAction("Details", "Posts", new { id = id });
    }

    // GET: PostReactions/Edit/5
    public ActionResult Edit(int id)
    {
      return View();
    }

    // POST: PostReactions/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
      try
      {
        return RedirectToAction(nameof(Index));
      }
      catch
      {
        return View();
      }
    }

    // GET: PostReactions/Delete/5
    public ActionResult Delete(int id)
    {
      return View();
    }

    // POST: PostReactions/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
      try
      {
        return RedirectToAction(nameof(Index));
      }
      catch
      {
        return View();
      }
    }
  }
}
