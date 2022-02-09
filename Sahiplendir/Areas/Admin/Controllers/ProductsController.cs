using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sahiplendir.Areas.Admin.Models;
using Sahiplendir.Models;
using Sahiplendir.Sahiplendir;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Sahiplendir.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly AppDbContext context;
        private readonly UtilsService utilsService;
        private readonly string entityName = "Ürün";

        public ProductsController(AppDbContext context, UtilsService utilsService)
        {
            this.context = context;
            this.utilsService = utilsService;

        }

        private async Task PopulateDropdowns()
        {
            ViewBag.Categories = new SelectList(await context.Categories.Include(p => p.Rayon).OrderBy(p => p.Name).ToListAsync(), "Id", "Name", null, "Rayon.Name");
            //Kategorisi için liste oluşturduk.
            ViewBag.Brands = new SelectList(await context.Brands.OrderBy(p => p.Name).ToListAsync(), "Id", "Name"); 
            //Markanın ismine göre sıraladık ve ondan bir liste oluşturduk.Bize ismi görünecek kayıt ederken Id si kayıt olacak.
        }

        public IActionResult Index(int? page)
        {
            var model = context.Products.Include(p => p.Category).ThenInclude(p => p.Rayon).ToList().ToPagedList(page ?? 1, 10);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            var model = new Product { Enabled = true };
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Create(Product model)
        {
            model.Image = await utilsService.PrepareImage(model.ImageFile?.OpenReadStream(), 800, 800, false);
            if (model.ImageFiles is not null)

                foreach (var imageFile in model.ImageFiles)

                    model.ProductImages.Add
                        (
                        new ProductImage { Image = await utilsService.PrepareImage(imageFile.OpenReadStream(), 800, 800, true) }
                        );

            model.Price = decimal.Parse(model.PriceText, CultureInfo.CreateSpecificCulture("tr-TR")); //Giren kullanıcının pc'sini türkçe kültürüne çevirerek göster.

            model.DateCreated = DateTime.Now;


            context.Products.Add(model);
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entityName} ekleme işlemi başarıyla tamamlanmıştır!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                await PopulateDropdowns();
                TempData["error"] = ErrorDescriber.ConstraintError(model.Name);
                return View(model);
            }
        }
        public async Task<IActionResult> Edit(int id)
        {
            await PopulateDropdowns();
            var model = await context.Products.Include(p=>p.ProductImages).SingleAsync(p=>p.Id == id);
            model.PriceText = model.Price.ToString("n2",CultureInfo.CreateSpecificCulture("tr-TR"));
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product model)
        {
            model.Price = decimal.Parse(model.PriceText, CultureInfo.CreateSpecificCulture("tr-TR"));//Giren kullanıcının pc'sini türkçe kültürüne çevirerek göster.

            context.Update(model);
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entityName} güncelleme işlemi başarıyla tamamlanmıştır!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                await PopulateDropdowns();
                TempData["error"] = ErrorDescriber.ConstraintError(model.Name);
                return View(model);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await context.Products.FindAsync(id);
            context.Remove(model);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                TempData["error"] = ErrorDescriber.ConcurrencyError(model.Name);
            }
            return RedirectToAction(nameof(Index));
        }


    }
}