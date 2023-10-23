using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_4.DAL;
using Practice_4.Models;
using Practice_4.ViewModels;

namespace Practice_4.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly AppDbContext _db;
        public RestaurantsController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Restaurant> restaurants =_db.Restaurants.ToList();

            return View(restaurants);
        }
        public async Task<IActionResult> Dish(int Id)
        {
            DishVM dishVM = new DishVM() 
            {
                Restaurant= await _db.Restaurants.FirstOrDefaultAsync(r=>r.Id==Id),
                Products  = await _db.Products.Where(p=>p.RestaurantID==Id).ToListAsync(),
                
            };

            return View(dishVM);
        }

    }
}
