using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_4.DAL;
using Practice_4.Helpers;
using Practice_4.Models;

namespace Practice_4.Areas.Admin.Controllers
{
    [Area("aAdmin")]
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
            if(id== 0)
            {
                return NotFound();
            }
            var category =await  _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(Category category , int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Category existcategory = await _db.Categories.FirstOrDefaultAsync (c => c.Name ==category.Name);
            if (existcategory != null)
            {
                ModelState.AddModelError("Name", "This name is already exist");
                return View(category);
            }
            var dbcategory =await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (dbcategory == null)
            {
                return NotFound();
            }
            dbcategory.Name=category.Name;
            await _db.SaveChangesAsync();
            TempData["Success"] = "Category changed successfully!";
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

            var categories = _db.Categories.FirstOrDefault(x=>x.Name==category.Name);
            if(categories != null)
            {
                ModelState.AddModelError("Name", "This name is already exist");
                return View();
            }
            Category newcat = new Category() { Name=category.Name};
            await _db.Categories.AddAsync(newcat);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Category added successfully!";
            return RedirectToAction("List" );
        }
        public async Task<IActionResult> List()
        {
           var categories= await _db.Categories.ToListAsync();
            if (TempData.ContainsKey("Success"))
            {
                ModelState.AddModelError("Success", TempData["Success"].ToString());
            }
            if (TempData.ContainsKey("Error"))
            {
                ModelState.AddModelError("Error", TempData["Error"].ToString());
            }
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
