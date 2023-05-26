﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TastyVault.Models;

namespace TastyVault.Controllers
{
  public class RecipesController : Controller
  {
    private readonly AppDbContext _context;

    public RecipesController(AppDbContext context)
    {
      _context = context;
    }

    // GET: Recipes
    public async Task<IActionResult> Index(int? cateId)
    {
      ViewData["CategoryId"] = cateId;
      return _context.Recipes != null ?
                  View(await (from r in _context.Recipes from rc in _context.RecipeCategories where rc.Category.Id == cateId select r).ToListAsync()) :
                  Problem("Entity set 'AppDbContext.Recipes'  is null.");
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
      string v = "";
      
      var errors = ModelState.Values.SelectMany(v => v.Errors);
      foreach (var e in errors)
      {
        v += e.ErrorMessage + "\n";
      }
      //return Content(ModelState.IsValid.ToString() + "\n" + v);
      return Content(User.Identity.Name);
      if (ModelState.IsValid)
      {
        v += _context.Recipes.Count() + " ";
        _context.Add(recipeModel.Recipe);
        v += _context.Recipes.Count() + " ";
        await _context.SaveChangesAsync();
        v += _context.Recipes.Count() + " ";
        return Content(v);
        //return RedirectToAction(nameof(Index));
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
