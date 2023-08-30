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
    public class ExpressionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ExpressionController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Testimonials = await _context.Testimonials.FirstOrDefaultAsync(),
                TestimonialsEdits = await _context.TestimonialsEdits.ToListAsync()
            };
            return View(homeVM);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Testimonials testimonials)
        {
            _context.Testimonials.Add(testimonials);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var x = await _context.Testimonials.FindAsync(id);
            return View(x);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Testimonials testimonials)
        {
            var existing = await _context.Testimonials.FindAsync(testimonials.Id);
            existing.Title = testimonials.Title;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var delete = await _context.Testimonials.FindAsync(id);
            _context.Remove(delete);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Pcreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pcreate(TestimonialsEdit testimonialsEdit, IFormFile file)
        {

            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "img", "testimonials", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    testimonialsEdit.Img = fileName;
                }
                _context.Add(testimonialsEdit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(testimonialsEdit);
        }

        [HttpGet]
        public async Task<IActionResult> Pupdate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var x = await _context.TestimonialsEdits.FindAsync(id);

            if (x == null)
            {
                return NotFound();
            }
            return View(x);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pupdate(int id, TestimonialsEdit testimonialsEdit, IFormFile file)
        {
            if (id != testimonialsEdit.Id)
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
                        var filePath = Path.Combine(_env.WebRootPath, "assets", "img", "testimonials", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        testimonialsEdit.Img = fileName;
                    }
                    _context.Update(testimonialsEdit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutExists(testimonialsEdit.Id))
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
            return View(testimonialsEdit);
        }

        public bool AboutExists(int id)
        {
            return _context.TestimonialsEdits.Any(a => a.Id == id);
        }


        public async Task<IActionResult> Pdelete(int id)
        {
            var del = await _context.TestimonialsEdits.FindAsync(id);
            _context.Remove(del);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
