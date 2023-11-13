using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Practice_4.DAL;
using Practice_4.Models;

namespace Practice_4.Controllers
{
    public class CommentController : Controller
    {
        
        private readonly AppDbContext _db;
        public CommentController(AppDbContext db)
        {
            _db=db;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm]Comment comment)
        {
            if(comment == null)
            {
                NotFound();
            }
            Comment newcomment = new Comment()
            {
                Title=comment.Title,
                Description=comment.Description,
                AppUserId=comment.AppUserId,
                Status="NotAnswered",
                CreateTime=DateTime.Now,
                
            };
           await _db.Comments.AddAsync(newcomment);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Comment has been sended!";
            return RedirectToAction("Index","Home");
        }
    }
}
