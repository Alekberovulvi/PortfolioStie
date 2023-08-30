using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PortfolioSite.Models;

namespace PortfolioSite.Areas.Manage.Controllers
{
    [Area("manage")]

    public class AdminController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (model.Email == "elekberovulvi520@gmail.com" && model.Password == "ulvifcbgts")
            {
                HttpContext.Session.SetString("Email", model.Email);

                return RedirectToAction("Index", "Dashboard", new {area= "manage"});
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Email və ya şifrə yanlışdır");
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Email");
            return RedirectToAction("Login");
        }

        public class AdminAuthorizeAttribute : Attribute, IAuthorizationFilter
        {
            public void OnAuthorization(AuthorizationFilterContext context)
            {
                var useremail = context.HttpContext.Session.GetString("Email");
                if (string.IsNullOrEmpty(useremail))
                {
                    context.Result = new RedirectToActionResult("Login", "Admin", new { area = "manage" });
                }
            }
        }
    }
}
