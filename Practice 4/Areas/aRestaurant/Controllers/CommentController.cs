using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_4.DAL;
using Practice_4.Models;
using Practice_4.ViewModels;
using System.Runtime.CompilerServices;

namespace Practice_4.Areas.aRestaurant.Controllers
{
    [Area("aRestaurant")]
    [Authorize(Roles ="Restaurant")]
    public class CommentController : Controller
    {
        private readonly AppDbContext _db;
    
        private readonly UserManager<AppUser> _userManager;
     

        public CommentController(RoleManager<IdentityRole> rolemanager,
            AppDbContext db,

            UserManager<AppUser> usermanager,
            SignInManager<AppUser> signInManager)
        {
          
            _userManager = usermanager;
         

            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var comments = await _db.Comments.Include(c=>c.AppUser).Where(c=>c.Status=="NotAnswered").ToListAsync();
            if (TempData.ContainsKey("Success"))
            {
                ModelState.AddModelError("Success", TempData["Success"].ToString());
            }
            if (TempData.ContainsKey("Error"))
            {
                ModelState.AddModelError("Error", TempData["Error"].ToString());
            }
            return View(comments);
        }
        public async Task<IActionResult> AnsweredComments()
        {
            var comments = await _db.Comments.Include(_ => _.AppUser).Where(c => c.Status == "Answered").ToListAsync();
            if (TempData.ContainsKey("Success"))
            {
                ModelState.AddModelError("Success", TempData["Success"].ToString());
            }
            if (TempData.ContainsKey("Error"))
            {
                ModelState.AddModelError("Error", TempData["Error"].ToString());
            }
            return View(comments);
        }
        public async Task<IActionResult> Delete(int id)
        {
            bool isAnswered = false;
            var comment = await _db.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (comment != null)
            {
                NotFound();
            }
            if (comment.Status == "Answered")
            {
                isAnswered = true;
            }
            _db.Comments.Remove(comment);
            await _db.SaveChangesAsync();
            if (isAnswered)
            {
                TempData["Error"] = "Comment has been deleted!";
                return RedirectToAction("AnsweredComments", "Comment");
            }
            TempData["Error"] = "Comment has been deleted!";
            return RedirectToAction("Index", "Comment");
        }
        

    }
}
