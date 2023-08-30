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
    public class SkillController : Controller
    {
        private readonly AppDbContext _context;
        public SkillController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Skill = await _context.Skills.FirstOrDefaultAsync(),
                Percents = await _context.Percents.ToListAsync()
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
        public async Task<IActionResult> Create(Skill skill)
        {
            _context.Add(skill);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var x = await _context.Skills.FindAsync(id);
            return View(x);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Skill skill)
        {
            var ex = await _context.Skills.FindAsync(skill.Id);
            ex.Title = skill.Title;
            ex.Description = skill.Description;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(int id)
        {
            var delete = await _context.Skills.FindAsync(id);
            _context.Skills.Remove(delete);
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
        public async Task<IActionResult> Pcreate(Percent percent)
        {
            _context.Add(percent);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Pupdate(int id)
        {
            var x = await _context.Percents.FindAsync(id);
            return View(x);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pupdate(Percent percent)
        {
            var existing = await _context.Percents.FindAsync(percent.Id);
            existing.Name = percent.Name;
            existing.SPercent = percent.SPercent;
            existing.Min = percent.Min;
            existing.Max = percent.Max;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Pdelete(int id)
        {
            var delete = await _context.Percents.FindAsync(id);
            _context.Remove(delete);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
