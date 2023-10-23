using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ProfileVM profileVM)
        {
            //if (!ModelState.IsValid)
            //{
            //    TempData["Error"] = "Fill in the field!";

            //    return RedirectToAction("MyProfile", "Profile");
            //}
            if(profileVM.CurrentPassword==null|| profileVM.NewPassword == null || profileVM.CheckPassword == null)
            {
                TempData["ErrorP"] = "Fill in the field!";
                
                return RedirectToAction("MyProfile", "Profile");
            }
            
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var result = await _userManager.ChangePasswordAsync(user, profileVM.CurrentPassword, profileVM.NewPassword);
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

            return RedirectToAction("MyProfile", "Profile");
        }
        public IActionResult Orders()
        {
            return View();
        }
        public IActionResult Invoice()
        {
            return View();
        }

    }
}
