using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TastyVault.Models;
using Microsoft.AspNetCore.Identity;

namespace TastyVault.Controllers{
    public class MenusController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public MenusController(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        public async Task<IActionResult> Details(int? menuId)
        {
            ViewData["Menus"] = _context.Menus.Where(c=>c.Id == menuId).FirstOrDefault();
            if (_context.Recipes != null)
            {
                ViewData["RecipeImage"] = (from img in _context.RecipeImages from rc in _context.Recipes where rc.Id == img.RecipeId select img).ToList();
                var recipes = await (from r in _context.Recipes from mr in _context.MenuRecipes where mr.Menus.Id == menuId select r).ToListAsync();
                return View(recipes);
            }
            return Problem("Entity set 'AppDbContext.Menus'  is null.");
        }

        public async Task<IActionResult> Delete()
        {
            return View();
        }
    }
}