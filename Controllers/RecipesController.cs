using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TastyVault.Models;

namespace TastyVault.Controllers
{
  public class RecipesController : Controller
  {
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public RecipesController(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
    {
      _context = context;
      _userManager = userManager;
      _webHostEnvironment = webHostEnvironment;
    }

    // GET: Recipes
    public async Task<IActionResult> Index(int? cateId)
    {
      ViewData["Category"] = _context.Categories.Where(c=>c.Id == cateId).FirstOrDefault();
      if (_context.Recipes != null)
      {
        ViewData["RecipeImage"] = (from img in _context.RecipeImages from rc in _context.Recipes where rc.Id == img.RecipeId select img).ToList();
        var recipes = await (from r in _context.Recipes from rc in _context.RecipeCategories where rc.Category.Id == cateId select r).ToListAsync();
        return View(recipes);

      }
      return Problem("Entity set 'AppDbContext.Recipes'  is null.");
    }

    // GET: Recipes/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null || _context.Recipes == null)
      {
        return NotFound();
      }

      var recipe = await _context.Recipes
          .FirstOrDefaultAsync(m => m.Id == id);
      if (recipe == null)
      {
        return NotFound();
      }

      return View(recipe);
    }

    // GET: Recipes/Create
    [HttpGet]
    public IActionResult Create(int cateId)
    {
      ViewData["Categories"] = _context.Categories.ToList();
      ViewData["Ingredients"] = _context.Ingredients.ToList();
      ViewData["CateId"] = cateId;
      return View();
    }

    // POST: Recipes/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    public class RecipeModel
    {
      public Recipe Recipe { get; set; }
      public IFormFile[] files { get; set; }
      public int[] cateId { get; set; }
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RecipeModel? recipeModel)
    {
      if (ModelState.IsValid)
      {
        //thêm ngày tạo,update
        recipeModel.Recipe.CreatedDate = DateTime.Now;
        recipeModel.Recipe.ModifiedDate = DateTime.Now;

        //thêm userid
        recipeModel.Recipe.UserId = _userManager.GetUserId(User);
        _context.Add(recipeModel.Recipe);
        await _context.SaveChangesAsync();

        // lấy id công thức vừa thêm
        var recipeId = (_context.Recipes.OrderBy(r => r.Id).LastOrDefault())?.Id;

        //thêm img
        if (recipeModel.files != null)
        {
          foreach (var file in recipeModel.files)
          {
            string datetimeprefix = DateTime.Now.ToString("yyyyMMddhhmmss");
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", datetimeprefix + file.FileName);
            using var stream = new FileStream(path, FileMode.Create);
            file.CopyTo(stream);
            var recipeImage = new RecipeImage();
            recipeImage.RecipeId = recipeId;
            recipeImage.Path = Path.Combine("uploads", datetimeprefix + file.FileName);
            _context.Add(recipeImage);
          }
        }
        await _context.SaveChangesAsync();

        // thêm cookstep
        foreach (var cs in Request.Form.Where(keyValuePair => { return keyValuePair.Key.Contains("cs"); }))
        {
          var cookStep = new CookStep();
          cookStep.StepOrder = int.Parse(cs.Key.Substring(cs.Key.IndexOf("s") + 1));
          cookStep.Description = cs.Value;
          cookStep.RecipeId = recipeId;
          _context.Add(cookStep);
        }
        await _context.SaveChangesAsync();

        // thêm nguyên liệu
        foreach (var i in Request.Form.Where(keyValuePair => { return keyValuePair.Key.StartsWith("i-"); }))
        {
          var recipeIngredient = new RecipeIngredient();
          recipeIngredient.IngredientId = int.Parse(i.Value);
          recipeIngredient.RecipeId = recipeId;
          recipeIngredient.Quantitative = Request.Form.Where(kvp => { return kvp.Key == "qi-" + i.Key.Substring(i.Key.IndexOf('-') + 1); }).FirstOrDefault().Value;
          _context.Add(recipeIngredient);
        }
        await _context.SaveChangesAsync();

        // thêm category cho công thức
        foreach (var c in recipeModel.cateId)
        {
          var recipeCategory = new RecipeCategory();
          recipeCategory.RecipeId = recipeId;
          recipeCategory.CategoryId = c;
          _context.Add(recipeCategory);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      return View(recipeModel);
    }

    // GET: Recipes/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null || _context.Recipes == null)
      {
        return NotFound();
      }

      var recipe = await _context.Recipes.FindAsync(id);
      if (recipe == null)
      {
        return NotFound();
      }
      return View(recipe);
    }

    // POST: Recipes/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CreatedDate,ModifiedDate,Status")] Recipe recipe)
    {
      if (id != recipe.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(recipe);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!RecipeExists(recipe.Id))
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
      return View(recipe);
    }

    // GET: Recipes/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null || _context.Recipes == null)
      {
        return NotFound();
      }

      var recipe = await _context.Recipes
          .FirstOrDefaultAsync(m => m.Id == id);
      if (recipe == null)
      {
        return NotFound();
      }

      return View(recipe);
    }

    // POST: Recipes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      if (_context.Recipes == null)
      {
        return Problem("Entity set 'AppDbContext.Recipes'  is null.");
      }
      var recipe = await _context.Recipes.FindAsync(id);
      if (recipe != null)
      {
        _context.Recipes.Remove(recipe);
      }

      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool RecipeExists(int id)
    {
      return (_context.Recipes?.Any(e => e.Id == id)).GetValueOrDefault();
    }
  }
}
