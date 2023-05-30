using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using TastyVault.Models;
using static TastyVault.Controllers.CategoriesController;

namespace TastyVault.Controllers
{
  public class PostsController : Controller
  {
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public PostsController(AppDbContext context,UserManager<AppUser> userManager,IWebHostEnvironment webHostEnvironment)
    {
      _context = context;
      _userManager = userManager;
      _webHostEnvironment = webHostEnvironment;
    }

    // GET: Posts
    public async Task<IActionResult> Index()
    {
      ViewData["PostImgs"] = _context.PostImages.ToList();
      return View(await _context.Posts.ToListAsync());
    }

    // GET: Posts/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null || _context.Posts == null)
      {
        return Content("vailon luon");
      }
      var post = await _context.Posts.FirstOrDefaultAsync(m => m.Id == id);
      if (post == null)
      {
        return Content("khong co post nao o day ca");
      }

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
    public async Task<IActionResult> Create([Bind("UserId,Title,Content")] Post post,IFormFile file)
    {
      if (ModelState.IsValid)
      {
        post.UserId = _userManager.GetUserId(User);
        post.CreatedDate = DateTime.Now;
        post.ModifiedDate = DateTime.Now;
        _context.Add(post);
        await _context.SaveChangesAsync();

        var postId = _context.Posts.OrderBy(p=>p.Id).LastOrDefault().Id;
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
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,CreatedDate,ModifiedDate,Status,UserId")] Post post)
    {
      if (id != post.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(post);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!PostExists(post.Id))
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
