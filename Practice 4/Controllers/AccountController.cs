using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_4.DAL;
using Practice_4.Helpers;
using Practice_4.Models;
using Practice_4.ViewModels;

namespace Practice_4.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        
        public AccountController(RoleManager<IdentityRole> rolemanager,
            AppDbContext db,
            
            UserManager<AppUser> usermanager,
            SignInManager<AppUser> signInManager)
        {
            _roleManager = rolemanager;
            _userManager = usermanager;
            _signInManager = signInManager;
            
            _db = db;
        }
        public IActionResult Login()
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    return NotFound();
            //}
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser appUser = await _db.Users.FirstOrDefaultAsync(x => x.UserName == loginVM.Username);
            if (appUser == null)
            {
                ModelState.AddModelError("Error", "Email or password is wrong");
                return View();
            }
            if (appUser.IsDeactive)
            {
                ModelState.AddModelError("Error", "Your account has been blocked");
                return View();
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, false, true);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("Error", "Your account locked out for 1 min");
                return View();
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("Error", "Email or password is wrong");
                return View();
            }

            return RedirectToAction("Index", "Home");
            

           
        }
        public IActionResult Register()
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    return NotFound();
            //}
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            AppUser newUser = new AppUser
            {
                Name = registerVM.Firstname,
                Surname = registerVM.Lastname,
                Email = registerVM.Email,
                UserName = registerVM.Username,
                Adress = registerVM.Adress,

            };
            var identityResult = await _userManager.CreateAsync(newUser,registerVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            //await CreateRole();
            await _userManager.AddToRoleAsync(newUser, Roles.Member.ToString());
            await _signInManager.SignInAsync(newUser,true);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task CreateRole()
        {
            if (!(await _roleManager.RoleExistsAsync(Roles.Admin.ToString())))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Admin.ToString() });
            }
            else if (!(await _roleManager.RoleExistsAsync(Roles.Member.ToString())))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Member.ToString() });
            }
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        
       
    }
}
