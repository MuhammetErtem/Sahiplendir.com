using Microsoft.AspNetCore.Mvc;
using Sahiplendir.Models;
using System.Linq;

namespace Sahiplendir.Components
{
    public class BrandsBarViewComponent : ViewComponent
    {
        private readonly AppDbContext context;

        public BrandsBarViewComponent(AppDbContext context)
        {
            this.context = context;
        }
        public IViewComponentResult Invoke()
        {
            var model = context.Brands.Where(p => p.Enabled).OrderBy(p => p.Name).ToList();
            return View(model);
        }
    }
}
