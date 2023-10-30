using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practice_4.DAL;
using Practice_4.Helpers;
using Practice_4.Models;
using Practice_4.ViewModels;


namespace Practice_4.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        
        public OrdersController(RoleManager<IdentityRole> rolemanager,
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
            var orders = await _db.PaidOrders.Include(o=>o.AppUser).ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> Process(int prId)
        {
           
            
            ProcessVM vm = new ProcessVM()
            {
                PaidOrder = await _db.PaidOrders.FirstOrDefaultAsync(p=>p.Id==prId),
               
            };

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Process(ProcessVM processVM,int id) 
        {
            if (id == 0)
            {
                NotFound();
            }
           if(processVM == null)
            {
                NotFound();
            }
           var paidorder = await _db.PaidOrders.FirstOrDefaultAsync(o=>o.Id==id);
            switch (processVM.Status)
            {
               
                case Status.OnWay:
                    paidorder.Status = Status.OnWay;
                    break;
                case Status.Cancelled:
                    paidorder.Status = Status.Cancelled;
                    break;
                case Status.Delivered:
                    paidorder.Status = Status.Delivered;
                    break;
                default:
                    paidorder.Status = Status.Dispatch;
                    break;
            }
           
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult>Delete(int id)
        {
            if(id == null)
            {
                NotFound();
            }
            var paidorder = await _db.PaidOrders.FirstOrDefaultAsync(o=>o.Id==id);
            if (paidorder == null)
            {
                return NotFound();
            }
            _db.PaidOrders.Remove(paidorder);
           await  _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


    }
}
