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
    public class AnimalsController : Controller
    {
        private readonly AppDbContext context;
        private readonly UtilsService utilsService;
        private readonly string entityName = "Ürün";


        public AnimalsController(AppDbContext context, UtilsService utilsService)
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
            var model = context.Animals.Include(p => p.Category).ThenInclude(p => p.Rayon).ToList().ToPagedList(page ?? 1, 10);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            var model = new Animal { Enabled = true };
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Create(Animal model)
        {
            model.Image = await utilsService.PrepareImage(model.ImageFile?.OpenReadStream(), 800, 800, false);
            if (model.ImageFiles is not null)

                foreach (var imageFile in model.ImageFiles)

                    model.AnimalImages.Add
                        (
                        new AnimalImage { Image = await utilsService.PrepareImage(imageFile.OpenReadStream(), 800, 800, true) }
                        );

            model.DateCreated = DateTime.Now;


            context.Animals.Add(model);
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
            var model = await context.Animals.Include(p => p.AnimalImages).SingleAsync(p => p.Id == id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Animal model)
        {
            {
                utilsService.AddImage(model, new ResizeImageOptions { Width = 800, Height = 800, Watermark = false });
                if (model.ImageFiles is not null)
                    foreach (var imageFile in model.ImageFiles)
                        model.AnimalImages.Add(
                            new AnimalImage { Image = await utilsService.PrepareImage(imageFile.OpenReadStream(), 800, 800, false) }
                            );

                if (model.ImagesToDeleted is not null)
                {
                    foreach (var item in model.ImagesToDeleted)
                    {
                        var animalImage = await context.AnimalImages.FindAsync(item);
                        context.Remove(animalImage);
                    }
                }
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
        }
                public async Task<IActionResult> Delete(int id)
                {
                    var model = await context.Animals.FindAsync(id);
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