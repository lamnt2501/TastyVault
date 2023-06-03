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
            var MenuUser = await (from mu in _context.Menus where mu.UserId == _userManager.GetUserId(User) select mu).FirstOrDefaultAsync();
            if(MenuUser != null){
                return View(MenuUser);
            }
            return NotFound();
        }
        public async Task<IActionResult> Create()
        {
            return _context.Menus != null ?
                    View(await _context.Menus.ToListAsync()) :
                    Problem("Entity set 'AppDbContext.Menus'  is null.");
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

        public async Task<IActionResult> Delete(int? menuId)
        {
            var DetailsMenu = await (from dm in _context.MenuRecipes where dm.MenuId == menuId select dm).ToListAsync();
            if(DetailsMenu != null)
                return View(DetailsMenu);
            return NotFound();
        }
    }
}