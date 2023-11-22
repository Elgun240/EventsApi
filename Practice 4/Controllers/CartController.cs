using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_4.DAL;
using Practice_4.Models;
using Practice_4.ViewModels;

namespace Practice_4.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public CartController(RoleManager<IdentityRole> rolemanager,
            AppDbContext db,

            UserManager<AppUser> usermanager,
            SignInManager<AppUser> signInManager)
        {
            _roleManager = rolemanager;
            _userManager = usermanager;
            _signInManager = signInManager;

            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var appuser = await _userManager.GetUserAsync(User);
            var orders = await _db.Orders.Where(o => o.AppUserId == appuser.Id).ToListAsync();



            OrderVM orderVM = new OrderVM()
            {
                Orders = await _db.Orders.Where(o => o.AppUserId == appuser.Id).ToListAsync(),
                Adress = appuser.Adress,
                AppUserId = appuser.Id



            };

            foreach (var order in orderVM.Orders)
            {
                orderVM.SubTotal += order.Price;
            }
            return View(orderVM);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeQuantity(int count, int productid, int resId)
        {

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var order = await _db.Orders.FirstOrDefaultAsync(x => x.AppUserId == user.Id && x.Id == productid && x.RestaurantId == resId);

            if (order != null)
            {
                order.Quatntity = count;
                order.SubTotal = order.Price * order.Quatntity;
                _db.Orders.Update(order);
                await _db.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout()
        {
            //    // Получите данные из запроса, включая grandTotal
            //    var formData = HttpContext.Request.Form;
            //    var grandTotal = formData["grandTotal"];

            //    // Создайте объект OrderVM и установите его свойства
            //    var orderVM = new OrderVM
            //    {
            //        // Установите свойства объекта на основе полученных данных
            //        GrandTotal = grandTotal,
            //        // Другие свойства OrderVM
            //    };

            //    return View(orderVM);
            var appuser = await _userManager.GetUserAsync(User);
            List<Order> orders = await _db.Orders.Where(o => o.AppUserId == appuser.Id).Include(o=>o.Restaurant).ToListAsync();
            CheckoutVM checkoutVM = new CheckoutVM()
            {
                Orders = orders,
                Adress = appuser.Adress

            };

            return View(checkoutVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmOrder(CheckoutVM checkoutVM)
        {
            if (checkoutVM == null)
            {
                NotFound();
            }
            var user =await _userManager.GetUserAsync(User);
            var paidOrders = await _db.PaidOrders.ToListAsync();

            foreach (var item in checkoutVM.Orders)
            {
                var existingPaidOrder = paidOrders.FirstOrDefault(p => p.Name == item.DishName);

                if (existingPaidOrder != null && existingPaidOrder.Status==Status.Dispatch )
                {
                    
                    existingPaidOrder.Quantity += item.Quatntity;
                }
                else
                {
                    
                    PaidOrder paidOrder = new PaidOrder()
                    {
                        RestaurantId = item.RestaurantId,
                        Price = item.Price,
                        Quantity = item.Quatntity, 
                        Name = item.DishName,
                        AppUserId = user.Id,
                        OrderDate = DateTime.Now,
                        Status = Status.Dispatch,
                        Total = item.Price*(double)item.Quatntity,
                        Adress=checkoutVM.Adress
                        
                    };
                    _db.PaidOrders.Add(paidOrder);
                }
            }
            foreach (var item in checkoutVM.Orders)
            {
                var orderToDelete = _db.Orders.FirstOrDefault(o => o.Id == item.Id);
                if (orderToDelete != null)
                {
                    _db.Orders.Remove(orderToDelete);
                }
            }
            await _db.SaveChangesAsync();

            return RedirectToAction("Orders","Profile");
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var deletorder = await _db.Orders.FirstOrDefaultAsync(o => o.Id == id);

            if (deletorder == null)
            {
                return NotFound();
            }
            _db.Orders.Remove(deletorder);
            await _db.SaveChangesAsync();


            return RedirectToAction("Index");
        }
    }
}
