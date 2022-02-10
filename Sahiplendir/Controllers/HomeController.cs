using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sahiplendir.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sahiplendir.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(AppDbContext context,ILogger<HomeController> logger)
        {
            this.context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.FeaturedProducts = await context.Products.Include(p => p.Category).Include(p => p.Brand).Where(p => p.Enabled).OrderBy(p => Guid.NewGuid()).Take(9).ToListAsync();
            //ViewBag Controllerdaki action veriyi View'e taşımak için kullanılır.
            ViewBag.Banners = await context.Banners.Where(p => p.Enabled).ToListAsync();
            return View();
        }
        public async Task<IActionResult> Category(int id)
        {
            var model = await context.Categories.Include(p => p.Rayon).Include(p => p.Products).ThenInclude(p => p.Brand).SingleOrDefaultAsync(p => p.Id == id && p.Enabled);
            return View(model);
        }
        public async Task<IActionResult> Brands(int id)
        {
            var model = await context.Brands.Include(p => p.Products).ThenInclude(p => p.Category).SingleOrDefaultAsync(p => p.Id == id && p.Enabled);
            return View(model);
        }

        public async Task<IActionResult> Search(string keyword)
        {
            var keywords = Regex.Split(keyword, @"\s+").ToList();
            var model = context.Products.Include(p => p.Brand).Include(p => p.Category).AsEnumerable().Where(p => keywords.Any(q => p.Name.ToLower().Contains(q.ToLower()))).ToList();
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Contact()
        {
            return View();
        }

    }
}
