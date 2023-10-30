using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_4.DAL;
using Practice_4.ViewModels;
using Practice_4.Models;
using System.Diagnostics;

namespace Practice_4.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db)
        {
            _db= db;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Dashboard", new {area="Admin"});
            }
            HomeVM homeVM = new HomeVM()
            {
                
                Sliders= await _db.Sliders.ToListAsync(),
                Products= await _db.Products.OrderByDescending(p=>p.SaledCount).Take(5).ToListAsync(),
            };
            return View(homeVM);
        }

       
    }
}