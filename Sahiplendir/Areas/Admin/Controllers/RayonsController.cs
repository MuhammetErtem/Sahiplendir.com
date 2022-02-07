using Microsoft.AspNetCore.Mvc;
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
    public class RayonsController : Controller
    {
        private readonly AppDbContext context;

        public RayonsController(AppDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index(int? page)
        {
            var model = context.Rayons.ToList().ToPagedList(page ?? 1, 10);
            return View(model);
        }

        public IActionResult Create()
        {
            var model = new Rayon { Enabled = true };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Rayon model)
        {
            model.DateCreated = DateTime.Now;
            context.Rayons.Add(model);
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
            var model = await context.Rayons.FindAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Rayon model)
        {
            context.Update(model);
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
            var model = await context.Rayons.FindAsync(id);
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