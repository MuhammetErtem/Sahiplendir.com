using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sahiplendir.Models;
using System.Linq;

namespace Sahiplendir.Components
{
    public class RayonBarViewComponent : ViewComponent
    {

        private readonly AppDbContext context;

        public RayonBarViewComponent(AppDbContext context)
        {
            this.context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View(context.Rayons.Include(p => p.Categories).Where(p => p.Enabled).ToList());
        }

    }
}