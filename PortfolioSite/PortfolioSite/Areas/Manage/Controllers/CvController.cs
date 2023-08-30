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
    public class CvController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;

        public CvController(IWebHostEnvironment env, AppDbContext context)
        {
            _env = env;
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var vm = new HomeVM
            {
                Cvs = await _context.Cvs.FirstOrDefaultAsync(),
                CVmodel = await _context.CVmodels.FirstOrDefaultAsync()
            };
            return View(vm);
        }

        [HttpGet]
        [AdminAuthorize]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Please select a file.");
                return View();
            }

            var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
            var fileExtension = Path.GetExtension(file.FileName);
            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError(string.Empty, "Invalid file format. Only PDF, DOC, and DOCX files are allowed.");
                return View();
            }


            var filename = Path.GetFileNameWithoutExtension(file.FileName) + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/cv", filename);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var cv = new Cv
            {
                FileName = filename,
                Name = file.FileName
            };

            _context.Cvs.Add(cv);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var cv = await _context.Cvs.FindAsync(id);
            if (cv == null)
                return NotFound();

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "cv", cv.FileName);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _context.Cvs.Remove(cv);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Generate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Generate(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Please select a file.");
                return View();
            }
            var allowedExtensions = new[] { ".pdf", "doc", "docx" };
            var fileExtension = Path.GetExtension(file.FileName);

            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError(string.Empty, "Invalid file format. Only PDF, DOC, and DOCX files are allowed.");
                return View();
            }

            var filename = Path.GetFileNameWithoutExtension(file.FileName) + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/cv", filename);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var cvmodel = new CVmodel
            {
                FileName = filename,
                Name = file.FileName
            };

            _context.CVmodels.Add(cvmodel);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Del(int id)
        {
            var cvmodel = await _context.CVmodels.FindAsync(id);

            if (cvmodel == null)
                return NotFound();

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "cv", cvmodel.FileName);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _context.CVmodels.Remove(cvmodel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
