using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sahiplendir.Areas.Admin.Models;
using Sahiplendir.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using X.PagedList.Mvc;

namespace Sahiplendir.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly AppDbContext context;

        public CategoriesController(AppDbContext context)
        {
            this.context = context;
        }

        private async Task PopulateDropdowns()
        {
            ViewBag.Categories = new SelectList(await context.Rayons.OrderBy(p => p.Name).ToListAsync(), "Id", "Name");
        }

        public IActionResult Index(int? page)
        {
            var model = context.Categories.Include(p => p.Rayon).ToList().ToPagedList(page ?? 1, 10); ;
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            var model = new Category { Enabled = true };
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Create(Category model)
        {
            model.DateCreated = DateTime.Now;
            context.Categories.Add(model);
            try
            {
                await context.SaveChangesAsync();
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
            var model = await context.Categories.FindAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category model)
        {
            context.Update(model);
            try
            {
                await context.SaveChangesAsync();
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
            var model = await context.Categories.FindAsync(id);
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