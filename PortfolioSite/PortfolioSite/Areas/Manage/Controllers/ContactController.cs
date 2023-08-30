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
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;
        public ContactController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            HomeVM vM = new HomeVM
            {
                ContactLocations = await _context.ContactLocations.ToListAsync()
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
        public async Task<IActionResult> Create(ContactLocation contactLocation)
        {
            _context.Add(contactLocation);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var x = await _context.ContactLocations.FindAsync(id);
            return View(x);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ContactLocation contact)
        {
            var ex = await _context.ContactLocations.FindAsync(contact.Id);
            ex.Icon = contact.Icon;
            ex.Name = contact.Name;
            ex.Description = contact.Description;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var del = await _context.ContactLocations.FindAsync(id);
            _context.Remove(del);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
