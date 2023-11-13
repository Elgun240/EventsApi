using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Practice_4.DAL;
using Practice_4.Models;
using Practice_4.ViewModels;

namespace Practice_4.Areas.Admin.Controllers
{
    [Area("aAdmin")]
    [Authorize(Roles ="Admin")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _db;
        public DashboardController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            DashBoardVM dashBoardVM = new DashBoardVM()
            {
                AppUsers =  _db.Users.Count(),
                Products = _db.Products.Count(),
                Restaurants = _db.Restaurants.Count(),
                RejectedOrders = _db.PaidOrders.Where(x => x.Status == Status.Cancelled).Count(),
                DeliveredOrders = _db.PaidOrders.Where(x=>x.Status==Status.Delivered).Count(),
                Categories = _db.Categories.Count(),
                PaidOrders = _db.PaidOrders.Count(),
                PendingOrders = _db.PaidOrders.Where(x => x.Status == Status.Dispatch).Count()

            };
            return View(dashBoardVM);
        }
    }
}
