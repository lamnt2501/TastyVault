using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using TastyVault.Models;
using static TastyVault.Controllers.CategoriesController;

namespace TastyVault.Controllers
{
  public class PostsController : Controller
  {
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public PostsController(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
    {
      _context = context;
      _userManager = userManager;
      _webHostEnvironment = webHostEnvironment;
    }

    // GET: Posts
    public async Task<IActionResult> Index()
    {
      ViewData["Comments"] = _context.PostComments.ToList();
      ViewData["Reactions"] = _context.PostReactions.ToList();
      ViewData["PostImgs"] = _context.PostImages.ToList();
      ViewData["PostOwner"] = _context.Users.ToList();
      return View(await _context.Posts.ToListAsync());
    }

    // GET: Posts/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null || _context.Posts == null)
      {
        return StatusCode(404);
      }
      var post = await _context.Posts.FirstOrDefaultAsync(m => m.Id == id);
      if (post == null)
      {
        return StatusCode(404);
      }
      ViewData["PostOwner"] = _context.Users.Where(u => u.Id == post.UserId).FirstOrDefault();
      ViewData["PostImg"] = _context.PostImages.Where(pi => pi.PostId == id).FirstOrDefault();
      ViewData["Comments"] = _context.PostComments.Where(pc => pc.PostId == id).ToList();
      ViewData["CommentUsers"] = (from c in _context.PostComments
                                 join u in _context.Users
                                 on c.UserId equals u.Id
                                 select u).ToList();
      ViewData["Reactions"] = (from r in _context.PostReactions where r.PostId == id select r.Id).Count();
      ViewData["UserReactions"] = (from r in _context.PostReactions
                                  where r.PostId == id
                                  select r.UserId).ToList();
      return View(post);
    }

    // GET: Posts/Create
    [Authorize]
    public IActionResult Create()
    {
      ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
      return View();
    }

    // POST: Posts/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("UserId,Title,Content")] Post post, IFormFile file)
    {
      if (ModelState.IsValid)
      {
        post.UserId = _userManager.GetUserId(User);
        post.CreatedDate = DateTime.Now;
        post.ModifiedDate = DateTime.Now;
        _context.Add(post);
        await _context.SaveChangesAsync();

        var postId = _context.Posts.OrderBy(p => p.Id).LastOrDefault().Id;
        if (file != null && file.Length > 0)
        {
          string datetimeprefix = DateTime.Now.ToString("yyyyMMddhhmmss");
          var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", datetimeprefix + file.FileName);
          using var stream = new FileStream(path, FileMode.Create);
          file.CopyTo(stream);
          var postImg = new PostImage()
          {
            ImagePath = Path.Combine("uploads", datetimeprefix + file.FileName),
            PostId = postId
          };
          _context.Add(postImg);
          await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
      }
      ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", post.UserId);
      return View(post);
    }

    // GET: Posts/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null || _context.Posts == null)
      {
        return NotFound();
      }

      var post = await _context.Posts.FindAsync(id);
      if (post == null)
      {
        return NotFound();
      }
      ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", post.UserId);
      return View(post);
    }

    // POST: Posts/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("UserId,Title,Content,CreatedDate")] Post post,IFormFile? file)
    {
      Post p = await _context.Posts.FindAsync(id);
      p.ModifiedDate = DateTime.Now;
      p.Title = post.Title;
      p.Content = post.Content;
      if (file != null && file.Length > 0)
      {
        _context.PostImages.Remove(_context.PostImages.Where(pi => pi.PostId==id).FirstOrDefault());
        string datetimeprefix = DateTime.Now.ToString("yyyyMMddhhmmss");
        var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", datetimeprefix + file.FileName);
        using var stream = new FileStream(path, FileMode.Create);
        file.CopyTo(stream);
        var postImg = new PostImage()
        {
          ImagePath = Path.Combine("uploads", datetimeprefix + file.FileName),
          PostId = id
        };
        _context.Add(postImg);
        await _context.SaveChangesAsync();
      }
      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(p);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
          if (!PostExists(id))
          {
            return Content(id.ToString());
          }
          else
          {
            throw;
          }
        }
        //return RedirectToAction(nameof(Index));
        //return RedirectToPage("/Manage", new {id=0});
        return Redirect("http://localhost:5271/Identity/Account/Manage?id=0");
      }
      ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", post.UserId);
      return View(post);
    }

    // GET: Posts/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null || _context.Posts == null)
      {
        return NotFound();
      }

      var post = await _context.Posts
          .Include(p => p.AppUser)
          .FirstOrDefaultAsync(m => m.Id == id);
      if (post == null)
      {
        return NotFound();
      }

      return View(post);
    }

    // POST: Posts/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      if (_context.Posts == null)
      {
        return Problem("Entity set 'AppDbContext.Posts'  is null.");
      }
      var post = await _context.Posts.FindAsync(id);
      if (post != null)
      {
        _context.PostComments.RemoveRange(_context.PostComments.Where(c => c.PostId == id));
        _context.PostImages.Remove(_context.PostImages.Where(c => c.PostId == id).FirstOrDefault());
        _context.Posts.Remove(post);
      }

      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool PostExists(int id)
    {
      return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
    }
  }
}
