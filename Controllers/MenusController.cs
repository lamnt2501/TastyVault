using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TastyVault.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

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
            // return View();
            if(_context.Menus != null){
                ViewData["MenusImage"] = (from mi in _context.MenusImages from m in _context.Menus where m.Id == mi.MenusId select mi).ToList();
                ViewData["Menus"] = await (from mu in _context.Menus where mu.UserId == _userManager.GetUserId(User) select mu).ToListAsync();
                return View();
            }
            return NotFound();
        }

        public class MenusModel{
            public Menus Menus {set; get;}
            public IFormFile File {set; get;}
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Recipes"] = _context.Recipes.ToList();
            ViewData["Categories"] = _context.Categories.ToList();
            ViewData["RecipeImages"] = _context.RecipeImages.ToList();
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
                _context.Add(menusModel.Menus);
                await _context.SaveChangesAsync();

                // lấy id Menus vừa thêm
                var menusId = (_context.Menus.OrderBy(m => m.Id).LastOrDefault())?.Id;

                //thêm ảnh
                string datetimeprefix = DateTime.Now.ToString("yyyyMMddhhmmss");
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", datetimeprefix + menusModel.File.FileName);
                using var stream = new FileStream(path, FileMode.Create);
                menusModel.File.CopyTo(stream);

                var menusImage = new MenusImage();
                menusImage.MenusId = menusId;
                menusImage.ImagePath = Path.Combine("uploads", datetimeprefix + menusModel.File.FileName);
                
                _context.Add(menusImage);
                await _context.SaveChangesAsync();

                //thêm các công thức của thực đơn
                foreach (var r in Request.Form.Where(keyValuePair => { return keyValuePair.Key.StartsWith("r-"); }))
                {
                    var menuRecipes = new MenuRecipes();
                    menuRecipes.RecipeId = int.Parse(r.Value);
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
            if (_context.Recipes != null)
            {
                ViewData["Menus"] = _context.Menus.Where(m => m.Id == menuId).FirstOrDefault();
                // ViewData["MenusRecipe"] = _context.MenuRecipes.Where(mr => mr.MenuId == menuId).ToList();
                ViewData["RecipeImage"] = (from img in _context.RecipeImages from rc in _context.Recipes where rc.Id == img.RecipeId select img).ToList();
                ViewData["Recipe"] = (from r in _context.Recipes from mr in _context.MenuRecipes where mr.RecipeId == r.Id select r).ToList();
                var menusRecipe = _context.MenuRecipes.Where(mr => mr.MenuId == menuId).ToList();
                return View(menusRecipe);
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Menus == null)
            {
                return Problem("Entity set 'AppDbContext.Recipes'  is null.");
            }
            var menus = await _context.Menus.FindAsync(id);
            if (menus != null)
            {
                _context.Menus.Remove(menus);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Save()
        {
            if(_context.Menus != null){
                ViewData["MenusImage"] = (from mi in _context.MenusImages from m in _context.Menus where m.Id == mi.MenusId select mi).ToList();
                return View(_context.Menus);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(int? menuId)
        {
            if(ModelState.IsValid){
                var menusUser = new MenusUser();
                //thêm user id
                menusUser.UserId = _userManager.GetUserId(User);

                //thêm menusId
                menusUser.MenusId = menuId;

                //thêm menusUser vào db
                _context.Add(menusUser);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Save));
            }
            return View();
        } 
        public async Task<IActionResult> UnSave(int? id)
        {
            if (id == null || _context.MenusUser == null)
            {
                return NotFound();
            }

            var menusUser = await _context.MenusUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menusUser == null)
            {
                return NotFound();
            }
            return View(menusUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnSave(int id)
        {
            if (_context.MenusUser == null)
            {
                return Problem("Entity set 'AppDbContext.Recipes'  is null.");
            }
            var menusUser = await _context.MenusUser.FindAsync(id);
            if (menusUser != null)
            {
                _context.MenusUser.Remove(menusUser);
            }

            await _context.SaveChangesAsync();
            
            return Redirect(HttpContext.Request.Headers["Referer"].ToString());
        }
    }
}