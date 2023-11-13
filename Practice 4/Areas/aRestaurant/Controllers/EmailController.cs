using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_4.DAL;
using Practice_4.Helpers;
using Practice_4.Models;
using Practice_4.ViewModels;


namespace Practice_4.Areas.aRestaurant.Controllers
{
    [Area("aRestaurant")]
    [Authorize(Roles="Restaurant")]
    public class EmailController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
       

        public EmailController(AppDbContext db,UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _db = db;
            
        }

       
        public async Task<IActionResult> SendMail(string id , int ComId)
        {
            if (id == null)
            {
                return NotFound();
            }
            var appuser = await _userManager.FindByIdAsync(id);

            if (appuser == null)
            {
                NotFound();
            }
            MailVM mailVM = new MailVM()
            {
                Email = appuser.Email,
                Comment= await _db.Comments.FirstOrDefaultAsync(c => c.Id == ComId)

            };
            
           
            return View(mailVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMail(MailVM model,int id)
        {
         
                string toEmail = model.Email;
                string subject = model.Title;
                string message = model.Text;
            
            IEmailService emailService = new EmailService();
            await emailService.SendEmailAsync(toEmail, subject, message);
            var comment = await _db.Comments.Include(c=>c.AppUser).FirstOrDefaultAsync(c=>c.Id == id);
            if (model.Email.ToString() != comment.AppUser.Email.ToString())
            {
                return NotFound();
            }
            comment.Status = "Answered";
            await _db.SaveChangesAsync();
            TempData["Success"] = "Mail sended successfully!";
            return RedirectToAction("Index", "Comment");
            
        }
    }
}
