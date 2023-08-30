using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioSite.DAL;
using PortfolioSite.Models;
using PortfolioSite.ViewsModel;
using System.Data;
using static PortfolioSite.Areas.Manage.Controllers.AdminController;

namespace PortfolioSite.Areas.Manage.Controllers
{
    [Area("manage")]
    [AdminAuthorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;

        public DashboardController(AppDbContext appDbContext, IWebHostEnvironment env)
        {
            _appDbContext = appDbContext;
            _env = env;
        }

        [AdminAuthorize]
        public async Task<IActionResult> Index()
        {
            string email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Admin");
            }

            HomeVM vM = new HomeVM
            {
                Favorite = await _appDbContext.Favorites.FirstOrDefaultAsync()
            };
            return View(vM);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Favorite favorite)
        {
            if (ModelState.IsValid)
            {
                _appDbContext.Favorites.Add(favorite);
                _appDbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(favorite);
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var model = await _appDbContext.Favorites.FindAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Favorite favoritemodel)
        {
            var existing = await _appDbContext.Favorites.FirstOrDefaultAsync();
            existing.Title = favoritemodel.Title;
            existing.Description = favoritemodel.Description;

            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Dashboard");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            Favorite favorite = await _appDbContext.Favorites.FindAsync(id);
            _appDbContext.Favorites.Remove(favorite);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Dashboard");
        }



    }
}
