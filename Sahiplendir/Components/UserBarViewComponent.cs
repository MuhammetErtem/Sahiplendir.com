using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sahiplendir.Models;

namespace Sahiplendir.Components
{
    public class UserBarViewComponent : ViewComponent
    {
        private readonly UserManager<User> userManager;

        public UserBarViewComponent(
            UserManager<User> userManager
            )
        {
            this.userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            User model = null;

            if (User.Identity.IsAuthenticated)
                model = userManager.FindByNameAsync(User.Identity.Name).Result;
            return View(model);
            
        }
    }
}
