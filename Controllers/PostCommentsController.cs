using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TastyVault.Models;

namespace TastyVault.Controllers
{
  public class PostCommentsController : Controller
  {
    private readonly AppDbContext _context;
    private UserManager<AppUser> _userManage;
    public PostCommentsController(AppDbContext context, UserManager<AppUser> userManager)
    {
      _context = context;
      _userManage = userManager;
    }

    // GET: PostComments
    public async Task<IActionResult> Index()
    {
      var appDbContext = _context.PostComments.Include(p => p.Post).Include(p => p.User);
      return View(await appDbContext.ToListAsync());
    }

    // GET: PostComments/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null || _context.PostComments == null)
      {
        return NotFound();
      }

      var postComment = await _context.PostComments
          .Include(p => p.Post)
          .Include(p => p.User)
          .FirstOrDefaultAsync(m => m.Id == id);
      if (postComment == null)
      {
        return NotFound();
      }

      return View(postComment);
    }

    // GET: PostComments/Create
    public IActionResult Create()
    {
      ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Content");
      ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
      return View();
    }

    // POST: PostComments/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string UserId,string PostId,string content)
    {

      if (ModelState.IsValid)
      {
        var postComment = new PostComment()
        {
          UserId = UserId,
          PostId = int.Parse(PostId),
          Content = content,
          CreatedDate = DateTime.Now,
          ModifiedDate = DateTime.Now,
        };
        _context.Add(postComment);
        await _context.SaveChangesAsync();
      }
      return RedirectToAction("Details", "Posts", new {id = PostId},"comment");
    }

    // GET: PostComments/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null || _context.PostComments == null)
      {
        return NotFound();
      }

      var postComment = await _context.PostComments.FindAsync(id);
      if (postComment == null)
      {
        return NotFound();
      }
      ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Content", postComment.PostId);
      ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", postComment.UserId);
      return View(postComment);
    }

    // POST: PostComments/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,PostId,Content,CreatedDate,ModifiedDate,Status")] PostComment postComment)
    {
      if (id != postComment.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(postComment);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!PostCommentExists(postComment.Id))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(Index));
      }
      ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Content", postComment.PostId);
      ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", postComment.UserId);
      return View(postComment);
    }

    // GET: PostComments/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null || _context.PostComments == null)
      {
        return NotFound();
      }

      var postComment = await _context.PostComments
          .Include(p => p.Post)
          .Include(p => p.User)
          .FirstOrDefaultAsync(m => m.Id == id);
      if (postComment == null)
      {
        return NotFound();
      }

      return View(postComment);
    }

    // POST: PostComments/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      if (_context.PostComments == null)
      {
        return Problem("Entity set 'AppDbContext.PostComments'  is null.");
      }
      var postComment = await _context.PostComments.FindAsync(id);
      if (postComment != null)
      {
        _context.PostComments.Remove(postComment);
      }

      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool PostCommentExists(int id)
    {
      return (_context.PostComments?.Any(e => e.Id == id)).GetValueOrDefault();
    }
  }
}
