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
            // var MenuUser = await (from mu in _context.Menus where mu.UserId == _userManager.GetUserId(User) select mu).FirstOrDefaultAsync();
            // if(MenuUser != null){
            //     return View(MenuUser);
            // }
            // return NotFound();
        }
        public class MenusModel{
            public Menus Menus {set; get;}
            public IFormFile File {set; get;}
            public int[] RecipeId {set; get;}
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Recipes"] = _context.Recipes;
            ViewData["Categories"] = _context.Categories;
            ViewData["RecipeImages"] = _context.RecipeImages;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenusModel? menusModel)
        {
            if(ModelState.IsValid){

                //thêm ngày tạo, Update
                menusModel.Menus.CreatedDate = DateTime.Now;
                menusModel.Menus.ModifiedDate= DateTime.Now;

                //thêm userid
                menusModel.Menus.UserId = _userManager.GetUserId(User);

                // lấy id Menus vừa thêm
                var menusId = (_context.Menus.OrderBy(m => m.Id).LastOrDefault())?.Id;

                //thêm ảnh
                string datetimeprefix = DateTime.Now.ToString("yyyyMMddhhmmss");
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", datetimeprefix + menusModel.File.FileName);
                using var stream = new FileStream(path, FileMode.Create);
                menusModel.File.CopyTo(stream);
                menusModel.Menus.path = Path.Combine("uploads", datetimeprefix + menusModel.File.FileName);
                
                _context.Add(menusModel.Menus);
                await _context.SaveChangesAsync();

                //thêm các công thức của thực đơn
                foreach(var r in menusModel.RecipeId){
                    var menuRecipes = new MenuRecipes();
                    menuRecipes.RecipeId = r;
                    menuRecipes.MenuId = menusId;
                    _context.Add(menuRecipes);
                }
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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

        public async Task<IActionResult> Delete(int? menuId)
        {
            var DetailsMenu = await (from dm in _context.MenuRecipes where dm.MenuId == menuId select dm).ToListAsync();
            if(DetailsMenu != null)
                return View(DetailsMenu);
            return NotFound();
        }
    }
}