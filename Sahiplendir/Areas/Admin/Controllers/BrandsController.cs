using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sahiplendir.Areas.Admin.Models;
using Sahiplendir.Models;
using Sahiplendir.Sahiplendir;
using X.PagedList;

namespace Sahiplendir.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandsController : Controller
    {
        private readonly AppDbContext context;
        private readonly UtilsService utilsService;

        public BrandsController(AppDbContext context, UtilsService utilsService)
        {
            this.context = context;
            this.utilsService = utilsService;
        }


        public IActionResult Index(int? page)
        {
            var model = context.Brands.ToList().ToPagedList(page ?? 1, 10);
            return View(model);
        }


        public IActionResult Create()
        {
            var model = new Brand { Enabled = true };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Brand model)
        {
            if (model.ImageFile is null)
            {
                TempData["error"] = ErrorDescriber.NoImageError();
                return View(model);
            }

            model.Image = await utilsService.PrepareImage(model.ImageFile.OpenReadStream(), 120, 120);
            model.DateCreated = DateTime.Now;
            context.Brands.Add(model);
            try
            {
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["error"] = ErrorDescriber.ConstraintError(model.Name);
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await context.Brands.FindAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Brand model)
        {
            if (model.ImageFile is not null)
                model.Image = await utilsService.PrepareImage(model.ImageFile.OpenReadStream(), 120, 120);

            context.Brands.Update(model); // resim yenilemek isim yenilemek için kuyllanılır.
            try
            {
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception)
            {
                TempData["error"] = ErrorDescriber.ConstraintError(model.Name);
                return View(model);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await context.Brands.FindAsync(id);
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
