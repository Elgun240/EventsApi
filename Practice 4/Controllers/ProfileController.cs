using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
using Practice_4.DAL;
using Practice_4.Models;
using Practice_4.ViewModels;

namespace Practice_4.Controllers
{
    public class ProfileController : Controller
    {
        
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _db;
        public ProfileController(AppDbContext db, RoleManager<IdentityRole> rolemanager,
            UserManager<AppUser> usermanager,
            SignInManager<AppUser> signInManager)
        {
            _roleManager = rolemanager;
            _userManager = usermanager;
            _signInManager = signInManager;
            _db = db;
            
        }
        public async Task<IActionResult> MyProfile()
        {
            AppUser user = await _userManager.GetUserAsync(User);
            ProfileVM profileVM = new ProfileVM()
            {
                Firstname=user.Name,
                Lastname=user.Surname,
                Username=user.UserName,
                Email=user.Email,
                Phone=user.PhoneNumber,
                Adress=user.Adress,
            };
            if (TempData.ContainsKey("Error"))
            {
                ModelState.AddModelError("Error", TempData["Error"].ToString());
            }
            if (TempData.ContainsKey("Success"))
            {
                ModelState.AddModelError("Success", TempData["Success"].ToString());
            }
            if (TempData.ContainsKey("ErrorP"))
            {
                ModelState.AddModelError("ErrorP", TempData["ErrorP"].ToString());
            }
            if (TempData.ContainsKey("SuccessP"))
            {
                ModelState.AddModelError("SuccessP", TempData["SuccessP"].ToString());
            }
           
            await _db.SaveChangesAsync();
            return View(profileVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MyProfile(ProfileVM profileVM)
        {
            if (profileVM == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.GetUserAsync(User);
            user.Name = profileVM.Firstname;
            user.Surname = profileVM.Lastname;
            user.UserName = profileVM.Username;
            user.Email = profileVM.Email;
            user.Adress= profileVM.Adress;
            user.PhoneNumber = profileVM.Phone;
            await _db.SaveChangesAsync();
            ModelState.AddModelError("Success", "Profile info has been changed!");  
            return View(profileVM);
        }
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM changePasswordVM)
        {
            //if (!ModelState.IsValid)
            //{
            //    TempData["Error"] = "Fill in the field!";

            //    return RedirectToAction("MyProfile", "Profile");
            //}
            if(changePasswordVM.CurrentPassword==null|| changePasswordVM.NewPassword == null || changePasswordVM.CheckPassword == null)
            {
                TempData["ErrorP"] = "Fill in the field!";
                
                return RedirectToAction("MyProfile", "Profile");
            }

            var user = await _userManager.FindByIdAsync(User.Claims.First().Value);
            var result = await _userManager.ChangePasswordAsync(user, changePasswordVM.CurrentPassword, changePasswordVM.NewPassword);
            if (result.Succeeded)
            {
                
                TempData["SuccessP"] = "Password changes successfully";

                return RedirectToAction("MyProfile", "Profile");
            }
            else
            {

                TempData["ErrorP"] = "Incorrect format of password";

                return RedirectToAction("MyProfile", "Profile");
            }

            //return RedirectToAction("MyProfile", "Profile");
        }
        public async Task<IActionResult> Orders()
        {
            var appuser = await _userManager.GetUserAsync(User);
            OrderVM orderVM = new OrderVM()
            {
                PaidOrders= await _db.PaidOrders.Where(o=>o.AppUserId == appuser.Id).Where(d => d.Status != Status.Delivered).ToListAsync(),
               DeliveredOrders = await _db.PaidOrders.Where(d=>d.Status==Status.Delivered).ToListAsync(),

            };
            if (TempData.ContainsKey("Error"))
            {
                ModelState.AddModelError("Error", TempData["Error"].ToString());
            }
            return View(orderVM);
        }
        public IActionResult Invoice()
        {
            return View();
        }
        public async Task<IActionResult> CancelOrder(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = await _db.PaidOrders.FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return NotFound();      
            }
            order.Status = Status.Cancelled;
            await _db.SaveChangesAsync();
            TempData["Error"] = "Order has been cancelled!";
            return RedirectToAction("Orders");
        }

    }
  
}
