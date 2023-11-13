using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Practice_4.DAL;
using Practice_4.Helpers;
using Practice_4.Models;
using Practice_4.ViewModels;
using System.Runtime.CompilerServices;

namespace Practice_4.Areas.Admin.Controllers
{
    [Area("aAdmin")]
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public OrdersController(RoleManager<IdentityRole> rolemanager,
            IWebHostEnvironment webHostEnvironment,
            AppDbContext db,

            UserManager<AppUser> usermanager,
            SignInManager<AppUser> signInManager)
        {
            _webHostEnvironment = webHostEnvironment;   
            _roleManager = rolemanager;
            _userManager = usermanager;
            _signInManager = signInManager;

            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await _db.PaidOrders.Include(o=>o.AppUser).ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> Process(int prId)
        {
           if(prId == 0)
            {
                return NotFound();
            }
            
            ProcessVM vm = new ProcessVM()
            {
                PaidOrder = await _db.PaidOrders.FirstOrDefaultAsync(p=>p.Id==prId),
               
            };
            if(vm.PaidOrder == null)
            {
                return NotFound();
            }

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Process(ProcessVM processVM,int id) 
        {
            if (id == 0)
            {
                NotFound();
            }
           if(processVM == null)
            {
                NotFound();
            }
           var paidorder = await _db.PaidOrders.FirstOrDefaultAsync(o=>o.Id==id);
            if(paidorder == null)
            {
                return NotFound();
            }
            switch (processVM.Status)
            {
               
                case Status.OnWay:
                    paidorder.Status = Status.OnWay;
                    break;
                case Status.Cancelled:
                    paidorder.Status = Status.Cancelled;
                    break;
                case Status.Delivered:
                    paidorder.Status = Status.Delivered;
                    break;
                default:
                    paidorder.Status = Status.Dispatch;
                    break;
            }
           
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult>Delete(int id)
        {
            if(id == null)
            {
                NotFound();
            }
            var paidorder = await _db.PaidOrders.FirstOrDefaultAsync(o=>o.Id==id);
            if (paidorder == null)
            {
                return NotFound();
            }
            _db.PaidOrders.Remove(paidorder);
           await  _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult GeneratePdfReport()
        {
            var orders = _db.PaidOrders.ToList();

            var pdfDocument = new PdfDocument();
            var pdfPage = pdfDocument.AddPage();
            var graph = XGraphics.FromPdfPage(pdfPage);
            var font = new XFont("Verdana", 12, XFontStyle.Regular);
            var yPosition = 50;
            var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "OrderReport.pdf");
            foreach (var order in orders)
            {
                // Создайте путь к файлу PDF в папке проекта


                graph.DrawString($"Order ID: {order.Id}", font, XBrushes.Black, new XRect(50, yPosition, pdfPage.Width, pdfPage.Height), XStringFormats.TopLeft);
                yPosition += 20;
                graph.DrawString($"Dish Name: {order.Name}", font, XBrushes.Black, new XRect(50, yPosition, pdfPage.Width, pdfPage.Height), XStringFormats.TopLeft);
                yPosition += 20;
                graph.DrawString($"Quantity Sold: {order.Quantity}", font, XBrushes.Black, new XRect(50, yPosition, pdfPage.Width, pdfPage.Height), XStringFormats.TopLeft);
                yPosition += 30;
            }

            // Сохраните PDF-файл в папку проекта
            using (MemoryStream stream = new MemoryStream())
            {
                pdfDocument.Save(filePath);
                return File(stream.ToArray(), "application/pdf", "OrderReport.pdf");
            }
        }


    }
}
