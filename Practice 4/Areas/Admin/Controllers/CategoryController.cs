using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_4.DAL;
using Practice_4.Helpers;
using Practice_4.Models;

namespace Practice_4.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _db;
        public CategoryController(AppDbContext db)
        {
            _db=db;
        }
        public async Task<IActionResult> Manage(int id)
        {
            var category =await  _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(Category category , int id)
        {
            var dbcategory =await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
            dbcategory.Name=category.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction("List" );
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            Category newcat = new Category() { Name=category.Name};
            await _db.Categories.AddAsync(newcat);
            await _db.SaveChangesAsync();
            return RedirectToAction("List" );
        }
        public async Task<IActionResult> List()
        {
           var categories= await _db.Categories.ToListAsync();
            return View(categories);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var dcat= await _db.Categories.FirstOrDefaultAsync(c=>c.Id==id);
            if (dcat==null)
            {
                return NotFound();
            }
             _db.Categories.Remove(dcat);
             await _db.SaveChangesAsync();
            return RedirectToAction("List" );
    }
    }
}
