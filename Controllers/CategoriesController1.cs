using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TastyVault.Models;

namespace TastyVault.Controllers
{
  public class CategoriesController : Controller
  {
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public CategoriesController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
      _webHostEnvironment = webHostEnvironment;
      _context = context;
    }   
    //public CategoriesController(AppDbContext context)
    //{
    //  _context = context;
    //}

    // GET: Categories
    public async Task<IActionResult> Index()
    {
      return _context.Categories != null ?
                  View(await _context.Categories.ToListAsync()) :
                  Problem("Entity set 'AppDbContext.Categories'  is null.");
    }

    // GET: Categories/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null || _context.Categories == null)
      {
        return NotFound();
      }

      var category = await _context.Categories
          .FirstOrDefaultAsync(m => m.Id == id);
      if (category == null)
      {
        return NotFound();
      }

      return View(category);
    }

    // GET: Categories/Create
    public async Task<IActionResult> Create()
    {
      return View();
    }

    // POST: Categories/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    public class CategoryModel
    {
      //[FileExtensions(Extensions = "jpg,png,gif,jpeg")]
      [DataType(DataType.Upload)]
      public IFormFile? FileUpload { set; get; }

      public Category Category { get; set; }

    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryModel categoryModel)
    {
      if (ModelState.IsValid)
      {
        if(categoryModel.Category == null || categoryModel.FileUpload == null)
        {
            return BadRequest(categoryModel);
        }
        if (categoryModel.Category.ParentCatecoryId != 0)
        {
          if (_context.Categories.Where(c => c.Id == categoryModel.Category.ParentCatecoryId).FirstOrDefault() == null)
          {
            return View(categoryModel);
          }
        }
        if (categoryModel.FileUpload != null && categoryModel.FileUpload.Length > 0)
        {
          string datetimeprefix = DateTime.Now.ToString("yyyyMMddhhmmss");
          var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads",datetimeprefix + categoryModel.FileUpload.FileName);
          using var stream = new FileStream(path, FileMode.Create);
          categoryModel.FileUpload.CopyTo(stream);
          categoryModel.Category.ImagePath = Path.Combine("uploads", datetimeprefix + categoryModel.FileUpload.FileName);
        }
        _context.Add(categoryModel.Category);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      return View(categoryModel);
    }

    // GET: Categories/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null || _context.Categories == null)
      {
        return NotFound();
      }

      var category = await _context.Categories.FindAsync(id);
      if (category == null)
      {
        return NotFound();
      }
      return View(category);
    }

    // POST: Categories/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ParentCatecoryId,ImagePath,Description")] Category category)
    {
      if (id != category.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(category);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!CategoryExists(category.Id))
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
      return View(category);
    }

    // GET: Categories/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null || _context.Categories == null)
      {
        return NotFound();
      }

      var category = await _context.Categories
          .FirstOrDefaultAsync(m => m.Id == id);
      if (category == null)
      {
        return NotFound();
      }

      return View(category);
    }

    // POST: Categories/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      if (_context.Categories == null)
      {
        return Problem("Entity set 'AppDbContext.Categories'  is null.");
      }
      var category = await _context.Categories.FindAsync(id);
      if (category != null)
      {
        _context.Categories.Remove(category);
      }

      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool CategoryExists(int id)
    {
      return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
    }
  }
}