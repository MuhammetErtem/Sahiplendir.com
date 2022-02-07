using Microsoft.AspNetCore.Mvc;
using Sahiplendir.Areas.Admin.Models;
using Sahiplendir.Models;
using Sahiplendir.Sahiplendir;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sahiplendir.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BannersController : Controller
    {
        private readonly AppDbContext context;
        private readonly UtilsService utilsService;

        public BannersController(
            AppDbContext context,
            UtilsService utilsService)
        {
            this.context = context;
            this.utilsService = utilsService;
        }

        public IActionResult Index()
        {
            var model = context.Banners.ToList();
            return View(model);
        }

        public IActionResult Create()
        {
            var model = new Banner { Enabled = true };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Banner model)
        {
            if (model.ImageFile is null)
            {
                TempData["error"] = ErrorDescriber.NoImageError();
                return View(model);
            }

            utilsService.AddImage(model, new ResizeImageOptions { Width = 1920, Height = 480, Watermark = false });

            model.DateCreated = DateTime.Now;
            context.Banners.Add(model);
            try
            {
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["error"] = ErrorDescriber.ConstraintError("Görsel");
                return View(model);
            }
        }


        public async Task<IActionResult> Edit(int id)
        {
            var model = await context.Banners.FindAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Banner model)
        {
            if (model.ImageFile is not null)
                utilsService.AddImage(model, new ResizeImageOptions { Width = 1920, Height = 480, Watermark = false });

            context.Update(model);
            try
            {
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["error"] = ErrorDescriber.ConstraintError("Görsel");
                return View(model);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await context.Banners.FindAsync(id);
            context.Remove(model);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                TempData["error"] = ErrorDescriber.ConcurrencyError("Görsel");
            }
            return RedirectToAction(nameof(Index));
        }


    }
}