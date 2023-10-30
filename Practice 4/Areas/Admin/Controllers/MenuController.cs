using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_4.DAL;
using Practice_4.Helpers;
using Practice_4.Models;
using Practice_4.ViewModels;

namespace Practice_4.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MenuController : Controller
    {
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _env;
        public MenuController(RoleManager<IdentityRole> rolemanager,
            AppDbContext db,
            IWebHostEnvironment env,
            UserManager<AppUser> usermanager,
            SignInManager<AppUser> signInManager)
        {
            _roleManager = rolemanager;
            _userManager = usermanager;
            _signInManager = signInManager;
            _env = env;
            _db = db;
        }
        public async Task<IActionResult> Manage(int id)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p=>p.Id == id);
            EditDishesVM editDishesVM = new EditDishesVM()
            {
                Product= await _db.Products.FirstOrDefaultAsync(x => x.Id == id),
                Restaurants = await _db.Restaurants.ToListAsync(),
                RestaurantID = product.RestaurantID


            };
            
            return View(editDishesVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(EditDishesVM EditDishesVM,int id)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p=>p.Id == id);
            EditDishesVM editDishesVM = new EditDishesVM()
            {
                Product = await _db.Products.FirstOrDefaultAsync(x => x.Id == id),
                Restaurants = await _db.Restaurants.ToListAsync(),
            };

            
            if (product == null)
            {
                return NotFound();
            }
            //if (!ModelState.IsValid)
            //{
            //    return View(editDishesVM);
            //}
            product.Name = EditDishesVM.Name;
            product.Description = EditDishesVM.Description;
            product.Price = EditDishesVM.Price;
            product.RestaurantID = EditDishesVM.RestaurantID;
            if (EditDishesVM.Photo != null)
            {
                if (!EditDishesVM.Restaurant.Photo.IsImage())
                {

                    TempData["Photo"] = "Select image file";
                    return RedirectToAction("Manage", "Menu", new { Id = id });
                }
                if (EditDishesVM.Restaurant.Photo.IsMore4mb())
                {
                    TempData["Photo"] = "Select image file";
                    return RedirectToAction("Manage", "Menu", new { Id = id });
                }
                string path = Path.Combine(_env.WebRootPath, @"assets\imgs\uploads\restaurant");
                product.Image = await EditDishesVM.Restaurant.Photo.SaveImageAsync(path);

            }
            await _db.SaveChangesAsync();
            
            return RedirectToAction("List", "Menu");
        }
        public async Task<IActionResult> Create()
        {
            EditDishesVM editDishesVM = new EditDishesVM()
            {
                Restaurants = await _db.Restaurants.ToListAsync()
            };
            return View(editDishesVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EditDishesVM editDishesVM)
        {
            if (editDishesVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "Select photo");
                return View(editDishesVM);
            }
            if (!editDishesVM.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Select image file");
                return View(editDishesVM);
            }
            if (editDishesVM.Photo.IsMore4mb())
            {
                ModelState.AddModelError("Photo", "Max size photo is 4 mb");
                return View(editDishesVM);
            }
            string path = Path.Combine(_env.WebRootPath, @"assets\imgs\uploads\restaurant");
            Product newproduct = new Product()
            {
                Name = editDishesVM.Name,
                Description = editDishesVM.Description,
                Price = editDishesVM.Price,
                RestaurantID=editDishesVM.RestaurantID,

            };
            newproduct.Image = await editDishesVM.Photo.SaveImageAsync(path);
            await _db.Products.AddAsync(newproduct);
            await _db.SaveChangesAsync();
            return RedirectToAction("List", "Menu");
        }
        public async Task<IActionResult> List()
        {
            var dishes = await _db.Products.ToListAsync();
            return View(dishes);
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _db.Products.FirstOrDefaultAsync(p=>p.Id == id);
           _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return RedirectToAction("List", "Menu");
        }

    }
}
