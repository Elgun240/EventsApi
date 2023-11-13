using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Practice_4.DAL;
using Practice_4.Helpers;
using Practice_4.Models;
using Practice_4.ViewModels;

namespace Practice_4.Areas.Admin.Controllers
{
    [Area("aAdmin")]
    [Authorize(Roles = "Admin")]
    public class RestaurantController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public RestaurantController(AppDbContext db, IWebHostEnvironment env)
        {
            _env = env;
            _db = db;
        }
        public async Task<IActionResult> Create()
        {
            RestaurantVM restaurantVM = new RestaurantVM() { 
                Categories = _db.Categories.ToList(),
            };
            
            return View(restaurantVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RestaurantVM restaurantVM)
        {
            
            if (restaurantVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "Select photo");
                return View(restaurantVM);
            }
            if (!restaurantVM.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Select image file");
                return View(restaurantVM);
            }
            if (restaurantVM.Photo.IsMore4mb())
            {
                ModelState.AddModelError("Photo", "Max size photo is 4 mb");
                return View(restaurantVM);
            }
            string path = Path.Combine(_env.WebRootPath, @"assets\imgs\uploads\restaurant");
            var existrestaurant = await _db.Restaurants.FirstOrDefaultAsync(x => x.Name == restaurantVM.Name);
            if (existrestaurant != null)
            {
                restaurantVM.Categories = await _db.Categories.ToListAsync();
                ModelState.AddModelError("Name", "This restaurant name is already taken!");
                return View(restaurantVM);
            }
            Restaurant restaurant = new Restaurant()
            {
                Name= restaurantVM.Name,
                Email=restaurantVM.Email,
                Contact= restaurantVM.Contact,
                URL=restaurantVM.URL,
                OpeningTime=restaurantVM.OpeningTime,
                ClosingTime=restaurantVM.ClosingTime,
                OenDays=restaurantVM.OenDays,
                CategoryId=restaurantVM.CategoryId,
                Adress=restaurantVM.Adress,
                Description=restaurantVM.Description,
                


            };
            restaurant.Image = await restaurantVM.Photo.SaveImageAsync(path);
            await _db.Restaurants.AddAsync(restaurant);
            await _db.SaveChangesAsync();
            
          
            TempData["Success"] = "Store has added successfully!";
            return  RedirectToAction("List" , "Restaurant");
        }

        public async Task<IActionResult> Manage(int Id)
        {
           
            EditRestaurantVM editRestaurantVM = new EditRestaurantVM()
            {
                 Restaurant = await _db.Restaurants.Include(x=>x.Category).FirstOrDefaultAsync(r=>r.Id==Id),
                 Categories = await _db.Categories.ToListAsync(),

             };
            if (TempData.ContainsKey("Photo"))
            {
                ModelState.AddModelError("Photo", TempData["Photo"].ToString());
            }
            return View(editRestaurantVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(EditRestaurantVM restaurantVM,int Id)
        {
           
            EditRestaurantVM editRestaurantVM = new EditRestaurantVM()
            {
                Restaurant = await _db.Restaurants.Include(x => x.Category).FirstOrDefaultAsync(r => r.Id == Id),
                Categories = await _db.Categories.ToListAsync(),

            };
            var restaurant = await _db.Restaurants.FirstOrDefaultAsync(r=>r.Id==Id);
            restaurant.Name=restaurantVM.Restaurant.Name;
            restaurant.Email=restaurantVM.Restaurant.Email;
            restaurant.Adress = restaurantVM.Restaurant.Adress;
            restaurant.Contact=restaurantVM.Restaurant.Contact;
            restaurant.OpeningTime=restaurantVM.Restaurant.OpeningTime;
            restaurant.ClosingTime  =restaurantVM.Restaurant.ClosingTime;
            restaurant.OenDays=restaurantVM.Restaurant.OenDays;
            restaurant.CategoryId = restaurantVM.CategoryId;
            if (restaurantVM.Restaurant.Photo != null)
            {
                if (!restaurantVM.Restaurant.Photo.IsImage())
                {
                    
                    TempData["Photo"] = "Select image file";
                    return RedirectToAction("Manage" , "Restaurant" , new {Id=Id});
                }
                if (restaurantVM.Restaurant.Photo.IsMore4mb())
                {
                    TempData["Photo"] = "Select image file";
                    return RedirectToAction("Manage", "Restaurant", new { Id = Id });
                }
                string path = Path.Combine(_env.WebRootPath, @"assets\imgs\uploads\restaurant");
                restaurant.Image = await restaurantVM.Restaurant.Photo.SaveImageAsync(path);

            }

            await _db.SaveChangesAsync();


            TempData["Success"] = "Store has changed successfully!";
            return RedirectToAction("List", "Restaurant");
        }
        public async Task<IActionResult> List()
        {
            var stores = await _db.Restaurants.ToListAsync();
            if (TempData.ContainsKey("Success"))
            {
                ModelState.AddModelError("Success", TempData["Success"].ToString());
            }
            if (TempData.ContainsKey("Error"))
            {
                ModelState.AddModelError("Error", TempData["Error"].ToString());
            }
            return View(stores);
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();  
            }
            var store = await _db.Restaurants.FirstOrDefaultAsync(r=> r.Id==id);
            _db.Restaurants.Remove(store);
            await _db.SaveChangesAsync();
            TempData["Error"] = "Restaurant has been deleted";
            return RedirectToAction("List", "Restaurant");
        }
        public async Task<IActionResult> GeneratePdfReportRestaurant()
        {
            var restaurants = _db.Restaurants.ToList();

            var pdfDocument = new PdfDocument();
            var pdfPage = pdfDocument.AddPage();
            var graph = XGraphics.FromPdfPage(pdfPage);
            var font = new XFont("Verdana", 12, XFontStyle.Regular);
            var yPosition = 50;
            var filePath = Path.Combine(_env.ContentRootPath, "RestaurantReport.pdf");

            foreach (var restaurant in restaurants)
            {
                int total = 0;
                var orders = await _db.PaidOrders.Where(x => x.RestaurantId == restaurant.Id).ToListAsync();
                foreach (var item in orders)
                {
                    total += item.Quantity;

                }
                graph.DrawString($"Restaurant ID: {restaurant.Id}", font, XBrushes.Black, new XRect(50, yPosition, pdfPage.Width, pdfPage.Height), XStringFormats.TopLeft);
                yPosition += 20;
                graph.DrawString($"Restaurant Name: {restaurant.Name}", font, XBrushes.Black, new XRect(50, yPosition, pdfPage.Width, pdfPage.Height), XStringFormats.TopLeft);
                yPosition += 20;
                graph.DrawString($"Total sales: {total}", font, XBrushes.Black, new XRect(50, yPosition, pdfPage.Width, pdfPage.Height), XStringFormats.TopLeft);
                yPosition += 30;
            }

            // Сохраните PDF-файл в папку проекта
            using (MemoryStream stream = new MemoryStream())
            {
                pdfDocument.Save(filePath);
                return File(stream.ToArray(), "application/pdf", "RestaurantReport.pdf");
            }
        }


    }
}
