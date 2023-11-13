using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Practice_4.DAL;
using Practice_4.Helpers;
using Practice_4.Models;
using Practice_4.ViewModels;

namespace Practice_4.Areas.Admin.Controllers
{
    [Area("aAdmin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _env;
        public UserController(RoleManager<IdentityRole> rolemanager,
            AppDbContext db,
            
            UserManager<AppUser> usermanager,
            SignInManager<AppUser> signInManager)
        {
            _roleManager = rolemanager;
            _userManager = usermanager;
            _signInManager = signInManager;
            
            _db = db;
        }
        public async Task<IActionResult> Manage(string Id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == Id);
            UserVM userVM = new UserVM()
            {
                Username = user.UserName,
                Firstname = user.Name,
                Lastname = user.Name,
                Email = user.Email,
                Adress = user.Adress,
                Phone = user.PhoneNumber,
                Id = user.Id
                
            };
            if (TempData.ContainsKey("PasswordError"))
            {
                ModelState.AddModelError("PasswordError", TempData["PasswordError"].ToString());
            }
            return View(userVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(UserVM userVM,string Id)
        {
            if (userVM.Firstname == null)
            {

            }
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == Id);
            user.Name = userVM.Firstname;
            user.Surname = userVM.Lastname;
            user.Email = userVM.Email;
            user.UserName = userVM.Username;
            user.Adress= userVM.Adress;
            user.PhoneNumber = userVM.Phone;
            if (userVM.Password != null)
            {
                
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, userVM.Password);
              
                if(!result.Succeeded) 
                {
                    TempData["PasswordError"] = "Wrong password format!";
                    return RedirectToAction("Manage", "User", new { Id = Id});
                }
            }
            await _db.SaveChangesAsync();
            TempData["Success"] = "User's info changed successfully!";
            return RedirectToAction("List" , "User" );
        }
        public async Task<IActionResult> Create()
        {
            RegisterVM registerVM = new RegisterVM()
            {
                Restaurants = await _db.Restaurants.ToListAsync(),
            };
            return View(registerVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterVM registerVM)
        {
            var users = await _db.Users.ToListAsync();
            foreach (var item in users)
            {
                if (item.UserName == registerVM.Username)
                {
                    TempData["Error"] = "This usarename is already taken!";
                    return RedirectToAction("List", "User");
                }
            }
            var selectedrestaurant = await _db.Restaurants.FirstOrDefaultAsync(x=>x.Name==registerVM.Restaurant);
            AppUser newUser = new AppUser
            {
                Name = registerVM.Firstname,
                Surname = registerVM.Lastname,
                Email = registerVM.Email,
                UserName = registerVM.Username,
                Adress = registerVM.Adress,
                RestaurantId =selectedrestaurant.Id,

            };
            var identityResult = await _userManager.CreateAsync(newUser, registerVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            //await CreateRole();
            if (registerVM.Role == "Member")
            {
                await _userManager.AddToRoleAsync(newUser, Roles.Member.ToString());
            }
            if (registerVM.Role == "Restaurant Admin")
            {
                await _userManager.AddToRoleAsync(newUser, Roles.Restaurant.ToString());
            }
            if (registerVM.Role == "Admin")
            {
                await _userManager.AddToRoleAsync(newUser, Roles.Admin.ToString());
            }
            
            
            await _db.SaveChangesAsync();
            TempData["Success"] = "User added successfully!";
            return RedirectToAction("List", "User");
        }
        public async Task<IActionResult> List()
        {

            var users =await _db.Users.ToListAsync();
            if (TempData.ContainsKey("Success"))
            {
                ModelState.AddModelError("Success", TempData["Success"].ToString());
            }
            if (TempData.ContainsKey("Error"))
            {
                ModelState.AddModelError("Error", TempData["Error"].ToString());
            }
            return View(users);
        }
        public async Task<IActionResult> Delete(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
             await _userManager.DeleteAsync(user);
            await _db.SaveChangesAsync();
            TempData["Error"] = "User was deleted!";
            return RedirectToAction("List", "User");
        }
    }
}
