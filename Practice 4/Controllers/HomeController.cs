using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_4.DAL;
using Practice_4.ViewModels;
using Practice_4.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;

namespace Practice_4.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _db= db;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Dashboard", new {area="aAdmin"});
            }
            if (User.IsInRole("Restaurant"))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "aRestaurant" });
            }
            HomeVM homeVM = new HomeVM()
            {
                
                Sliders= await _db.Sliders.ToListAsync(),
                Products= await _db.Products.OrderByDescending(p=>p.SaledCount).Take(5).ToListAsync(),
            };

            ViewData["Id"] = _userManager.GetUserId(User);
            if (TempData.ContainsKey("Success"))
            {
                ModelState.AddModelError("Success", "Comment has been sended");
            }
            return View(homeVM);
        }

       
    }
}