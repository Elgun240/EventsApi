using Microsoft.AspNetCore.Identity;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public RestaurantsController(RoleManager<IdentityRole> rolemanager,
            AppDbContext db,

            UserManager<AppUser> usermanager,
            SignInManager<AppUser> signInManager)
        {
            _roleManager = rolemanager;
            _userManager = usermanager;
            _signInManager = signInManager;

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
        public async Task<IActionResult> AddToCart(int id)
        {
            Product product =await  _db.Products.FirstOrDefaultAsync(p =>p.Id==id);
            
            if (product==null)
            {
                return NotFound();
            }
            
            var appuser = await _userManager.GetUserAsync(User);

            Order order = new Order()
            {   
                DishName=product.Name,
                Price=product.Price,
                Quatntity=1,
                RestaurantId=product.RestaurantID,
                Image=product.Image,
                AppUserId = appuser.Id,
                SubTotal=product.Price
                

            };
           await  _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Restaurants");
        }

    }
}
