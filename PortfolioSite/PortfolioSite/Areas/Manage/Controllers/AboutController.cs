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
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AboutController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        public async Task<IActionResult> Index()
        {
            HomeVM home = new HomeVM
            {
                About = await _context.Abouts.FirstOrDefaultAsync()
            };
            return View(home);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(About about, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "img", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    about.Img = fileName;
                }
                _context.Add(about);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(about);
        }


        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var about = await _context.Abouts.FindAsync(id);
            if (about == null)
            {
                return NotFound();
            }
            return View(about);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, About about, IFormFile file)
        {
            if (id != about.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null && file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(_env.WebRootPath, "assets", "img", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        about.Img = fileName;
                    }
                    _context.Update(about);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutExists(about.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(about);
        }


        private bool AboutExists(int id)
        {
            return _context.Abouts.Any(a => a.Id == id);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var delete = await _context.Abouts.FindAsync(id);
            _context.Abouts.Remove(delete);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



    }
}
